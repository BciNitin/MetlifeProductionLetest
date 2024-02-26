using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Data;
using MobiVUE_ATS.DAL;
using MobiVUE_ATS.PRP;
using System.Configuration;
/// <summary>
/// Summary description for ADSearch
/// </summary>
public class ADSearch
{

    #region constants
    public const string SamAccountNameProperty = "sAMAccountName";
    public const string userPrincipalNameProperty = "userPrincipalName";
    public const string DisplayNameproperty = "displayname";
    public const string EmployeeIdProperty = "description";
    public const string CompanyCodeproperty = "extensionAttribute10";
    public const string SitecodeProperty = "extensionAttribute12";
    public const string UerEmailProperty = "mail";
    public const string UserPasswordProperty = "userPassword";
    public const string Departmentproperty = "department";
    public const string Designationproperty = "title";
    public const string DepartmentHeadNameproperty = "manager";
    #endregion

    #region Properties


    //public string UserType { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }

    public string EmployeeId { get; set; }
    public string EmployeeId1 { get; set; }
    public string EmployeeId2 { get; set; }
    public string EmployeeId3 { get; set; }

    public string EmployeeId4 { get; set; }
    public string EmployeeId5 { get; set; }
    //public string EmployeeId5 { get; set; }

    public string Designation1 { get; set; }
    public string SeatNo { get; set; }
    public string Department { get; set; }
    public string ProcessName { get; set; }
    public string LOB { get; set; }
    public string SubLOB { get; set; }

    public string ABC = "";
    #endregion
    public ADSearch()
    {

    }
    public class Users
    {
        public string EmpCode { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Designation { get; set; }
        public bool isMapped { get; set; }

    }



    public static List<Users> GetADUsers(string UserLogin, string pass, string userName)
    {
        List<Users> lstADUsers = new List<Users>();
        try
        {
            string DomainPath = ConfigurationManager.AppSettings["LDAPAdd"].ToString();
            DirectoryEntry searchRoot = new DirectoryEntry(DomainPath, UserLogin, pass);
            DirectorySearcher search = new DirectorySearcher(searchRoot);
            //search.Filter = "(&(objectClass=user)(objectCategory=person))";
            search.Filter = "(EMPLOYEEID=" + userName + ")";
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("usergroup");
            search.PropertiesToLoad.Add("employeeid");
            search.PropertiesToLoad.Add("displayname");//first name
            search.PropertiesToLoad.Add("description");
            SearchResult result;
            SearchResultCollection resultCol = search.FindAll();
            if (resultCol != null)
            {
                for (int counter = 0; counter < resultCol.Count; counter++)
                {
                    string UserNameEmailString = string.Empty;
                    result = resultCol[counter];
                    if (result.Properties.Contains("samaccountname") &&
                             result.Properties.Contains("mail") &&
                        result.Properties.Contains("displayname") &&
                        result.Properties.Contains("description")  ) 
                    {
                        string desc = (String)result.Properties["description"][0];
                        if (!desc.StartsWith("M") &&  !desc.StartsWith("S")) {
                            Users objSurveyUsers = new Users();
                            objSurveyUsers.Email = (String)result.Properties["mail"][0];
                            objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
                            objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
                            objSurveyUsers.EmpCode = (String)result.Properties["employeeid"][0];
                            //objSurveyUsers.Designation = (String)result.Properties["title"][0];
                            lstADUsers.Add(objSurveyUsers);
                        }
                    }
                }
            }
            return lstADUsers;
        }
        catch (Exception ex)
        {
            return lstADUsers;
        }
    }

    public static DataTable GetEmpDetailsTemp1(string UsrLogin, string UsrPass, string userName, string Company)
    {
        GroupMaster_DAL oDALj = new GroupMaster_DAL(HttpContext.Current.Session["DATABASE"].ToString());
        DataTable dtEmp = oDALj.GetEmpDetails(userName);
        try
        {
            List<Users> lstADUsers = GetADUsers(UsrLogin, UsrPass, userName);
            EmployeeMaster_PRP oPRP = new EmployeeMaster_PRP();
            oPRP.Active = true;
            oPRP.Lob = "";
            oPRP.SubLob = "";
            oPRP.CompCode = Company;
            int i = 0;
            foreach (var item in lstADUsers)
            {
                i++;
                oPRP.Designation = item.Designation;
                oPRP.EmpEmail = item.Email;
                oPRP.EmpName = item.DisplayName;
                oPRP.EmpCode = item.EmpCode;
                if (i == 1)
                {
                    break;
                }
            }
            EmployeeMaster_DAL obj = new EmployeeMaster_DAL(HttpContext.Current.Session["DATABASE"].ToString());
            obj.SaveUpdateEmployee("SAVE", oPRP);
            DataTable dt = oDALj.GetEmpDetails(userName);
            return dt;
        }        
        catch (Exception ex)
        {
            return dtEmp;
        }
    }
    public static DataTable GetEmpDetails(string UsrLogin, string UsrPass, string userName, string Company)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("EMP_TAG", typeof(string));
        dt.Columns.Add("EMPLOYEE_CODE", typeof(string));
        dt.Columns.Add("EMPLOYEE_NAME", typeof(string));
        dt.Columns.Add("EMP_EMAIL", typeof(string));
        dt.Columns.Add("Designation", typeof(string));
        dt.Columns.Add("ProcessName", typeof(string));
        dt.Columns.Add("Lob", typeof(string));
        dt.Columns.Add("SubLOB", typeof(string));
        DataRow dr = dt.NewRow();
        dr[0] = "";
        try
        {
            List<Users> lstADUsers = GetADUsers(UsrLogin, UsrPass, userName);
            EmployeeMaster_PRP oPRP = new EmployeeMaster_PRP();
            oPRP.Active = true;
            oPRP.Lob = "";
            oPRP.SubLob = "";
            oPRP.Process = "";
            oPRP.seatno = "";
            oPRP.CompCode = Company;
            oPRP.CreatedBy = UsrLogin;
            oPRP.ModifiedBy = UsrLogin;
            int i = 0;
            foreach (var item in lstADUsers)
            {
                i++;
                oPRP.Designation = item.Designation == null ? "" : item.Designation;
                dr[4] = item.Designation;
                oPRP.EmpEmail = item.Email;
                dr[3] = item.Email;
                oPRP.EmpName = item.DisplayName;
                dr[2] = item.DisplayName;
                oPRP.EmpCode = item.EmpCode;
                dr[1] = item.EmpCode;
                dr[6] = "";
                dr[7] = "";
                EmployeeMaster_DAL obj = new EmployeeMaster_DAL(HttpContext.Current.Session["DATABASE"].ToString());
                obj.SaveUpdateEmployee("SAVE", oPRP);                
                if (i == 1)
                {
                    break;
                }
            }
            return dt;
        }
        catch (Exception ex)
        {
            return dt;
        }


    }
    public static DataTable GetEmpDetailsTemp(string UsrLogin, string UsrPass, string userName, string Company)
    {
        GroupMaster_DAL oDALj = new GroupMaster_DAL(HttpContext.Current.Session["DATABASE"].ToString());
        DataTable dtEmp = oDALj.GetEmpDetails(userName);
        DirectoryEntry m_obDirEntry;
        try
        {
            string strLDAP = ConfigurationManager.AppSettings["LDAPAdd"].ToString();
            m_obDirEntry = new DirectoryEntry(strLDAP, UsrLogin, UsrPass);
            DirectorySearcher srch = new DirectorySearcher(m_obDirEntry);
            srch.Filter = "(EMPLOYEEID=" + userName + ")";
            SearchResultCollection results;
            results = srch.FindAll();
            ResultPropertyCollection propColl = null;           

            DataTable dt = new DataTable();

            dt.Columns.Add("EMP_TAG", typeof(string));
            dt.Columns.Add("EMPLOYEE_CODE", typeof(string));
            dt.Columns.Add("EMPLOYEE_NAME", typeof(string));
            dt.Columns.Add("EMP_EMAIL", typeof(string));
            dt.Columns.Add("Designation", typeof(string));
            dt.Columns.Add("ProcessName", typeof(string));
            dt.Columns.Add("Lob", typeof(string));
            dt.Columns.Add("SubLOB", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "";

            EmployeeMaster_PRP oPRP = new EmployeeMaster_PRP();
            oPRP.Active = true;
            oPRP.Lob = "";
            oPRP.SubLob = "";
            oPRP.CompCode = Company;

            foreach (SearchResult result in results)
            {
                propColl = result.Properties;
            }
            if (propColl != null)
            {
                foreach (string strKey in propColl.PropertyNames)
                {
                    foreach (object obProp in propColl[strKey])
                    {
                        if (strKey.ToUpper() == "DISPLAYNAME")
                        {
                            dr[2] = obProp.ToString();
                            oPRP.EmpName = obProp.ToString();
                        }
                        if (strKey.ToUpper() == "MAIL")
                        {
                            dr[3] = obProp.ToString();
                            oPRP.EmpEmail = obProp.ToString();
                        }
                        if (strKey.ToUpper() == "EMPLOYEEID")
                        {
                            dr[1] = obProp.ToString();
                            oPRP.EmpCode = obProp.ToString();
                        }
                        if (strKey.ToUpper() == "DEPARTMENT")
                        {
                            dr[5] = obProp.ToString();
                            oPRP.Process = obProp.ToString();
                        }
                        if (strKey.ToUpper() == "TITLE")
                        {
                            dr[4] = obProp.ToString();
                            oPRP.Designation = obProp.ToString();
                        }
                    }
                }
                dr[6] = "";
                dr[7] = "";
                EmployeeMaster_DAL obj = new EmployeeMaster_DAL(HttpContext.Current.Session["DATABASE"].ToString());
                obj.SaveUpdateEmployee("SAVE", oPRP);
                return dt;
            }
            else
            {
                DataTable dp = new DataTable();
                return dtEmp;
            }
        }
        catch (System.DirectoryServices.DirectoryServicesCOMException Ex)
        {
            DataTable dp = new DataTable();
            return dtEmp;
        }

        catch (Exception ex)
        {
            DataTable dp = new DataTable();
            return dtEmp;
        }


    }
    public static DataTable GetEmp(string username)
    {
        DataTable dt = new DataTable();
        List<clsLDAP> users = new List<clsLDAP>();
        //DirectoryEntry searchRoot = new DirectoryEntry("LDAP://RootDSE");
        //var defaultNamingContext = searchRoot.Properties["defaultNamingContext"][0].ToString();
        string strLDAP = ConfigurationManager.AppSettings["LDAPAdd"].ToString();
        using (DirectorySearcher directorySearcher = new DirectorySearcher(strLDAP))
        {

            directorySearcher.Filter = "objectCategory=person";
            directorySearcher.Filter = "(description=" + username + ")";
            directorySearcher.PropertiesToLoad.Add("cn");
            directorySearcher.PropertiesToLoad.Add(DisplayNameproperty);
            directorySearcher.PropertiesToLoad.Add(SamAccountNameProperty);
            directorySearcher.PropertiesToLoad.Add(userPrincipalNameProperty);
            directorySearcher.PropertiesToLoad.Add(EmployeeIdProperty);
            directorySearcher.PropertiesToLoad.Add(CompanyCodeproperty);
            directorySearcher.PropertiesToLoad.Add(SitecodeProperty);
            directorySearcher.PropertiesToLoad.Add(UerEmailProperty);
            directorySearcher.PropertiesToLoad.Add(UserPasswordProperty);
            directorySearcher.PropertiesToLoad.Add(DepartmentHeadNameproperty);
            directorySearcher.PropertiesToLoad.Add(Departmentproperty);
            directorySearcher.PropertiesToLoad.Add(Designationproperty);
            SearchResult result = directorySearcher.FindOne();

            if (result.Properties.Count > 0)
            {

                dt.Columns.Add("EMP_TAG", typeof(string));
                dt.Columns.Add("EMPLOYEE_CODE", typeof(string));
                dt.Columns.Add("EMPLOYEE_NAME", typeof(string));
                dt.Columns.Add("EMP_EMAIL", typeof(string));
                dt.Columns.Add("Designation", typeof(string));
                dt.Columns.Add("ProcessName", typeof(string));
                dt.Columns.Add("Lob", typeof(string));
                dt.Columns.Add("SubLOB", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = "";
                dr[1] = result.Properties[EmployeeIdProperty][0].ToString();
                dr[2] = result.Properties[DisplayNameproperty][0].ToString();
                dr[3] = result.Properties[userPrincipalNameProperty][0].ToString();
                dr[4] = result.Properties[Designationproperty][0].ToString();
                dr[5] = result.Properties[Departmentproperty][0].ToString();
                dr[6] = "";
                dr[7] = "";
                EmployeeMaster_PRP oPRP = new EmployeeMaster_PRP();
                oPRP.Active = true;
                oPRP.EmpCode = result.Properties[EmployeeIdProperty][0].ToString();
                oPRP.EmpName = result.Properties[DisplayNameproperty][0].ToString();
                oPRP.EmpEmail = result.Properties[userPrincipalNameProperty][0].ToString();
                oPRP.Process = result.Properties[Departmentproperty][0].ToString();
                oPRP.Designation = result.Properties[Designationproperty][0].ToString();
                oPRP.Lob = "";
                oPRP.SubLob = "";
                EmployeeMaster_DAL obj = new EmployeeMaster_DAL(HttpContext.Current.Session["DATABASE"].ToString());
                obj.SaveUpdateEmployee("SAVE", oPRP);

            }
        }

        return dt;

    }

}

public class clsLDAP
{

    #region constants
    public const string SamAccountNameProperty = "sAMAccountName";
    public const string userPrincipalNameProperty = "userPrincipalName";
    public const string DisplayNameproperty = "displayname";
    public const string EmployeeIdProperty = "description";
    public const string CompanyCodeproperty = "extensionAttribute10";
    public const string SitecodeProperty = "extensionAttribute12";
    public const string UerEmailProperty = "mail";
    public const string UserPasswordProperty = "userPassword";
    public const string Departmentproperty = "department";
    public const string Designationproperty = "title";
    public const string DepartmentHeadNameproperty = "manager";
    #endregion


    #region Properties


    //public string UserType { get; set; }
    public string UserName { get; set; }

    public string EmployeeId { get; set; }
    public string EmployeeId1 { get; set; }
    public string EmployeeId2 { get; set; }
    public string EmployeeId3 { get; set; }

    public string EmployeeId4 { get; set; }
    public string EmployeeId5 { get; set; }
    //public string EmployeeId5 { get; set; }

    public string Designation { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public string SeatNo { get; set; }
    public string ProcessName { get; set; }
    public string LOB { get; set; }
    public string SubLOB { get; set; }
    #endregion



}