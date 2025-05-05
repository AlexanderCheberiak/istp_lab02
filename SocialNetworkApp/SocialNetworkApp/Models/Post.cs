namespace SocialNetworkApp.Models
{
    public class Post
    {
        public int PostId { get; set; }

        public int CommunityId { get; set; }

        public int AuthorId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Community? Community { get; set; }
        public virtual User? Author { get; set; }

    }
}
