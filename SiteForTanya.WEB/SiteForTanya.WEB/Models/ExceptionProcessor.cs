using System;
using SiteForTanya.Models;

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