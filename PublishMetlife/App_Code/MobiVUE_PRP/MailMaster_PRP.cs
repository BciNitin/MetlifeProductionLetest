using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobiVUE_ATS.PRP
{
    /// <summary>
    /// Mail Configuration Master
    /// </summary>
    public class MailMaster_PRP
    {
        public string TransactionType { get; set; }
        public string ToMailAddress { get; set; }
        public string CCMailAddress { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string CompCode { get; set; }
        public string Remarks { get; set; }
        public string UserID { get; set; }
    }
}