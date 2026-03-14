using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReXLPgDA;
using ReXLPgDA.Repos;
using ReXLPgDAS.Util;
using ReXLPgDM;

namespace ReXLPgDAS
{
    public interface IUserService
    {
        public void Add(User user, bool verificationNeeded, int? enquiryMonths = null);
        public void EditProfile(User user);
        public void UpdatePassword(Guid userId, string newPassword);
        public void UpdateDocument(UserDocument userDocuments);
        public void Activate(string mobileNo);
        public void DeActivate(string mobileNo);
        public bool TryFeatchDetailForLogin(ref User user);
    }

    public class UserService: IUserService
    {
        private readonly QSContext _qS;
        private readonly Lazy<IUserDA> _userDA;
        private readonly Lazy<IEnquiryDA> _enquiryDA;
        private readonly IUnitOfWork _uW;
        private readonly IValidatorFactory _validatorFactory;
        private readonly IConfiguration _config;
        public UserService(IServiceProvider serviceProvider, IConfiguration config)
        {
            _qS = QSContext.Instance;

            _uW = serviceProvider.GetService<IUnitOfWork>();
            _validatorFactory = serviceProvider.GetService<IValidatorFactory>();
            _userDA = new Lazy<IUserDA>(() => serviceProvider.GetService<IUserDA>());
            _enquiryDA = new Lazy<IEnquiryDA>(() => serviceProvider.GetService<IEnquiryDA>());

            _qS.User = _userDA;
        }

        public void Add(User user, bool verificationNeeded, int? enquiryMonths = null)
        {
            _validatorFactory.GetValidator<User, UserInsertValidator>().ValidateAndThrow(user);
            user.Active = true;
            user.LoginAllowed = !verificationNeeded;

            _uW.BeginTransaction();
            _userDA.Value.Add(user);

            if (enquiryMonths.HasValue)
            {
                var enq = new Enquiry() { UserId = user.GUID, DurationOfStayInMonths = enquiryMonths.Value};
                _enquiryDA.Value.Add(enq);
            }
            _uW.Commit();
        }
        public void UpdateDocument(UserDocument userDocuments)
        {
            var user = _qS.User.Value.Get(userDocuments.UserId);
            _validatorFactory.GetValidator<UserDocument, UserDocumentValidator>().ValidateAndThrow(userDocuments);

            _uW.BeginTransaction();
            _userDA.Value.UpdateDocument(userDocuments);
            _uW.Commit();
        }

        public void UpdatePassword(Guid userId, string newPassword)
        {
            var user = _qS.User.Value.Get(userId);
            user.Password = newPassword;
            Edit(user, user);
        }
        public void EditProfile(User user)
        {
            var oldUser = _qS.User.Value.Get(user.GUID);
            user.Password = oldUser.Password;
            
            Edit(user, oldUser);
        }
        private void Edit(User user, User oldUser)
        {
            user.DisplayName = user.Name;
            user.Active = oldUser.Active;
            user.RegisterDate = oldUser.RegisterDate;
            user.DateOfJoining = oldUser.DateOfJoining;
            user.DateOfLeft = oldUser.DateOfLeft;
            user.Designation = oldUser.Designation;
            user.LoginAllowed = oldUser.LoginAllowed;

            _validatorFactory.GetValidator<User, UserUpdateValidator>(oldUser).ValidateAndThrow(user);

            _uW.BeginTransaction();
            _userDA.Value.Update(user);
            _uW.Commit();
        }
        public void Activate(string mobileNo)
        {
            var user = new User() { Mobile = mobileNo };
            _validatorFactory.GetValidator<User, UserActivateValidator>().ValidateAndThrow(user);

            user = _qS.User.Value.Get(mobileNo);
            user.Active = true;

            _uW.BeginTransaction();
            _userDA.Value.Update(user);
            _uW.Commit();
        }
        public void DeActivate(string mobileNo)
        {
            var user = new User() { Mobile = mobileNo };
            _validatorFactory.GetValidator<User, UserDeActivateValidator>().ValidateAndThrow(user);

            user = _qS.User.Value.Get(mobileNo);
            user.Active = false;

            _uW.BeginTransaction();
            _userDA.Value.Update(user);
            _uW.Commit();
        }
        public bool TryFeatchDetailForLogin(ref User user) => _userDA.Value.TryFeatchDetailForLogin(ref user);
    }
}
