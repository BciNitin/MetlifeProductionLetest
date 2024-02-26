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
    /// Summary description for Category Master Properties
    /// </summary>
    public class CategoryMaster_PRP
    {
        #region CATEGORY MASTER PROPERTIES
        public string CategoryInitials
        { get; set; }
        public string CategoryCode
        { get; set; }
        public string CategoryName
        { get; set; }
        public string CategoryType
        { get; set; }
        public string ParentCategory
        { get; set; }
        public string AssetType
        { get; set; }
        public string CompCode
        { get; set; }
        public int CategoryLevel
        { get; set; }
        public string Remarks
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