﻿namespace Zante_Hotel.Models
{
    public class Reply:BaseEntity
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid CommentId { get; set; }
        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }
        [ScaffoldColumn(false)]
        public DateTime CreateOn { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }
}