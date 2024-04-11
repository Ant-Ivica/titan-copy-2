using FA.LVIS.Tower.Common;
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    public class LVISCustom : Exception
    {
        string message;
        public LVISCustom(string CMessage) : base()
        {

            message = RemoveCRLFCharacters(CMessage);
        } //constructor

        public override string Message
        {
            get
            {
                return message;
            }
        }

        public override string StackTrace
        {
            get { return "Message instead stacktrace"; }
        }

        public static string RemoveCRLFCharacters(string inputString)
        {
            var outputString = string.Empty;
            if (!string.IsNullOrEmpty(inputString))
            {
                inputString = CleanString(inputString);
                outputString = HttpUtility.UrlDecode(inputString)
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("%0d", string.Empty)
                    .Replace("%0D", string.Empty)
                    .Replace("%0a", string.Empty)
                    .Replace("%0A", string.Empty)
                    .Replace("%5Cr", string.Empty)
                    .Replace("%5Cn", string.Empty)
                    .Replace("%5C%6E", string.Empty)
                    .Replace("%5C%72", string.Empty)
                    .Replace(@"\%6E", string.Empty)
                    .Replace(@"\%72", string.Empty);
            }

            return outputString;
        }

        public static string CleanString(string inputString)
        {
            string ALLOWABLE_CHARS = @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'?,./\s/g]";
            return Regex.Replace(inputString, ALLOWABLE_CHARS, "");
        }
    }

    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        Logger sLogger = new Logger();

        public override void OnException(HttpActionExecutedContext ExecutedContext)
        {
            string exceptionMessage = string.Empty;

            if (ExecutedContext.Exception.StackTrace == "Message instead stacktrace")
            {

                sLogger.Error(ExecutedContext.Exception.Message);
                var response1 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ExecutedContext.Exception.Message),
                    ReasonPhrase = ExecutedContext.Exception.Message

                };
                ExecutedContext.Response = response1;
                return;
            }


            if (ExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = ExecutedContext.Exception.Message;
            }
            else
            {
                exceptionMessage = ExecutedContext.Exception.InnerException.Message;
            }
            sLogger.Error(exceptionMessage);
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content =  new StringContent("An unhandled exception was thrown by service."),
                ReasonPhrase = "Internal Server Error.Please Contact your Administrator." + ExecutedContext.Exception.Source

            };
            ExecutedContext.Response = response;


        }
    }
}