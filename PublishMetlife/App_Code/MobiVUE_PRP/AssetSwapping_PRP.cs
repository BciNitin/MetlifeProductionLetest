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
    /// Summary description for Asset Swapping Properties
    /// </summary>
    public class AssetSwapping_PRP
    {
        #region ASSET SWAPPING PROPERTIES
        public string AssetCode
        { get; set; }
        public string SerialCode
        { get; set; }
        public string AssetID
        { get; set; }
        public string LocationCode
        { get; set; }
        public bool AllocatedAssets
        { get; set; }
        public string SwappingRemarks
        { get; set; }
        public string PortNo
        { get; set; }
        public string CompCode
        { get; set; }
        public string SwapDate
        { get; set; }

        public string AssetCode1
        { get; set; }
        public string AssetID1
        { get; set; }
        public string SerialCode1
        { get; set; }
        public string LocationCode1
        { get; set; }
        public string EmpName1
        { get; set; }
        public string EmpCode1
        { get; set; }
        public string ProcessCode1
        { get; set; }
        public string AllocatedLocCode1
        { get; set; }
        public string WorkStation1
        { get; set; }
        public string PortNo1
        { get; set; }

        public string AssetCode2
        { get; set; }
        public string AssetID2
        { get; set; }
        public string SerialCode2
        { get; set; }
        public string LocationCode2
        { get; set; }
        public string EmpName2
        { get; set; }
        public string EmpCode2
        { get; set; }
        public string ProcessCode2
        { get; set; }
        public string AllocatedLocCode2
        { get; set; }
        public string WorkStation2
        { get; set; }
        public string PortNo2
        { get; set; }
        #endregion
    }
}