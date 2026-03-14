using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReXL.Util
{
    public static class ExceptionProcessorExtension
    {
        public static DbContextOptionsBuilder UseExceptionProcessor(this DbContextOptionsBuilder self)
        {
            return self.AddInterceptors(new ExceptionInterceptor());
        }

        public static DbContextOptionsBuilder<TContext> UseExceptionProcessor<TContext>(this DbContextOptionsBuilder<TContext> self) where TContext : DbContext
        {
            return self.AddInterceptors(new ExceptionInterceptor());
        }
    }
}
