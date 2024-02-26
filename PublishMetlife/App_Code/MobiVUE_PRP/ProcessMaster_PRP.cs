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
    /// Summary description for Process Master Properties
    /// </summary>
    public class ProcessMaster_PRP
    {
        #region PROCESS MASTER PROPERTIES
        public string DeptCode
        { get; set; }
        public string ProcessCode
        { get; set; }
        public string ProcessName
        { get; set; }
        public string ProcessOwner
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string CompCode
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public bool Active
        { get; set; }
        #endregion
    }
}