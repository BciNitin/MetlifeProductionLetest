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
    /// Property Class for Gate Pass Generation
    /// </summary>
    public class StoreMovementInOut_PRP
    {
        #region StoreMovementInOut_PRP PROPERTIES
        public string TransferItem
        { get; set; }

        public string DocumentNo
        { get; set; }
        public string ExpReturnDate
        { get; set; }
        public string AssetCode
        { get; set; }
        public string SerialCode
        { get; set; }

        public string ASSET_FAR_TAG
        { get; set; }

        public string AssetTag
        { get; set; }

        public string EMPTag
        { get; set; }
        public string GatePassCode
        { get; set; }
        public string GatePassNo
        { get; set; }
        public string CompCode
        { get; set; }
        public string Floor
        { get; set; }
        public string Store
        { get; set; }
        public string Site
        { get; set; }
        public string GatePassDate
        { get; set; }
        public string GatePassType
        { get; set; }
        public string VendorCode
        { get; set; }
        public string EmpCode
        { get; set; }
        public string CustCode
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string DestinationLocation
        { get; set; }
        public string AssetMake
        { get; set; }
        public string ModelName
        { get; set; }
        public string AssetType
        { get; set; }
        public string CategoryCode
        { get; set; }
        public string BearerName
        { get; set; }
        public string CarrierName
        { get; set; }        
        public string Remarks
        { get; set; }
        public bool Active
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public bool Approve_GatePass
        { get; set; }
        public int Running_Serial_No
        { get; set; }
        #endregion
    }
}