using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReXLPgDAS.Util
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T, T1>() where T1 : IValidator<T>, new();
        public IValidator<T> GetValidator<T, T1>(params object[] args);
    }
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator<T> GetValidator<T, T1>() where T1 : IValidator<T>, new()
        {
            try
            {
                return new T1();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"No validator found for type {typeof(T1).Name} \n More Detail {ex.Message}");
            }
        }
        public IValidator<T> GetValidator<T, T1>(params object[] args)
        {
            try
            {
                var validatorInstance = Activator.CreateInstance(typeof(T1), args);
                return (IValidator<T>)validatorInstance!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Cannot create validator for type {typeof(T1).Name}. More detail: {ex.Message}", ex);
            }
        }
    }
}
