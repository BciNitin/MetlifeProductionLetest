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
    /// Summary description for AssetTransfer_PRP
    /// </summary>
    public class AssetTransfer_PRP
    {
        #region ASSET TANSFER PROPERTIES
        public string AssetCode
        { get; set; }
        public string AssetSerialCode
        { get; set; }
        public string AssetCategoryCode
        { get; set; }
        public string AssetMakeName
        { get; set; }
        public string AssetModelName
        { get; set; }
        public string AssetType
        { get; set; }
        public string SerialNo
        { get; set; }
        public string AssetModel
        { get; set; }
        public string IUTStatus
        { get; set; }
        public string AssetProcess
        { get; set; }
        public string PortNo
        { get; set; }
        public string FromLocationCode
        { get; set; }
        public string ToLocationCode
        { get; set; }
        public string FromInterLocationCode
        { get; set; }
        public string ToInterLocationCode
        { get; set; }
        public string ToInterProcessCode
        { get; set; }
        public string FromWorkStation
        { get; set; }
        public string ToWorkStation
        { get; set; }
        public string FromPort
        { get; set; }
        public string ToPort
        { get; set; }
        public string TransferRemarks
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string TransferType
        { get; set; }
        public string CompCode
        { get; set; }
        public string FromDate
        { get; set; }
        public string ToDate
        { get; set; }
        public string TransferDate
        { get; set; }
        public string TransferLocation
        { get; set; }
        #endregion
    }
}