using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for SendmailAlert
/// </summary>
public class SendmailAlert
{
    public void SendMailForTransaction(string ToAddress, string CCAddress, string Subject, string Msg)
    {
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        if (!string.IsNullOrEmpty(ToAddress))
        {
            string frommailid = string.Empty;
            string password = string.Empty;
            frommailid = ConfigurationManager.AppSettings["SENDER"];
            password = ConfigurationManager.AppSettings["password"];
            
            string smtpclient = ConfigurationManager.AppSettings["SMTP_HOST"];
            int portno = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
            bool enablessl = Convert.ToBoolean(ConfigurationManager.AppSettings["enablessl"]);

            MailMessage mm = new MailMessage();
            MailAddress from = new MailAddress(frommailid);
            mm.From = from;
            mm.Subject = Subject;
            mm.IsBodyHtml = false;
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendLine(Msg);
            sbMsg.AppendLine("");
            sbMsg.AppendLine("");
            sbMsg.AppendLine("");
            mm.Body = sbMsg.ToString();
            if (ToAddress.Contains(","))
            {
                string[] Tomail = ToAddress.Split(',');
                for (int i = 0; i < Tomail.Length; i++)
                {
                    mm.To.Add(new MailAddress(Tomail[i].ToString()));
                }
            }
            else
            {
                mm.To.Add(new MailAddress(ToAddress));
            }
            if (!string.IsNullOrEmpty(CCAddress))
            { 
                if (CCAddress.Contains(","))
                {
                    string[] ccmail = CCAddress.Split(',');
                    for (int i = 0; i < ccmail.Length; i++)
                    {
                        mm.CC.Add(new MailAddress(ccmail[i].ToString()));
                    }
                }
                else
                {
                    mm.CC.Add(new MailAddress(CCAddress));
                }
            }
            //foreach (DataRow row in dt.Rows)
            //{
            //    mm.To.Add(new MailAddress(row["USER_EMAIL"].ToString()));
            //}
            SmtpClient smtp = new SmtpClient(smtpclient);
            smtp.EnableSsl = enablessl;
            smtp.UseDefaultCredentials = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = frommailid;
            NetworkCred.Password = password;
            smtp.Credentials = NetworkCred;
            smtp.Port = portno;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mm);
        }
    }

    public void FunctionSendingAqcuisitionMail(DataTable dt, string ToAddress, string CCAddress, string Subject, string Msg)
    {
        string[] ColumnsToBeDeleted = { "Asset Code", "Asset code", "AssetCode", "Assetcode ", "ASSET CODE" };

        foreach (string ColName in ColumnsToBeDeleted)
        {
            if (dt.Columns.Contains(ColName))
                dt.Columns.Remove(ColName);
        }
        if (!string.IsNullOrEmpty(ToAddress))
        {
            string frommailid = ConfigurationManager.AppSettings["SENDER"];
            string password = ConfigurationManager.AppSettings["password"];

            string smtpclient = ConfigurationManager.AppSettings["SMTP_HOST"];
            int portno = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
            bool enablessl = Convert.ToBoolean(ConfigurationManager.AppSettings["enablessl"]);

            MailMessage mm = new MailMessage();
            MailAddress from = new MailAddress(frommailid);
            mm.From = from;
            mm.IsBodyHtml = false;
            //mm.To.Add(ToAddress);
            if (ToAddress.Contains(","))
            {
                string[] Tomail = ToAddress.Split(',');
                for (int i = 0; i < Tomail.Length; i++)
                {
                    mm.To.Add(new MailAddress(Tomail[i].ToString()));
                }
            }
            else
            {
                mm.To.Add(new MailAddress(ToAddress));
            }
            if (!string.IsNullOrEmpty(CCAddress))
            {
                if (CCAddress.Contains(","))
                {
                    string[] ccmail = CCAddress.Split(',');
                    for (int i = 0; i < ccmail.Length; i++)
                    {
                        mm.CC.Add(new MailAddress(ccmail[i].ToString()));
                    }
                }
                else
                {
                    mm.CC.Add(new MailAddress(CCAddress));
                }
            }
            mm.Subject = Subject;
            mm.IsBodyHtml = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='80%' cellspacing='0' cellpadding='2'>");
            sb.Append("<tr><td align='center' style='background-color: #EEE8AA' colspan = '2'><b>Asset Details</b></td></tr>");
            sb.Append("</table>");
            sb.Append("<br />");
            sb.Append("<table style='border: 2px double #006600;'>");
            sb.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append("<th style = 'background-color: BDB76B;  border: 1px solid #dddddd;font-size:10px;'>");
                sb.Append(column.ColumnName);
                sb.Append("</th>");
            }
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr style = 'font-size:10px;'>");
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append("<td style='border: 1px solid #dddddd;'>");
                    sb.Append(row[column]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            StringBuilder sbMsg = new StringBuilder();
            //sbMsg.AppendLine("Dear User,");
            //sbMsg.AppendLine("");
            //sbMsg.AppendLine("<br/>");
            //sbMsg.AppendLine("Please Check the Asset Details ");
            //sbMsg.AppendLine("");
            //sbMsg.AppendLine("");
            sbMsg.AppendLine("<br/>");
            sbMsg.AppendLine(Msg);
            sbMsg.AppendLine("");
            sbMsg.AppendLine("");
            sbMsg.AppendLine("<br/>"); sbMsg.AppendLine("<br/>"); sbMsg.AppendLine("<br/>");
            sbMsg.AppendLine("");
            sbMsg.AppendLine("<br/>");
            sbMsg.AppendLine("");
            sbMsg.AppendLine("");
            StringBuilder sMsg = new StringBuilder();
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("Thanks and Regards,");
            sMsg.AppendLine("<br/>");
            if (clsGeneral.gStrCompType == "IT")
            {
                sMsg.AppendLine("GOSC-ShiftLeads – IT Operations|MetLife");
            }
            else
            {
                sMsg.AppendLine("GOSC-ShiftLeads – MetLife");
            }
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("**This is an auto system generated email**");
            sMsg.AppendLine("");
            sMsg.AppendLine("");
            mm.Body = sbMsg.ToString() + sb.ToString() + sMsg.ToString();
            SmtpClient smtp = new SmtpClient(smtpclient);
            smtp.EnableSsl = enablessl;
            smtp.UseDefaultCredentials = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = frommailid;
            NetworkCred.Password = password;
            smtp.Credentials = NetworkCred;
            smtp.Port = portno;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mm);
        }
    }

    public void FunctionSendingMailWithAssetData(DataTable dt, string ToAddress, string CCAddress, string Subject, string Msg)
    {
        string[] ColumnsToBeDeleted = { "Asset Code", "Asset code", "AssetCode", "Assetcode ", "ASSET CODE" };

        foreach (string ColName in ColumnsToBeDeleted)
        {
            if (dt.Columns.Contains(ColName))
                dt.Columns.Remove(ColName);
        }
        if (!string.IsNullOrEmpty(ToAddress))
        {
            string frommailid = ConfigurationManager.AppSettings["SENDER"];
            string password = ConfigurationManager.AppSettings["password"];

            string smtpclient = ConfigurationManager.AppSettings["SMTP_HOST"];
            int portno = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
            bool enablessl = Convert.ToBoolean(ConfigurationManager.AppSettings["enablessl"]);

            MailMessage mm = new MailMessage();
            MailAddress from = new MailAddress(frommailid);
            mm.From = from;
            mm.IsBodyHtml = false;
            //mm.To.Add(ToAddress);
            if (ToAddress.Contains(","))
            {
                string[] Tomail = ToAddress.Split(',');
                for (int i = 0; i < Tomail.Length; i++)
                {
                    mm.To.Add(new MailAddress(Tomail[i].ToString()));
                }
            }
            else
            {
                mm.To.Add(new MailAddress(ToAddress));
            }
            if (!string.IsNullOrEmpty(CCAddress))
            {
                if (CCAddress.Contains(","))
                {
                    string[] ccmail = CCAddress.Split(',');
                    for (int i = 0; i < ccmail.Length; i++)
                    {
                        mm.CC.Add(new MailAddress(ccmail[i].ToString()));
                    }
                }
                else
                {
                    mm.CC.Add(new MailAddress(CCAddress));
                }
            }
            mm.Subject = Subject;
            mm.IsBodyHtml = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='80%' cellspacing='5' cellpadding='2'>");
            sb.Append("<tr><td align='center' style='background-color: #EEE8AA'  colspan = '2'><b>Asset Details</b></td></tr>");
            sb.Append("</table>");
            sb.Append("<br />");
            sb.Append("<table style='border: 2px double #006600;'>");
            sb.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append("<th style = 'background-color: BDB76B; border: 1px solid #dddddd;  font-size:10px;'>");
                sb.Append(column.ColumnName);
                sb.Append("</th>");
            }
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr style = 'font-size:10px;'>");
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append("<td style=' border: 1px solid #dddddd;'>");
                    sb.Append(row[column]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            StringBuilder sbMsg = new StringBuilder();
            //sbMsg.AppendLine("Dear User,");
            //sbMsg.AppendLine("");
            //sbMsg.AppendLine("<br/>");
            //sbMsg.AppendLine("Please Check the Asset Details ");
            //sbMsg.AppendLine("");
            //sbMsg.AppendLine("");
            //sbMsg.AppendLine("<br/>");
            sbMsg.AppendLine(Msg);
            sbMsg.AppendLine("");
            sbMsg.AppendLine("");
            sbMsg.AppendLine("<br/>"); sbMsg.AppendLine("<br/>"); sbMsg.AppendLine("<br/>");
            sbMsg.AppendLine("");
            sbMsg.AppendLine("<br/>");
            sbMsg.AppendLine("");
            sbMsg.AppendLine("");
            StringBuilder sMsg = new StringBuilder();
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("");
            sMsg.AppendLine("");
            sMsg.AppendLine("");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("Thanks and Regards,");
            sMsg.AppendLine("<br/>");
            if (clsGeneral.gStrCompType == "IT")
            {
                sMsg.AppendLine("GOSC-ShiftLeads – IT Operations|MetLife");
            }
            else
            {
                sMsg.AppendLine("GOSC-ShiftLeads – MetLife");
            }
            sMsg.AppendLine("<br/>");
            sMsg.AppendLine("**This is an auto system generated email**");
            sMsg.AppendLine("");
            sMsg.AppendLine("");
            mm.Body = sbMsg.ToString() + sb.ToString() + sMsg.ToString();
            SmtpClient smtp = new SmtpClient(smtpclient);
            smtp.EnableSsl = enablessl;
            smtp.UseDefaultCredentials = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = frommailid;
            NetworkCred.Password = password;
            smtp.Credentials = NetworkCred;
            smtp.Port = portno;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mm);
        }
    }
}