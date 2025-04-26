using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.Models
{
    public class User
    {
        public User()
        {
            Posts = new List<Post>();
            UserCommunities = new List<UserCommunity>();
            Friendships = new List<Friendship>();
        }

        [Key]
        public int UserId { get; set; }

        public string UserPhone { get; set; }

        public string FullName { get; set; }

        public string UserPhoto { get; set; }

        public DateTime UserBirth { get; set; }

        public string UserAbout { get; set; }

        public string City { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserCommunity> UserCommunities { get; set; }
        public virtual ICollection<Friendship> Friendships { get; set; }
    }

}
