using System;

namespace UsersVoice.UI.Web.Models
{
    public class Area
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorCompleteName { get; set; }
        public Guid AuthorId { get; set; }
    }
}