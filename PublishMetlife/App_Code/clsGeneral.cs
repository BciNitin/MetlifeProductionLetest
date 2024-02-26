using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Net.Mail;
using System.Net.Sockets;
using context = System.Web.HttpContext;
using System.Collections.Generic;
using System.Text;

public static class clsGeneral
{
    #region STATIC VARIABLES
    public static string gStrAssetType { get; set; }
    public static string OpType = "";
    public static string ErrMsg = "";
    public static string[] _strRights;
    public static string gStrSessionID = "";
    public static bool gStrApproveStatus = false;
    public static string gStrCompType { get; set; }
    public static string GlobalEmpEmailId { get; set; }

    #endregion

    #region PUBLIC STATIC FUNCTIONS
    /// <summary>
    /// Get the set of user rights based on logged in user group.
    /// </summary>
    /// <param name="_PageName"></param>
    /// <param name="dtRights"></param>
    /// <returns></returns>
    public static string GetRights(string _PageName, DataTable dtRights)
    {
        char _RtVw, _RtSave, _RtEdit, _RtDlt, _RtExp;
        var vrCountry = (from country in dtRights.AsEnumerable()
                         where country.Field<string>("PAGE_NAME") == _PageName
                         select country);
        var rows = vrCountry.ToList();
        _RtVw = rows[0][9].ToString() == "True" ? '1' : '0';
        _RtSave = rows[0][10].ToString() == "True" ? '1' : '0';
        _RtEdit = rows[0][11].ToString() == "True" ? '1' : '0';
        _RtDlt = rows[0][12].ToString() == "True" ? '1' : '0';
        _RtExp = rows[0][13].ToString() == "True" ? '1' : '0';
        return (_RtVw + "^" + _RtSave + "^" + _RtEdit + "^" + _RtDlt + "^" + _RtExp);
    }

    /// <summary>
    /// Convert date format from dd/MM/yyyy into MM/dd/yyyy.
    /// </summary>
    /// <param name="_strDate"></param>
    /// <returns></returns>
    public static string GetDateFormatChanged(string _strDate)
    {
        try
        {
            string _DateMMDDYYYY = "";
            string[] _DateParts = _strDate.Split('/');
            _DateMMDDYYYY = _DateParts[1] + "/" + _DateParts[0] + "/" + _DateParts[2];
            return _DateMMDDYYYY;
        }
        catch (Exception ex)
        { throw ex; }
    }

    /// <summary>
    /// Compare two dates in string format.
    /// </summary>
    /// <param name="_Date1"></param>
    /// <param name="_Date2"></param>
    /// <returns></returns>
    public static int CompareDate(string _Date1, string _Date2)
    {
        DateTime date1, date2;
        int i = 0;
        if (_Date2.Trim() != "" && _Date1.Trim() != "")
        {
            date1 = Convert.ToDateTime(_Date1);
            date2 = Convert.ToDateTime(_Date2);
            i = date1.Date.CompareTo(date2.Date);
        }
        return i;
    }

    /// <summary>
    /// Log the error/exception occured and return user frindly error message.
    /// </summary>
    /// <param name="ee">The ee.</param>
    /// <param name="userFriendlyError">The user friendly error.</param>
    /// <returns></returns>
    public static string LogErrorToLogFile(Exception ex, string userFriendlyError)
    {
        try
        {
            string path = context.Current.Server.MapPath("~/ErrorLogging/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + DateTime.Today.ToString("dd-MMM-yyyy") + ".log";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            using (StreamWriter writer = File.AppendText(path))
            {
                string error = "Log written at : " + DateTime.Now.ToString() +
                "\r\nError occured on page : " + context.Current.Request.Url.ToString() +
                "\r\n\r\nHere is the actual error :\n" + ex.ToString();
                writer.WriteLine(error);
                writer.WriteLine("=================================================================================");
                writer.Flush();
                writer.Close();
            }
            return userFriendlyError;
        }
        catch
        { throw; }
    }

    /// <summary>
    /// Log user and location/company along with page name visited.
    /// </summary>
    /// <param name="ee">The ee.</param>
    /// <param name="userFriendlyError">The user friendly error.</param>
    /// <returns></returns>
    public static void LogUserOperationToLogFile(string UserID, string UserLocation, string PageVisited)
    {
        try
        {
            string path = context.Current.Server.MapPath("~/ErrorLogging/");            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + DateTime.Today.ToString("dd-MMM-yyyy") + ".log";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            using (StreamWriter writer = File.AppendText(path))
            {
                string date = "User operation done at : " + DateTime.Now.ToString() +
                              "\r\nUser login id : " + UserID +
                              "\r\nUser logged in location/Company : " + UserLocation +
                              "\r\nUser visited page name : " + PageVisited;
                writer.WriteLine(date);
                writer.WriteLine("=================================================================================");
                writer.Flush();
                writer.Close();
            }
        }
        catch
        { throw; }
    }

    /// <summary>
    /// Send mail message to end user.
    /// </summary>
    /// <param name="toAddress"></param>
    /// <param name="ccAddress"></param>
    /// <param name="bccAddress"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="priority"></param>
    /// <param name="isHtml"></param>
    public static void SendEmail(string toAddress, string ccAddress, string bccAddress, string subject, string body, MailPriority priority, bool isHtml)
    {
        try
        {
            SmtpClient smtpClient = new SmtpClient();
            using (MailMessage message = new MailMessage())
            {
                MailAddress fromAddress = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["SENDER"].ToString(), "Your name");
                // You can specify the host name or ipaddress of your server
                smtpClient.Host = "mail.yourdomain.com"; //you can also specify mail server IP address here
                //Default port will be 25
                smtpClient.Port = Convert.ToInt32( System.Configuration.ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
                smtpClient.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SMTP_HOST"].ToString(), System.Configuration.ConfigurationManager.AppSettings["PASS"].ToString());
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                //From address will be given as a MailAddress Object
                message.From = fromAddress;
                message.Priority = priority;
                // To address collection of MailAddress
                message.To.Add(toAddress);
                message.Subject = subject;
                // CC and BCC optional
                if (ccAddress.Length > 0)
                {
                    message.CC.Add(ccAddress);
                }
                if (bccAddress.Length > 0)
                {
                    message.Bcc.Add(bccAddress);
                }
                //Body can be Html or text format
                //Specify true if it is html message
                message.IsBodyHtml = isHtml;
                // Message body content
                message.Body = body;
                // Send SMTP mail
                smtpClient.Send(message);
            }
        }
        catch (Exception ex)
        {
            LogErrorToLogFile(ex, "Mail sending error");
        }
    }

    public static DateTime? ToDate(string date)
    {
        DateTime? dt = null;
        try
        {
            var _date = date.Split('/');
            dt = new DateTime(Convert.ToInt32(_date[2]), Convert.ToInt32(GetMonth(_date[1])), Convert.ToInt32(_date[0]));
        }
        catch (Exception ex)
        {
            return dt;
        }
        return dt;

    }


    public static int GetMonth(string month)
    {
        Dictionary<string, int> dictmonth = new Dictionary<string, int>();
        dictmonth.Add("JAN", 1);
        dictmonth.Add("FEB", 2);
        dictmonth.Add("MAR", 3);
        dictmonth.Add("APR", 4);
        dictmonth.Add("MAY", 5);
        dictmonth.Add("JUN", 6);
        dictmonth.Add("JUL", 7);
        dictmonth.Add("AUG", 8);
        dictmonth.Add("SEP", 9);
        dictmonth.Add("OCT", 10);
        dictmonth.Add("NOV", 11);
        dictmonth.Add("DEC", 12);

        if (dictmonth.ContainsKey(month.ToUpper()))
        {
            return dictmonth.FirstOrDefault(p => p.Key == month.ToUpper()).Value;
        }
        return 0;

    }

    public static string CapitalizeFirst(this string s)
    {
        bool IsNewSentense = true;
        s = s.ToLower();
        var result = new StringBuilder(s.Length);
        for (int i = 0; i < s.Length; i++)
        {
            if (IsNewSentense && char.IsLetter(s[i]))
            {
                result.Append(char.ToUpper(s[i]));
                IsNewSentense = false;
            }
            else
                result.Append(s[i]);

            if (s[i] == '!' || s[i] == '?' || s[i] == '.' || s[i]==' ')
            {
                IsNewSentense = true;
            }
        }

        return result.ToString();
    }

    #endregion

    /*
    #region OLD CODE
    public static void WriteLog(string Module, string Class, string Function, string Description)
    {
        try
        {
            string d_Date = DateTime.Now.ToString();
            string FileLine = d_Date + "|" + Module + "|" + Class + "|" + Function + "|" + Description;

            if (File.Exists(HttpContext.Current.Server.MapPath("LogFile/test.txt")) != true)
            {
                string FileLine1 = "DATE | MODULE | CLASS| FUNCTION|DESCRIPTION";
                System.IO.StreamWriter StreamWriter1 = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("LogFile/test.txt"));
                StreamWriter1.WriteLine(FileLine1);
                StreamWriter1.Close();
                StreamWriter1 = null;
            }
            System.IO.StreamWriter StreamWriter2 = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("LogFile/test.txt"), true);
            StreamWriter2.WriteLine(FileLine, true);
        }
        catch { }
        {
        }
    }

    public static void CheckSessionTimeout(Page oPage, Type oType, string sPath)
    {
        //time to remind, 30 minuts before session ends
        string str_Script = @"
            var sessionTimeout = " + 30 * 60 * 1000 + ";" +
            "function doRedirect(){ window.location.href='" + sPath + "'; }" + @"
            myTimeOut=setTimeout('doRedirect()', sessionTimeout); ";

        ScriptManager.RegisterClientScriptBlock(oPage, oType, "CheckSessionOut", str_Script, true);

        if (oPage.Session["CURRENTUSER"] == null || Convert.ToString(oPage.Session["CURRENTUSER"]).Trim() == "")
        {
            oPage.Response.Redirect("~/SessionExpired.aspx?id=checksessionnulluser");
        }
    }

    public static string ChangeBinaryToString(string sActive)
    {
        string sReturn = "";
        sReturn = (sActive.Trim() == "1" ? "YES" : "NO");
        return sReturn;
    }

    public static string ChangeStringToBinary(string sActive)
    {
        string sReturn = "";
        sReturn = (sActive.Trim().ToUpper() == "YES" ? "1" : "0");
        return sReturn;
    }

    public static DataTable ChangeBinaryToString(DataTable dt)
    {    
        int iCol = 0;
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            if (dt.Columns[i].ToString().ToUpper().Contains("ACTIVE"))
            {
                iCol = i;
                break;
            }
        }
        for (int j = 0; j < dt.Rows.Count; j++)
        {
            if (Convert.ToString(dt.Rows[j][iCol]) == "1")
            {
                dt.Rows[j][iCol] = "YES";
            }
            else if (Convert.ToString(dt.Rows[j][iCol]) == "0")
            {
                dt.Rows[j][iCol] = "NO";
            }
        }
        return dt;
    }

    public static string ChangeDateFormat(string strDate, string Format)
    {
        try
        {
            string strReturnDate = "";

            if (strDate.Trim() == "")
                return strReturnDate;


            string[] sDate = strDate.Split('/');
            if (Format.Trim().ToUpper() == "DDMMYYYY")
            {
                if (sDate[1].Trim().Length == 1)
                {
                    sDate[1] = "0" + sDate[1].Trim();
                }
                if (sDate[0].Trim().Length == 1)
                {
                    sDate[0] = "0" + sDate[0].Trim();
                }
                strReturnDate = sDate[1] + "/" + sDate[0] + "/" + sDate[2];
            }
            if (Format.Trim().ToUpper() == "MMDDYYYY")
            {
                if (sDate[1].Trim().Length == 1)
                {
                    sDate[1] = "0" + sDate[1].Trim();
                }
                if (sDate[0].Trim().Length == 1)
                {
                    sDate[0] = "0" + sDate[0].Trim();
                }
                strReturnDate = sDate[0] + "/" + sDate[1] + "/" + sDate[2];
            }
            return strReturnDate;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DataTable ChangeDateDataTable(DataTable dt, string Format, int []arrColNo)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < arrColNo.Length - 1; j++)
            {            
                int iCol = arrColNo[j];              
                if (Convert.ToString(dt.Rows[i][iCol]).Trim() != "")
                {
                    dt.Rows[i][iCol] = ChangeDateFormat(Convert.ToString(dt.Rows[i][iCol]).Trim().Substring(0,10), "DDMMYYYY");
                }
            }
        }
        return dt;
    }

    public static string GetDate(string str)
    { 
        string strRet="";
        if (str.Trim() != "")
        {
            string[] arr = str.Split(' ');
            if (arr.Length > 0)
            {
                strRet = arr[0].Trim();
            }
        }
        return strRet;
    }

    public static DataTable ChangeValueFromMicroMaster(DataTable dt, int iColNo)
    {
        try
        {
            string MMVALUE = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BL_clsMicroMaster oBL_clsMicroMaster = new BL_clsMicroMaster();
                oBL_clsMicroMaster.MMId = Convert.ToInt32(dt.Rows[i][iColNo]);
                DataTable dtMM = oBL_clsMicroMaster.GetMicroMaster("MMVALUE");
                if (dtMM.Rows.Count > 0)
                {
                    MMVALUE = Convert.ToString(dtMM.Rows[0][2]).Trim();
                    dt.Rows[i][iColNo] = MMVALUE.Trim();
                }
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DataTable  ChangeDataTable(DataTable dt , int []arrCol)
    {
        DataTable dtReturn = dt;
        ArrayList arr = new ArrayList();
        for(int i = 0; i< arrCol.Length ;i++)
        {
            string sCol =  dt.Columns[arrCol[i]].ToString();
            arr.Add(sCol);
       }
        foreach (string str in arr)
        {
            dtReturn.Columns.Remove(str);
        }
        return dtReturn;
    }


    public static DataTable ChangeDataTable(DataTable dt, int[] arrCol)
    {
        DataTable dtReturn = new DataTable () ;
        ArrayList arr = new ArrayList();
        for (int i = 0; i < arrCol.Length; i++)
        {
            string sCol = dt.Columns[arrCol[i]].ToString();
            arr.Add(sCol);
        }
        foreach (string str in arr)
        {
            dtReturn.Columns.Add(str);
        }
        for (int j = 0; j < dt.Rows.Count; j++)
        { 
            ArrayList arrData = new ArrayList ();
            for(int k = 0 ; k <dt.Columns.Count ;k++)
            {
               foreach (string str in arr)
               {
                   arrData.Add(Convert.ToString(dt.Rows[j][str]));   
               }
               dtReturn.Rows.Add(arrData.ToArray());
               break;
            }        
        }
        return dtReturn;
    }
        
    public static void DeleteFiles(string DirectoryPath, string InitailFileName)
    {        
        string[] strFiles = Directory.GetFiles(DirectoryPath);
        for (int i = 0; i < strFiles.Length; i++)
        {
            if (strFiles[i].ToString().ToUpper().Trim().Contains(InitailFileName))
            {
                string strFilePath = strFiles[i].ToString();
                File.Delete(strFilePath);
            }
        }
    }

    /// <summary>
    /// To Print Message
    /// </summary>
    /// <param name="lbl"></param>
    /// <param name="Message"></param>
    /// <param name="iType"> 0 = Error , 1 = Sucess</param>
    public static void Message(ref Label lbl, string Message, int iType)
    {
        lbl.Text = "";
        lbl.Text = Message.Trim();
        if (iType == 0)
        {
            lbl.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            lbl.ForeColor = System.Drawing.Color.Green;
        }
    }

    public static DataTable AddRowNumber(DataTable dt)
    {
        try
        {
            dt.Columns.Add("ROWNO");
            dt.AcceptChanges();
            int colNo = dt.Columns.Count;
            colNo = colNo - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][colNo] = i + 1;
                dt.AcceptChanges();
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    */
}