using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ReXLPgDM
{
    public abstract class EnquiryDataModel
    {
        public User User { get; set; }
        public int DurationOfStayInMonths { get; set; }
        public EnquiryStatus Status { get; set; }
        [NotMapped]
        public List<Comments> Notes { get; set; }
    }
    public class Enquiry : EnquiryDataModel
    {
        public Guid GUID { get; set; }
        public Guid UserId { get; set; }
    }

    public enum EnquiryStatus
    {
        Open,
        Canceled,
        Converted
    }
}
