using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_AYNI.Models
{
    public class ForumModel
    {
        [Key]
        public string? Id { get; set; }
        public string? topic { get; set; }
        public string content { get; set; }

        public string UserId { get; set; }
        public string? fatherId { get; set; }

        [ForeignKey("UserId")]
        public UserModel? owner { get; set; }
        [ForeignKey("fatherId")]
        public ForumModel? father { get; set; }
    }
}
