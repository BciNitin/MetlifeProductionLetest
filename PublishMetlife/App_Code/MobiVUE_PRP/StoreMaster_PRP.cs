using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobiVUE_ATS.PRP
{

    public class StoreMaster_PRP
    {
        public string StoreCode
        { get; set; }
        public string StoreName
        { get; set; }

        public string SiteCode
        { get; set; }
        public string CompCode
        { get; set; }
        public string SiteName
        { get; set; }
        public string Floor
        { get; set; }
        public string Remarks
        { get; set; }
        public bool Active
        { get; set; }
        public string CreatedBy
        { get; set; }
        public string ModifiedBy
        { get; set; }
    }
}
