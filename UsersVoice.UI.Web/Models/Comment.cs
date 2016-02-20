using System;

namespace UsersVoice.UI.Web.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid IdeaId { get; set; }
        public string IdeaTitle{ get; set; }
        
        public Guid AuthorId { get; set; }
        public string AuthorCompleteName { get; set; }
    }

    public class CommentFilter
    {
        public CommentFilter()
        {
            this.Page = 0;
            this.PageSize = 10;
        }
        public int PageSize { get; set; }
        public int Page { get; set; }

        public Guid IdeaId { get; set; }
        public Guid AuthorId { get; set; }
    }
}