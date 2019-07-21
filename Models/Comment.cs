using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggster.Models
{
    public class Comment
    {
        [Key]
        public int CommentID {get;set;}

        [Required]
        public string Content {get;set;}

        [Required]
        public DateTime CreatedAt {get;set;}

        [Required]
        public DateTime UpdatedAt {get;set;}

        public int UserID {get;set;}

        public int PostID {get;set;}

        [ForeignKey("PostID")]
        public virtual Post Post {get;set;}

        [ForeignKey("UserID")]
        public virtual User User {get;set;}

    }
}