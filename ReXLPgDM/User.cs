using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ReXLPgDM
{
    public class User
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Mobile { get; set; }
        public DateTimeOffset DateOfJoining { get; set; }
        public DateTimeOffset? DateOfLeft { get; set; }
        public string? About { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public Gender Gender { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string? AadharNumber { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact1Name { get; set; }
        public string? EmergencyContact1Mobile { get; set; }
        public string? EmergencyContact2Name { get; set; }
        public string? EmergencyContact2Mobile { get; set; }
        public string? PurposeOfStay { get; set; }
        public string? WorkPlaceName { get; set; }
        public string? WorkingPlaceId { get; set; }
        public string Password { get; set; }
        public UserRole Designation { get; set; }
        public bool Active { get; set; }
        public bool LoginAllowed { get; set; } = false;
    }
    public class UserDocument
    {
        public Guid UserId { get; set; }
        public byte[]? UserImage { get; set; }
        public byte[]? AadharFrontImage { get; set; }
        public byte[]? AadharBackImage { get; set; }
        public byte[]? WorkIdFrontImage { get; set; }
        public byte[]? WorkIdBackImage { get; set; }
    }
    public enum Gender
    {
        Unknown,
        Male,
        Female,
        Transgender
    }
    public enum UserRole
    {
        SuperAdmin = 1,
        BusinessOwner,
        Auditor,
        Admin,
        Supervisor,
        WalkinCustomer,
        RegularCustomer
    }
}
