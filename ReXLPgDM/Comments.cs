using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ReXLPgDM
{
    public abstract class CommentsDataModel
    {
        public string Notes { get; set; }
        public DateTimeOffset  PostedOn { get; set; }
        public Activity Activity { get; set; }
    }
    public class Comments : CommentsDataModel
    {
        public Guid GUID { get; set; }
        public Guid UserId { get; set; }
        public Guid DocumentId { get; set; }
    }

    public enum Activity
    {
        Enquiry,
        Invoice
    }
}
