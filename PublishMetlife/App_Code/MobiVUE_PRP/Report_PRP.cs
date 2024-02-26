using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for Report_PRP
/// </summary>
namespace MobiVUE_ATS.PRP
{
public class Report_PRP
{
    

         public string CompCode
        { get; set; }
        public string Type
        { get; set; }
        public string FromDate
        { get; set; }
        public string ToDate
        { get; set; }

        public string TagID
        { get; set; }
        //
        // TODO: Add constructor logic here
        //
    }
}
