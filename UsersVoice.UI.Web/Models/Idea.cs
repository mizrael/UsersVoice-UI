using System;
using System.Collections.Generic;

namespace UsersVoice.UI.Web.Models
{
    public class Idea
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid AreaId { get; set; }
        public string AreaTitle { get; set; }
        
        public int TotalPoints { get; set; }
        public int TotalComments { get; set; }

        
        public string AuthorCompleteName { get; set; }
        public Guid AuthorId { get; set; }
        
        public IEnumerable<Comment> Comments { get; set; }

        public IList<Vote> Votes { get; set; } 
    }

    public class IdeaFilter
    {
        public IdeaFilter()
        {
            this.Page = 0;
            this.PageSize = 10;
        }
        public int PageSize { get; set; }
        public int Page { get; set; }

        public Guid AreaId { get; set; }

        public IdeaSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public enum IdeaSortBy
    {
        None,
        Title,
        CreationDate,
        Points
    }

    public enum UserSortBy
    {
        None,
        Name,
        Points,
        RegistrationDate
    }

    public enum SortDirection
    {
        ASC,
        DESC
    }

    public class CreateIdeaCommand
    {
        public CreateIdeaCommand(Guid ideaId, Guid areaId, Guid authorId, string title, string description)
        {
            if (ideaId == Guid.Empty) throw new ArgumentNullException("ideaId");
            if (areaId == Guid.Empty) throw new ArgumentNullException("areaId");
            if (authorId == Guid.Empty) throw new ArgumentNullException("authorId");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException("title");

            this.Title = title;

            this.Description = description ?? string.Empty;

            this.AuthorId = authorId;
            this.AreaId = areaId;
            this.IdeaId = ideaId;
        }

        public string Title { get; private set; }
        public string Description { get; set; }

        public Guid AuthorId { get; private set; }
        public Guid AreaId { get; private set; }
        public Guid IdeaId { get; private set; }
    }
}