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

namespace MobiVUE_ATS.PRP
{
    /// <summary>
    /// Summary description for AddViewFiles_PRP
    /// </summary>
    public class DocumentMgmt_PRP
    {
        #region DOCUMENT MANAGEMENT PROPERTIES
        public string Description
        { get; set; }
        public string Category
        { get; set; }
        public string Remarks
        { get; set; }
        public string AttachFileName
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string CompCode
        { get; set; }
        public string CompanyName
        { get; set; }
        public int SerialNo
        { get; set; }
        #endregion
    }
}