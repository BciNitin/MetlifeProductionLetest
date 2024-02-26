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
    /// Summary description for DeptMaster_PRP
    /// </summary>
    public class DeptMaster_PRP
    {
        #region DEPARTMENT MASTER PROPERTIES
        public string DeptCode
        { get; set; }
        public string DeptName
        { get; set; }
        public string Remarks
        { get; set; }
        public string CompCode
        { get; set; }
        public bool Active
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        #endregion
    }
}