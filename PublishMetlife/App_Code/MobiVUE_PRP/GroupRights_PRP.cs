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
    /// Summary description for Page Master/Group Rights Properties
    /// </summary>
    public class GroupRights_PRP
    {
        #region PAGE GROUP RIGHTS PROPERTIES
        public int PageCode
        { get; set; }
        public string PageName
        { get; set; }
        public string PageDesc
        { get; set; }
        public string GroupCode
        { get; set; }
        public string CompCode
        { get; set; }
        public bool ViewRight
        { get; set; }
        public bool SaveRight
        { get; set; }
        public bool EditRight
        { get; set; }
        public bool DeleteRight
        { get; set; }
        public bool ExportRight
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public string AssetType
        { get; set; }
        #endregion
    }
}