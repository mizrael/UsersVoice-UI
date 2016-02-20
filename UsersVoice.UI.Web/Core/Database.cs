using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersVoice.UI.Web.Models;

namespace UsersVoice.UI.Web.Core
{
    public static class Database
    {
        private static List<Area> _areas;
        private static List<Idea> _ideas;
        private static List<Comment> _comments;
        private static List<User> _users;
 
        static Database()
        {
            _users = new List<User>();
            var firstNamesIndex = 0;
            var lastNamesIndex = 0;
            var firstNames =
                new string[]
                {
                    "John", "Paul", "Ringo", "Jack", "David", "Jim", "Johnny", "Brad", "Kim", "Jimmy", "Robert", "Freddy",
                    "Matthew", "Nicholas", "Mike"
                }.OrderBy(n => Guid.NewGuid()).ToArray();
            var lastNames =
                new string[]
                {
                    "Jones", "Mercury", "Starr", "Page", "Pitt", "Cage", "Bones", "Doe", "Bonham", "White", "Plant",
                    "Portnoy", "Grohl", "May"
                }.OrderBy(n => Guid.NewGuid()).ToArray();
            for (int x = 0; x < 50; x++)
            {
                var firstName = firstNames[(firstNamesIndex++) % firstNames.Length];
                var lastName = lastNames[(lastNamesIndex++) % lastNames.Length];

                var username = string.Format("{0}.{1}", firstName, lastName).ToLower();
                var email = string.Format("{0}@usersvoice.com", username);

                _users.Add(new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Id = Guid.NewGuid(),
                    AvailablePoints = 10,
                    Email = email,
                    IsAdmin = false
                });
            }

            _users.Add(new User()
            {
                FirstName = "admin",
                LastName = "admin",
                Id = Guid.NewGuid(),
                AvailablePoints = 10,
                Email = "admin@usersvoice.com",
                IsAdmin = true
            });

            _users.Add(new User()
            {
                FirstName = "user",
                LastName = "user",
                Id = Guid.NewGuid(),
                AvailablePoints = 10,
                Email = "user@usersvoice.com",
                IsAdmin = false
            });

            _areas = new List<Area>();

            for (int x = 0; x < 10; x++)
            {
                var author = _users.OrderBy(u => Guid.NewGuid()).First();
                _areas.Add(new Area()
                {
                    Id = Guid.NewGuid(),
                    Title = string.Format("Area {0} Title", x),
                    Description = string.Format("Area {0}Description", x),
                    AuthorId = author.Id,
                    AuthorCompleteName = author.CompleteName
                });
            }

            _ideas = new List<Idea>();
            var description = string.Join(" ", Enumerable.Repeat("lorem ipsum dolor", 50));
            foreach (var area in _areas)
            {
                for (int i = 0; i != 5; ++i)
                {
                    var author = _users.OrderBy(u => Guid.NewGuid()).First();
                    _ideas.Add(new Idea()
                    {
                        AreaId = area.Id,
                        Id = Guid.NewGuid(),
                        AuthorId = author.Id,
                        AuthorCompleteName = author.CompleteName,
                        CreationDate = DateTime.Now,
                        Title = string.Format("Idea {0} for {1}", i + 1, area.Title),
                        Description = description
                    });
                }
                    
            }

            _comments = new List<Comment>();
            var text = string.Join(" ", Enumerable.Repeat("lorem ipsum dolor", 20));
            foreach (var idea in _ideas)
            {
                for (int i = 0; i != 5; ++i)
                {
                    var author = _users.OrderBy(u => Guid.NewGuid()).First();
                    _comments.Add(new Comment()
                    {
                        IdeaId = idea.Id,
                        CommentId = Guid.NewGuid(), 
                        CreationDate = DateTime.Now,
                        AuthorId = author.Id,
                        AuthorCompleteName = author.CompleteName,
                        Text = text
                    });
                }
                   
            }
        }

        public static async Task<IEnumerable<Idea>> GetIdeasAsync(Guid areaID)
        {
            var ideas = _ideas.Where(i => i.AreaId == areaID).OrderByDescending(i => i.TotalPoints).ToList();

            foreach (var idea in ideas)
            {
                idea.TotalPoints = (idea.Votes == null) ? 0 : idea.Votes.Sum(v => v.Points);
                idea.TotalComments = (idea.Comments == null) ? 0 : idea.Comments.Count();
            }

            return ideas;
        }


        public static async Task<IEnumerable<Idea>> GetLatestIdeasAsync(int pageSize)
        {
            var ideas = _ideas.Take(pageSize).OrderByDescending(i => i.CreationDate).ToList();

            foreach (var idea in ideas)
            {
                idea.TotalPoints = (idea.Votes == null) ? 0 : idea.Votes.Sum(v => v.Points);
                idea.TotalComments = (idea.Comments == null) ? 0 : idea.Comments.Count();
            }

            return ideas;
        }

        public static async Task<IEnumerable<Idea>> GetIdeasByUserAsync(Guid userId, int pageSize = 10)
        {
            var ideas = _ideas
                .Where(i => i.AuthorId == userId)
                .OrderByDescending(i => i.TotalPoints);

            foreach (var idea in ideas)
            {
                idea.TotalPoints = (idea.Votes == null) ? 0 : idea.Votes.Sum(v => v.Points);
                idea.TotalComments = (idea.Comments == null) ? 0 : idea.Comments.Count();
            }

            return ideas;
        }

        public static async Task<Idea> GetIdeaAsync(Guid ideaId)
        {
            var idea = _ideas.FirstOrDefault(c => c.Id == ideaId);
            if(null != idea)
                idea.Comments = await GetCommentsAsync(idea.Id);
            return idea;
        }

        public static async Task<IEnumerable<Comment>> GetCommentsAsync(Guid ideaId)
        {
            return _comments.Where(c => c.IdeaId == ideaId).ToArray();
        }

        public static async Task SaveCommentAsync(Comment comment)
        {
            if (null == comment) return;
            if (comment.CommentId == Guid.Empty) comment.CommentId = Guid.NewGuid();
            _comments.Add(comment);
        }

        public static async Task<IEnumerable<Area>> GetAreasAsync()
        {
            return _areas;
        }
        
        public static async Task AddArea(Area area)
        {
            _areas.Add(area);
        }

        public static async Task VoteIdea(Vote vote)
        {
            if (null == vote) 
                return;
            var idea = _ideas.FirstOrDefault(i => i.Id == vote.IdeaId);
            if (null == idea)
                return;
            idea.Votes = idea.Votes ?? new List<Vote>();

            vote.CreationDate = DateTime.Now;
            idea.Votes.Add(vote);
        }

        public static async Task<User> GetUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(c => c.Email == email);
            return user;

		}

        public static async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = _users.FirstOrDefault(c => c.Id == id);
            return user;
        }

        public static async Task AddIdea(Idea idea)
        {
            _ideas.Add(idea);
        }

        public static async Task<bool> HasVoted(Guid ideaId, Guid userId)
        {
            var idea = _ideas.First(i => i.Id == ideaId);

            if (idea.Votes == null)
                return false;

            return idea.Votes.FirstOrDefault(v => v.VoterId == userId) != null;
        }

        public static async Task<User> AddVote(Vote newVote)
        {
            var user = await GetUserByIdAsync(newVote.VoterId);

            var idea = _ideas.First(i => i.Id == newVote.IdeaId);

            if (newVote.Points == 0)
            {
                var vote = idea.Votes.First(v => v.VoterId == newVote.VoterId);

                idea.Votes.Remove(vote);

                user.AvailablePoints += vote.Points;
            }
            else
            {
                if (await HasVoted(newVote.IdeaId, newVote.VoterId))
                    return user;

                if (idea.Votes == null)
                    idea.Votes = new List<Vote>();

                newVote.CreationDate = DateTime.Now;

                idea.Votes.Add(newVote);

                user.AvailablePoints -= newVote.Points;
            }

            return user;
        }
    }

}