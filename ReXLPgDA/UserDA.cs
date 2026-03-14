using Microsoft.Extensions.DependencyInjection;
using ReXLPgDA.Repos;
using ReXLPgDM;
using System;

namespace ReXLPgDA
{
    public interface IUserDA
    {
        public void Add(User user);
        public void Update(User user);
        public void UpdateDocument(UserDocument documents);
        public User Get(Guid userId);
        public User Get(string mobileNo);
        public UserDocument GetDocuments(Guid userId);
        public bool TryFeatchDetailForLogin(ref User user);
    }
    public class UserDA : IUserDA
    {
        private readonly IUnitOfWork _uW;
        public UserDA(IServiceProvider serviceProvider)
        {
            _uW = serviceProvider.GetService<IUnitOfWork>();
        }

        public void Add(User user) => _uW.UserTBL.Value.Insert(user);
        public void Update(User user) => _uW.UserTBL.Value.Update(_uW.UserTBL.Value.GetById(user.GUID), user);
        public void UpdateDocument(UserDocument documents)
        {
            var oldDoc = _uW.UserDocumentTBL.Value.GetById(documents.UserId);
            if (oldDoc == null)
                _uW.UserDocumentTBL.Value.Insert(documents);
            else
                _uW.UserDocumentTBL.Value.Update(oldDoc, documents);
        }
        public User Get(Guid userId) => _uW.UserTBL.Value.GetById(userId);
        public User Get(string mobileNo) => _uW.UserTBL.Value.GetFirst(x=>x.Mobile == mobileNo);
        public UserDocument GetDocuments(Guid userId) => _uW.UserDocumentTBL.Value.GetFirst(x => x.UserId == userId, true);
        public bool TryFeatchDetailForLogin(ref User user)
        {
            var mobile = user.Mobile; var password = user.Password;
            var userDetail = _uW.UserTBL.Value.GetFirst(x => (x.Mobile == mobile && x.Password == password && x.Active), true);
            if (userDetail != null)
                user = userDetail;

            return (userDetail != null);
        }
    }
}
