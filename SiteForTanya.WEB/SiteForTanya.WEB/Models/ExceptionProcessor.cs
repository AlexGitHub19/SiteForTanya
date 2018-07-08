using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models
{
    public class ExceptionProcessor
    {
        public void process(Exception exception)
        {
            Repository<ExceptionEntity> exceptionRepository = new Repository<ExceptionEntity>();
            exceptionRepository.Create(new ExceptionEntity {Mesage = exception.Message, StackTrace = exception.StackTrace, Date = DateTime.Now });
        }
    }
}