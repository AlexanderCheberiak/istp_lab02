using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.Models
{
    public class Community
    {
        public Community()
        {
            Posts = new List<Post>();
            UserCommunities = new List<UserCommunity>();
        }

        [Key]
        public int CommunityId { get; set; }

        [Required]
        public string CommunityName { get; set; }

        public string CommunityDescription { get; set; }

        public DateTime CommunityCreatedAt { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserCommunity> UserCommunities { get; set; }
    }
}
