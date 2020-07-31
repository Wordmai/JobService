using JobService.Common;
using JobService.DAL;
using JobService.Model;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Job
{
    public class SolderPasteExpiredWarn : IJob
    {
        DBHelper dBHelper = new DBHelper();
        static Dictionary<string, string> dictOfStatus = new Dictionary<string, string> {
            {"F","冰库"},{"W","回温"},{"M","搅拌"},{"P","领用"},{"O","开封"},
            {"U","上料"},{"T","用完"},{"S","报废"},{"R","回冰"},{"B","下料"}};
        public void Execute(IJobExecutionContext context)
        {
                //查出来所有锡膏
                List<LuxSolder> pasteList = dBHelper.GetAllSolderInfo(ConnectionFactory.CreatYNConnection());

                //声明一个锡膏信息对象，保存预警信息
                List<LuxSolder> expirePasteList = new List<LuxSolder>();
                foreach (LuxSolder paste in pasteList)
                {
                    //对比是否达到快过期时间
                    if (checkTimeExpor(paste) < 0)
                    {
                        //直接报废
                        dBHelper.CopySolderMess(ConnectionFactory.CreatYNConnection(), paste);
                        dBHelper.UpdateSolderStatusToScrap(ConnectionFactory.CreatYNConnection(), paste);
                    }
                    else
                    {
                        //记录预警的锡膏
                        if (checkTimeExpor(paste) <= 30)
                        {
                            expirePasteList.Add(paste);
                        }
                    }

                }
                if (expirePasteList.Count > 0)
                {
                    //发送预警邮件
                    List<string> recipientList = new List<string>();
                    List<string> ccList = new List<string>();
                    string program = "SOLDEREMAIL";
                    dBHelper.GetEmailPerson(ConnectionFactory.CreatYNConnection(), program, ref recipientList, ref ccList); //获取收件人信息
                    //发送邮件
                    MailInfo info = new MailInfo();
                    info.MailBody = GeneralPasteMailBody(pasteList);
                    info.MailSubject = string.Format("锡膏过期时间预警");
                    info.MailToArray = recipientList;
                    //info.MailCCArray = ccList;
                    try
                    {
                        SendMail.SendMailMethod(info);
                        LogHelper.WriteLog($"{DateTime.Now}成功发送邮件");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message);
                    }
                }
                else
                    LogHelper.WriteLog("当前无锡膏过期预警");

        }

        #region 自定义方法
        /// <summary>
        /// 判断当前时间与保质期时间的差值
        /// </summary>
        /// <param name="solder">锡膏对象</param>
        /// <returns>int 时间差</returns>
        private int checkTimeExpor(LuxSolder solder)
        {
            //获取当前时间
            DateTime nowTime = DateTime.Now;
            DateTime endTime = Convert.ToDateTime(solder.DATE_CODE);
            TimeSpan ts = endTime - nowTime;
            //返回时间
            return Convert.ToInt32(ts.Days);
        }
        /// <summary>
        /// 生成Mail表单
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        static string GeneralPasteMailBody(List<LuxSolder> pasteList)
        {
            var mailBodyText = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\View\SolderPasteWarnPage.html");
            StringBuilder sb = new StringBuilder();
            foreach (LuxSolder paste in pasteList)
            {
                sb.Append(string.Format("<tr><td style='text-align:center;'>{0}</td>", paste.REEL_NO));
                sb.Append(string.Format("<td style='text-align:center;'>{0}</td>", paste.DATE_CODE));
                sb.Append(string.Format("<td style='text-align:center;'>{0}</td>", dictOfStatus.FirstOrDefault(d => d.Key == paste.CURRENT_STATUS).Value));
            }
            var register_mailBody = mailBodyText.Replace("{TableContent}", sb.ToString());
            return register_mailBody;
        }

        #endregion
    }
}

