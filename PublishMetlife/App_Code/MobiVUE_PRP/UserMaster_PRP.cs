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
    /// Description : UserMaster_PRP
    /// </summary>
    public class UserMaster_PRP
    {
        #region USER MASTER PROPERTIES
        public string UserName
        { get; set; }
        public string UserID
        { get; set; }
        public string UserPswd
        { get; set; }
        public string LocationCode
        { get; set; }
        public string CompCode
        { get; set; }
        public string UserEmail
        { get; set; }
        public string TechOpsEmail
        { get; set; }
        public bool Active
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public string GroupCode
        { get; set; }
        public string NewPswd
        { get; set; }
        public string SessionID
        { get; set; }

        public string EmployeeID { get; set; }

        public string GroupName { get; set; }
        public string GroupRemarks { get; set; }

        #endregion
    }
}