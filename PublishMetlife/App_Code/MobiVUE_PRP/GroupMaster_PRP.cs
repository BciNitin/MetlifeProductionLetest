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
    /// Description : GroupMaster_PRP
    /// </summary>
    public class GroupMaster_PRP
    {
        #region GROUP MASTER PROPERTIES
        public string GroupCode
        { get; set; }
        public string GroupName
        { get; set; }
        public string BtnStatus { get; set; }
        public string isExistGroupCode
        { get; set; }
        public string isExistGroupName
        { get; set; }
        public string UserName
        { get; set; }
        public string UserID
        { get; set; }
        public string LocationCode
        { get; set; }
        public string UserEmail
        { get; set; }
        public string EmployeeID
        { get; set; }
        public string Remarks
        { get; set; }
        public string GroupRemarks
        { get; set; }
        public bool Active
        { get; set; }
        public string CreatedBy
        { get; set; }        
        public string ModifiedBy
        { get; set; }
        public string CompCode
        { get; set; }
        #endregion
    }
}