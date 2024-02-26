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
/// Summary description for EmployeeMaster_PRP
/// </summary>
public class EmployeeMaster_PRP
{
    #region EMPLOYEE MASTER PROPERTIES
    public string EmpCode
    { get; set; }
    public string EmpName
    { get; set; }
    public string EmpCompCode
    { get; set; }
    public string EmpProcCode
    { get; set; }
    public string Designation
    { get; set; }
    public string EmpEmail
    { get; set; }
    public string Process
    { get; set; }
    public string seatno
    { get; set; }
    public string SubLob
    { get; set; }
    public string CompCode
    { get; set; }
    public bool Active
    { get; set; }
    public string Lob
    { get; set; }
    public string CreatedBy
    { get; set; }
    public string ModifiedBy
    { get; set; }

    public string EmpReprotTo { get; set; }
    public string EmpDOJ { get; set; }
    public string EmpPhone { get; set; }
    public string EmpRemarks { get; set; }
    #endregion
}