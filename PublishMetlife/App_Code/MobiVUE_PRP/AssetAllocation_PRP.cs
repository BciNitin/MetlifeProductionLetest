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
    /// Summary description for AssetAllocation_PRP
    /// </summary>
    public class AssetAllocation_PRP
    {
        #region ASSET ALLOCATION PROPERTIES
        public string MeetingRoom
        { get; set; }

        public string CompCode
        { get; set; }
        public string AssetCode
        { get; set; }
        public string Site
        { get; set; }
        public string SerialCode
        { get; set; }
        public string AssetAllocationTo
        { get; set; }
        public string SubAssetAllocationTo
        { get; set; }
        public string AllocatedTo
        { get; set; }
        public string AllocatedToId
        { get; set; }
        public string RequestedBy
        { get; set; }
        public string RequestedById
        { get; set; }
        public string ApprovedBy
        { get; set; }
        public string ApprovedById
        { get; set; }
        public string AssetLocation
        { get; set; }
        public string Asset_FAR_TAG
        { get; set; }
        public string AssetMake
        { get; set; }
        public string RFIDTag
        { get; set; }
        public string ModelName
        { get; set; }
        public string WorkStationNo
        { get; set; }
        public string AllocationDate
        { get; set; }
        public string ExpReturnDate
        { get; set; }
        public string ActualReturnDate
        { get; set; }
        public bool AssetAllocated
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
        public string PortNo
        { get; set; }
        public string Vlan
        { get; set; }
        public string TicketNo
        { get; set; }
        public string GatePassNo
        { get; set; }
        public string AllocationRemarks
        { get; set; }
        public string FromDeptCode
        { get; set; }
        public string ToDeptCode
        { get; set; }
        public string FromProcessCode
        { get; set; }
        public string ToProcessCode
        { get; set; }
        public string AssetType
        { get; set; }
        public string Store
        { get; set; }
        public string Floor
        { get; set; }
        public string EmailID
        { get; set; }
        public string EmpName
        { get; set; }
        public string IdentifierLocation
        { get; set; }
        public string Process
        { get; set; }
        public string SeatNo
        { get; set; }
        public string LOB
        { get; set; }
        public string EmpTag
        { get; set; }
        public string AssetTag
        { get; set; }
        public string SubLOB
        { get; set; }
        public string AllocationType
        { get; set; }
        public string Designation
        { get; set; }
        public string CategoryCode
        { get; set; }
        public string EmpCode
        { get; set; }

        public string Status
        { get; set; }

        public string SubStatus
        { get; set; }

        public string DeallocationRemarks
        { get; set; }
        public string AllocationId
        { get; set; }

        public string NoofDueDate
        { get; set; }

        public string AssetTagID
        { get; set; }


        public string EmpTagID
        { get; set; }


        public string EmpFloor
        { get; set; }

        public string HostName { get; set; }



        #endregion
    }
}