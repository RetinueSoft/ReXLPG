using Microsoft.EntityFrameworkCore;
using ReXLPgDA;
using ReXLPgDA.Repos;
using ReXLPgDM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReXLPgDA
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            UserTBL = new Lazy<Repository<User>>(() => new Repository<User>(_context));
            UserDocumentTBL = new Lazy<Repository<UserDocument>>(() => new Repository<UserDocument>(_context));
            EnquiryTBL = new Lazy<Repository<Enquiry>>(() => new Repository<Enquiry>(_context));
        }
        public Lazy<Repository<User>> UserTBL { get; private set; }
        public Lazy<Repository<Enquiry>> EnquiryTBL { get; private set; }
        public Lazy<Repository<UserDocument>> UserDocumentTBL { get; private set; }
        public void Dispose()
        {
            _context.Dispose();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.SaveChanges();
            if (_context.Database.CurrentTransaction != null)
                _context.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _context.Database.RollbackTransaction();
        }
        public List<T1> GetByQuery<T1>(string query)
        {
            return _context.Database.SqlQuery<T1>(FormattableStringFactory.Create(query)).ToList();
        }
    }

    public class QSContext
    {
        static QSContext _instance;
        public static QSContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new QSContext();
                return _instance;
            }
        }

        private QSContext() { }

        public Lazy<IUserDA> User { get; set; }

        //public override bool Equals(object? obj)
        //{
        //    return obj is QSContext context &&
        //           EqualityComparer<Lazy<IGroupAttributeDQS>>.Default.Equals(GroupAttribute, context.GroupAttribute);
        //}

    }

}
