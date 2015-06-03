using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace OrixMvc.Objects
{
    public static class sendMail
    {
        /// <summary>
        /// 透過.net mail 寄出
        /// </summary>
        /// <param name="strFrom">mail from </param>
        /// <param name="strTo">mail to</param>
        /// <param name="strCc">mail cc</param>
        /// <param name="strSubject">subject</param>
        /// <param name="strBody">mail contact</param>
        public static void SendNow(string strFrom, string strTo, string strCc, string strSubject, string strBody)
        {
            SendNow(strFrom, "", strTo, strCc, strSubject, strBody);            
        }

        /// <summary>
        /// 透過.net mail 寄出
        /// </summary>
        /// <param name="strFrom">mail from </param>
        /// /// <param name="strTo">mail to 密件</param>
        /// <param name="strTo">mail to</param>
        /// <param name="strCc">mail cc</param>
        /// <param name="strSubject">subject</param>
        /// <param name="strBody">mail contact</param>
        public static void SendNow(string strFrom, string strBCc,string strTo, string strCc, string strSubject, string strBody)
        {
            strTo = strTo.Replace(';', ',');
            MailMessage mailObj = new MailMessage(strFrom, strTo, strSubject, strBody);
            if (strCc != "")
                mailObj.CC.Add(strCc.Replace(";", ","));

            if (strBCc!="")
                mailObj.Bcc.Add(strBCc.Replace(";", ","));
             
            mailObj.IsBodyHtml = true;
            mailObj.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient(ConfigurationSettings.AppSettings["SMTP"]);
            smtp.Send(mailObj);
            

        }

    }
}
