using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System;
using ReXLPgDM;
using FluentValidation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static ReXLPgDAS.UserService;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using ReXLPgDA;

namespace ReXLPgDAS
{
    public abstract class UserValidator : AbstractValidator<User>
    {
        public static Regex PasswrodFor_Level2Security { get { return new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"); } }
        public static Regex MobileFor { get { return new Regex(@"^[6-9]\d{9}$"); } }

        public User ValidateExist(QSContext qS)
        {
            var oldUser = new User();
            RuleFor(x => x).Must(x =>
            {
                oldUser = qS.User.Value.Get(x.Name);
                return oldUser != null;
            }).WithMessage("User not valid");

            return oldUser;
        }
    }
    public class UserInsertValidator : UserValidator
    {
        public static UserInsertValidator Instance { get { return new UserInsertValidator(); } }
        public UserInsertValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Mobile).NotEmpty().WithMessage("Mobile number is required");

            RuleFor(x => x.Password).NotEmpty().When(x => x.LoginAllowed)
            .WithMessage("Password is required");

            RuleFor(x => x.Designation).Must(x=> x >= UserRole.Auditor)
           .WithMessage("Cannot create this designation user, contact admin");

            RuleFor(x => x.Password).Must(x => PasswrodFor_Level2Security.IsMatch(x)).When(x => x.Password.Trim() != "" && x.LoginAllowed)
            .WithMessage("Password should be,\n  * Minimum eight characters\n  * At least one letter\n  * At least one number\n  * At least on special character");
            
            RuleFor(x => x.Mobile).Must(x => MobileFor.IsMatch(x)).When(x => x.Mobile.Trim() != "")
            .WithMessage("Mobile number is not valid");
        }        
    }
    public class UserUpdateValidator : UserValidator
    {
        public UserUpdateValidator(User oldUser)
        {
            RuleFor(x => x).Must(x => oldUser != null && oldUser.Active)
            .WithMessage("User not valid");

            RuleFor(x => x).Custom((x, context) =>
            {
                var result = UserInsertValidator.Instance.Validate(context.InstanceToValidate);
                if (!result.IsValid)
                    result.Errors.ForEach(y => context.AddFailure(y));
            });

            RuleFor(x => x.Designation).Must(newDesignation =>
            {
                var oldDesignation = oldUser.Designation;
                if ((oldDesignation == UserRole.WalkinCustomer || oldDesignation == UserRole.RegularCustomer) &&
                    (newDesignation == UserRole.WalkinCustomer || newDesignation == UserRole.RegularCustomer))
                    return true;

                if (oldDesignation == UserRole.SuperAdmin && newDesignation == UserRole.SuperAdmin)
                    return true;

                return false;
            }).WithMessage("User desingnation cannot change");

            RuleFor(x => x.AadharNumber).NotEmpty().WithMessage("Aadhar Number is required");

            RuleFor(x => x.DateOfBirth).Must(dob => (dob.Value <= DateTime.UtcNow.AddYears(-10)))
                .When(x => x.DateOfBirth.HasValue)
                .WithMessage("Not a valid age, check your date of birth");
        }
    }
    public class UserDocumentValidator : AbstractValidator<UserDocument>
    {
        public UserDocumentValidator()
        {
            RuleFor(x => x.AadharFrontImage).NotEmpty().WithMessage("Aadhar Front Image is required");
            RuleFor(x => x.AadharBackImage).NotEmpty().WithMessage("Aadhar Back Image is required");
            RuleFor(x => x.WorkIdFrontImage).NotEmpty().WithMessage("Work ID Image is required");
        }
    }
    public class UserActivateValidator : UserValidator
    {
        public UserActivateValidator()
        {
            var oldUser = ValidateExist(QSContext.Instance);
            RuleFor(x => oldUser.Active).Must(x => !x)
            .WithMessage("User already actived");
        }
    }
    public class UserDeActivateValidator : UserValidator
    {
        public UserDeActivateValidator()
        {
            var oldUser = ValidateExist(QSContext.Instance);
            RuleFor(x => oldUser.Active).Must(x => x)
            .WithMessage("User already deactived");
        }
    }
}
