using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReXL.Util
{
    public class ExceptionInterceptor : ExceptionProcessorInterceptor<SqlException>
    {
        private const int ReferenceConstraint = 547;
        private const int CannotInsertNull = 515;
        private const int CannotInsertDuplicateKeyUniqueIndex = 2601;
        private const int CannotInsertDuplicateKeyUniqueConstraint = 2627;
        private const int ArithmeticOverflow = 8115;
        private const int StringOrBinaryDataWouldBeTruncated = 8152;
        protected override DatabaseError? GetDatabaseError(SqlException dbException)
        {
            return dbException.Number switch
            {
                ReferenceConstraint => DatabaseError.ReferenceConstraint,
                CannotInsertNull => DatabaseError.CannotInsertNull,
                CannotInsertDuplicateKeyUniqueIndex => DatabaseError.UniqueConstraint,
                CannotInsertDuplicateKeyUniqueConstraint => DatabaseError.UniqueConstraint,
                ArithmeticOverflow => DatabaseError.NumericOverflow,
                StringOrBinaryDataWouldBeTruncated => DatabaseError.MaxLength,
                _ => null
            };
        }
    }
}
