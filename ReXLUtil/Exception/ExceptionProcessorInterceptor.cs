using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ReXL.Util
{
    public abstract class ExceptionProcessorInterceptor<T> : SaveChangesInterceptor where T : DbException
    {
        protected internal enum DatabaseError
        {
            UniqueConstraint,
            CannotInsertNull,
            MaxLength,
            NumericOverflow,
            ReferenceConstraint
        }

        protected abstract DatabaseError? GetDatabaseError(T dbException);

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        public override void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            var dbUpdateException = eventData.Exception as DbUpdateException;
            var pp = eventData.Exception.GetBaseException();
            if (eventData.Exception.GetBaseException() is T providerException)
            {
                var error = GetDatabaseError(providerException);

                if (error != null && dbUpdateException != null)
                {
                    var exception = ExceptionFactory.Create(error.Value, dbUpdateException, dbUpdateException.Entries.Select(x => x.GetInfrastructure()).ToList());
                    throw exception;
                }
            }

            base.SaveChangesFailed(eventData);
        }



        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = new CancellationToken())
        {
            var dbUpdateException = eventData.Exception as DbUpdateException;

            if (eventData.Exception.GetBaseException() is T providerException)
            {
                var error = GetDatabaseError(providerException);

                if (error != null && dbUpdateException != null)
                {
                    //var pp = dbUpdateException.Entries[0].GetInfrastructure<IUpdateEntry>();
                    var exception = ExceptionFactory.Create(error.Value, dbUpdateException, dbUpdateException.Entries.Select(x => x.GetInfrastructure()).ToList());
                    throw exception;
                }
            }

            return base.SaveChangesFailedAsync(eventData, cancellationToken);
        }


    }
}
