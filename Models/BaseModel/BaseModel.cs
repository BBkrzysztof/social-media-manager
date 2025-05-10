namespace SocialMediaManager.Models;

using System;
using System.ComponentModel.DataAnnotations;


public abstract class BaseModel{

        [Key]
        public Guid Id {get; set;} = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
