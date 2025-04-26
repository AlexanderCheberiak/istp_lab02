using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.Models
{
    public class UserCommunity
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int CommunityId { get; set; }

        public DateTime JoinedAt { get; set; }

        public string Role { get; set; }

        public virtual User User { get; set; }
        public virtual Community Community { get; set; }
    }
}
