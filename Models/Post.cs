using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bloggster.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        public int PostID {get;set;}
        public int BloggerID {get;set;}

        [Required(ErrorMessage="Title cannot be empty")]
        [MinLength(1)]
        public string Title {get;set;}

        [Required(ErrorMessage="Body cannoy be empty")]
        [MinLength(30)]
        public string Body {get;set;}

        [Required]
        public string Image {get;set;}

        [Required]
        public string ImageText {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public User Blogger {get;set;}
        public List<Comment> Comments {get;set;}

    }
}