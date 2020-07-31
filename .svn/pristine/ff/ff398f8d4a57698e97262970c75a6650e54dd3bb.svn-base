using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Model
{
    public class MailInfo
    {
        //发件者
        public string MailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["mailFrom"].ToString();
            }
        }

        //收件者
        public List<string> MailToArray { get; set; }

        //抄送人
        public List<string> MailCCArray { get; set; }

        //邮件标题
        public string MailSubject { get; set; }
        //正文
        public string MailBody { get; set; }

        //Host
        public string MailHostAccount
        {
            get
            { return ConfigurationManager.AppSettings["mailHost"].ToString(); }
        }
        //邮件服务器密码
        public string MailHostPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["mailHostPassword"].ToString();
            }
        }

        //邮件服务器IP
        public string Host
        {
            get
            {
                return ConfigurationManager.AppSettings["Host"].ToString();
            }
        }

        //端口号
        public int Port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            }
        }

        //附带文件
        public Attachment File{ get; set; }
    }
}
