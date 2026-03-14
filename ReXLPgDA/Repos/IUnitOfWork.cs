using Microsoft.EntityFrameworkCore;
using ReXLPgDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReXLPgDA.Repos
{
    public interface IUnitOfWork : IDisposable
    {
        Lazy<Repository<User>> UserTBL { get; }
        Lazy<Repository<UserDocument>> UserDocumentTBL { get; }
        Lazy<Repository<Enquiry>> EnquiryTBL { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        List<T1> GetByQuery<T1>(string query);
    }
}
