using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.Models
{
    public class Friendship
    {
        [Key]
        public int UserId1 { get; set; }

        [Key]
        public int UserId2 { get; set; }

        public string Status { get; set; }

        public DateTime RequestedAt { get; set; }

        public virtual User? User1 { get; set; }
        public virtual User? User2 { get; set; }
    }
}
