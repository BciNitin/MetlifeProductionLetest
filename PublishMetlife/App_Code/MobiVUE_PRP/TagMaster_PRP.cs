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
/// Summary description for TagMaster_PRP
/// </summary>
    public class TagMaster_PRP
    {
        public bool Active
        { get; set; }
        public string SerialNo
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }

        public string AssetCode
        { get; set; }

        public string TagSerialNo
        { get; set; }
        public string HostName
        { get; set; }
        public string AssetTag
        { get; set; }

        public string CompCode
        { get; set; }

        public string EmpCode
        { get; set; }

        public string EmailID
        { get; set; }

        public string Designation
        { get; set; }

        public string SeatNo
        { get; set; }

        public string ProcessName
        { get; set; }

        public string LOB
        { get; set; }

        public string SubLOB
        { get; set; }

        public string EmpName
        { get; set; }
    }
}