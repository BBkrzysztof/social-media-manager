using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using SocialMediaManager.Data;
using SocialMediaManager.Models;
using SocialMediaManager.Services;
using SocialMediaManager.Enum;

namespace SocialMediaManager.BgService.Scheduler;

public class Scheduler : BackgroundService
{
    private readonly ILogger<Scheduler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public Scheduler(ILogger<Scheduler> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("!!!!Database scan background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var matchingItems = dbContext.Posts
                    .Where(p => p.ScheduledAt <= DateTimeOffset.Now).ToList();

                _logger.LogInformation($"Found {matchingItems.Count} post to publish");

                foreach (Post post in matchingItems)
                {
                    var socialAccount = dbContext.SocialAccounts.Where(s => s.UserId == post.UserId && post.Platforms.Contains(s.Platform)).First();

                    if (post.Platforms.Contains(Platform.Facebook))
                    {
                        var socialPost = new SocialPost
                        {
                            PostId = post.Id,
                            Post = post,
                            SocialAccount = socialAccount,
                            SocialAccountId = socialAccount.Id
                        };
                        dbContext.SocialPosts.Add(socialPost);

                        var facebookService = scope.ServiceProvider.GetRequiredService<FacebookService>();

                        string postId = await facebookService.PostAsync(socialAccount, socialPost);

                        post.ScheduledAt = null;
                        socialPost.SocialPostId = postId;
                        socialPost.Status = SocialPostStatus.Send;

                        await dbContext.SaveChangesAsync();

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while scanning the database.");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Database scan background service stopping.");
    }

}