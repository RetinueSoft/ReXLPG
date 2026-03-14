
using ReXL.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReXLPgAPI.Models;
using TupleExt = ReXL.Util.TupleExtensions;

namespace ReXLPgAPI.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        //private readonly IEventLogDBS _eventLogDBS;
        public ExceptionFilter(/*IEventLogDBS eventLogDBS*/)
        {
            //_eventLogDBS = eventLogDBS;
        }

        public override void OnException(ExceptionContext context)
        {
            var actionResponse = new ActionResponse();
            actionResponse.Success = false;
            //var logLevel = MotoC2BBO.Entities.LogLevel.AppError;
            var errorMessage = "";

            if(context.Exception is InternalInnerException)
            {
                errorMessage = context.Exception.Message;
                context.Exception = context.Exception.InnerException;
            }

            if (context.Exception is UniqueConstraintException)
            {
                var errorEntry = UniqueErrorFormatter(context.Exception as UniqueConstraintException);
                actionResponse.AddError(errorEntry.Value.ToString(), errorEntry.Key.ToString());
            }
            else if (context.Exception is ReferenceConstraintException)
                errorMessage = ReferenceErrorFormatter(context.Exception as ReferenceConstraintException);
            else if (context.Exception is FluentValidation.ValidationException)
            {
                var validationError = context.Exception as FluentValidation.ValidationException;
                foreach (var item in validationError.Errors)
                {
                    actionResponse.AddError(item.ErrorMessage, item.PropertyName, item.AttemptedValue?.ToString());
                }
            }
            else if (context.Exception is InternalException && context.Exception.Data != null && context.Exception.Data.Count > 0)
            {
                foreach (DictionaryEntry item in context.Exception.Data)
                {
                    actionResponse.AddError(item.Key.ToString(), item.Value.ToString());
                }
            }
            else if (context.Exception is BadHttpRequestException)
            {
                actionResponse.AddError("Request too large. " + context.Exception.Message);
            }
            else
            {
                //logLevel = MotoC2BBO.Entities.LogLevel.SystemError;
                errorMessage = context.Exception.Message;
                if (context.Exception.InnerException != null)
                    errorMessage += '\n' + context.Exception.InnerException.Message;
            }

            if (errorMessage != "")
                actionResponse.AddError(errorMessage);


            //AppLogger logger = new AppLogger(_eventLogDBS);
            //logger.Log(errorMessage, context.Exception.StackTrace, context.HttpContext, logLevel);

            context.Result = new JsonResult(actionResponse);

        }

        public DictionaryEntry UniqueErrorFormatter(UniqueConstraintException ex)
        {
            var message = ex.InnerException.Message;
            var firstQuote = message.IndexOf('\'') + 1;
            var secoundQuote = message.Remove(0, firstQuote + 1).IndexOf('\'') + 1;
            var tableName = message.Substring(firstQuote, secoundQuote).Replace("dbo.", "");

            message = message.Remove(0, firstQuote + 1 + secoundQuote);

            firstQuote = message.IndexOf('\'') + 1;
            secoundQuote = message.Remove(0, firstQuote + 1).IndexOf('\'') + 1;
            var columnName = message.Substring(firstQuote, secoundQuote).Replace("IX_" + tableName + "_", "");

            message = message.Remove(0, firstQuote + 1 + secoundQuote);

            firstQuote = message.IndexOf('(') + 1;
            secoundQuote = message.Remove(0, firstQuote + 1).IndexOf(')') + 1;
            var duplicateValue = message.Substring(firstQuote, secoundQuote);
            var dicEntry = new DictionaryEntry(columnName, duplicateValue + " already exist.");
            return dicEntry;
            //var errorMessage = "The " + columnName + ": " + duplicateValue + " already exist, Cannot insert duplicate";
            //return errorMessage;
        }

        public string ReferenceErrorFormatter(ReferenceConstraintException ex)
        {
            var message = ex.InnerException.Message;
            var firstQuote = message.IndexOf('\"') + 1;
            var secoundQuote = message.Remove(0, firstQuote + 1).IndexOf('\"') + 1;
            firstQuote = message.Substring(0, firstQuote + secoundQuote).LastIndexOf("_") + 1;
            secoundQuote = message.Remove(0, firstQuote + 1).IndexOf('\"') + 1;
            var childColumnName = message.Substring(firstQuote, secoundQuote);

            message = message.Remove(0, firstQuote + 1 + secoundQuote);

            secoundQuote = message.LastIndexOf('\"');
            firstQuote = message.Remove(secoundQuote).LastIndexOf('\"') + 1;
            var parentTableName = message.Substring(firstQuote, secoundQuote - firstQuote).Replace("dbo.", "");


            secoundQuote = message.LastIndexOf('\'');
            firstQuote = message.Remove(secoundQuote).LastIndexOf('\'') + 1;
            var parentColumnName = message.Substring(firstQuote, secoundQuote - firstQuote);


            var errorMessage = "Invalid " + childColumnName + ", Given " + childColumnName + " not available in " + parentTableName + "'s " + parentColumnName + ".";
            return errorMessage;
        }
    }

    public class InternalException : Exception
    {
        public InternalException(string message, string errorfeild)
        {
            Data.Add(errorfeild, message);
        }
    }    
}
