﻿using JobService.Common;
using JobService.DAL;
using JobService.Model;
using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Job
{
    public class LogiCapacityPush : IJob
    {
        DBHelper dBHelper = new DBHelper();
        public void Execute(IJobExecutionContext context)
        {
            LogHelper.WriteLog($"开始执行 LogiCapacityPush");
            try
            {
                string today = DateTime.Now.ToString("yyyyMMdd");
                int hour = DateTime.Now.AddHours(-1).Hour;
                //获取前两小时产能
                List<LogiCapacityModel> logiCapacityModelList = dBHelper.GetLogiCapacity(ConnectionFactory.CreatKSConnection(), "10615", "10613", today, hour);
                if (logiCapacityModelList != null && logiCapacityModelList.Count > 0) //有数据才发送邮件
                {
                    //获取邮件Mail页面信息
                    var mailBodyText = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\View\LogiCapacityPage.html");
                    StringBuilder sb = new StringBuilder();
                    foreach (var capacityInfo in logiCapacityModelList)
                    {
                        sb.Append(string.Format("<tr><td style='text-align:center;'>{0}</td>", capacityInfo.StationName));
                        sb.Append(string.Format("<td style='text-align:center;'>{0}</td>", $"{hour - 1}-{hour + 1}"));
                        sb.Append(string.Format("<td style='text-align:center;'>{0}</td>", capacityInfo.PASS_QTY));
                        sb.Append(string.Format("<td style='text-align:center;'>{0}</td>", capacityInfo.TotalFAIL));
                        sb.Append(string.Format("<td style='text-align:center;'>{0}</td>", capacityInfo.REPASS_QTY));
                        sb.Append(string.Format("<td style='text-align:center;'>{0}</td>", capacityInfo.OUTPUT_QTY));
                    }

                    //将页面中的TableContent替换
                    var register_mailBody = mailBodyText.Replace("{TableContent}", sb.ToString());
                    //获取收件人信息
                    List<string> recipientList = new List<string>();
                    List<string> ccList = new List<string>();
                    //recipientList.Add("Tengfei.Mai@luxshare-ict.com");
                    string program = "LOGIMAIL";
                    dBHelper.GetEmailPerson(ConnectionFactory.CreatKSConnection(), program, ref recipientList, ref ccList);

                    //定义邮件属性
                    MailInfo info = new MailInfo();
                    info.MailBody = register_mailBody;
                    info.MailSubject = string.Format("Logi产能推送");
                    info.MailToArray = recipientList;
                    info.MailCCArray = ccList;

                    //绘制产能图片
                    LogHelper.WriteLog($"开始绘制产能图片");
                    string imageName = "Two hour capacity";
                    DrawCapacityImage drawImage = new DrawCapacityImage(logiCapacityModelList);
                    drawImage.DrawImageByPorduction(imageName);
                    //将产能图片加载到Mail中
                    string productCapacityPic = string.Empty;
                    Attachment am = new Attachment(AppDomain.CurrentDomain.BaseDirectory + "Img\\" + DateTime.Now.ToString("yyyy-MM-dd") + $"{imageName}.jpeg");
                    am.ContentDisposition.Inline = true;
                    am.NameEncoding = System.Text.Encoding.UTF8;
                    am.ContentId = "s+" + DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString();
                    info.File = am;
                    productCapacityPic = $"<img src = 'cid:{am.ContentId}' />";
                    info.MailBody = info.MailBody.Replace("{ContentId}", am.ContentId).Replace("{NowTime}", DateTime.Now.ToString("yyyy-MM-dd"));

                    //发送邮件
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
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"{ex.Message}");
                throw;
            }
            
        }
    }
}
