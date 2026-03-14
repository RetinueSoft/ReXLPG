using ReXLPgDM;

namespace ReXLPgAPI.Models
{
    public class LoginModel
    {
        public string Mobile { get; set; }
        public string Password { get; set; }
    }
    public class UserCreateModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        //public Gender Gender { get; set; }
        public string AadharNumber { get; set; }
        public int EnquirForMonths { get; set; }
    }
    public class UserEditModel : UserCreateModel
    {
        public Guid GUID { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Address { get; set; }
        public string EmergencyContact1Name { get; set; }
        public string EmergencyContact1Mobile { get; set; }
        public string EmergencyContact2Name { get; set; }
        public string EmergencyContact2Mobile { get; set; }
        public string PurposeOfStay { get; set; }
        public string WorkPlaceName { get; set; }
        public string WorkingPlaceId { get; set; }

    }

    public class UserDocumentModel : UserDocument { }
}
