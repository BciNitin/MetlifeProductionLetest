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
    /// Summary description for VendorMaster_PRP
    /// </summary>
    public class VendorMaster_PRP
    {
        #region VENDOR MASTER PROPERTIES
        public string VendorCode
        { get; set; }
        public string VendorName
        { get; set; }
        public string VendorAddress
        { get; set; }
        public string VendorCountry
        { get; set; }
        public string VendorSate
        { get; set; }
        public string VendorCity
        { get; set; }
        public string VendorPIN
        { get; set; }
        public string VendorContPerson
        { get; set; }
        public string VendorPhone
        { get; set; }
        public string VendorEmail
        { get; set; }
        public bool Active
        { get; set; }
        public string Remarks
        { get; set; }
        public string CompCode
        { get; set; }

        public string WorkCatagory { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        #endregion
    }
}