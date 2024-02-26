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

/// <summary>
/// Summary description for VendoeEscalationMatrix_PRP
/// </summary>
public class VendorEscalationMatrix_PRP
{
    #region VENDOR ESCALATION MATRIX PROPERTIES
    public int VEMCode
    { get; set; }
    public string VEMVendorCode
    { get; set; }
    public string VEMPersonName
    { get; set; }
    public string VEMEmail
    { get; set; }
    public string VEMMobile
    { get; set; }
    public string VEMAddress
    { get; set; }
    public string VEMLevel
    { get; set; }
    public string CompCode
    { get; set; }
    public bool VEMActive
    { get; set; }
    public string VEMRemarks
    { get; set; }
    public string VEMSupportType
    { get; set; }
    public string CreatedBy
    { get; set; }
    public string ModifiedBy
    { get; set; }
    #endregion
}