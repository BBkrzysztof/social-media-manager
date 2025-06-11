namespace SocialMediaManager.Services.Exceptions
{
    public class SocialMediaException : Exception
    {
        public SocialMediaException(string message, Exception? inner = null)
            : base(message, inner) { }
    }
}
