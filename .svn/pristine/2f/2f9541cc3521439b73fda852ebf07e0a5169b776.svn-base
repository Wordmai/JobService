using JobService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Common
{
    public class SendMail
    {
        public static void SendMailMethod(MailInfo info)
        {
            using (MailMessage myMail = new MailMessage())
            {
                myMail.From = new MailAddress(info.MailFrom);
                if (info.MailToArray != null)
                {
                    foreach (string mailTo in info.MailToArray)
                        myMail.To.Add(mailTo);
                }
                if (info.MailCCArray != null)
                {
                    foreach (string mailCC in info.MailCCArray)
                        myMail.CC.Add(mailCC);
                }
                if(info.File !=null)
                {
                    myMail.Attachments.Add(info.File);
                }
                myMail.Subject = info.MailSubject;
                myMail.Body = info.MailBody;
                myMail.BodyEncoding = Encoding.Default;
                myMail.Priority = MailPriority.Normal;
                myMail.IsBodyHtml = true;
                //不被当作垃圾邮件的关键代码--Begin     
                myMail.Headers.Add("X-Priority", "3");
                myMail.Headers.Add("X-MSMail-Priority", "Normal");
                myMail.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");   //本文以outlook名义发送邮件，不会被当作垃圾邮件            
                myMail.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
                myMail.Headers.Add("ReturnReceipt", "1");
                using (SmtpClient smtp = new SmtpClient())
                {
                    //指定发件人邮箱地址和密码用于验证发件人身份
                    smtp.Credentials = new System.Net.NetworkCredential(info.MailHostAccount, info.MailHostPassword);
                    smtp.Host = info.Host;
                    smtp.Port = info.Port;
                    try
                    {
                        smtp.Send(myMail);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        //   stream.Close();
                    }

                }
            }
        }
    }
}
