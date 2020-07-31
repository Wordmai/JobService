using JobService.Common;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JobService
{
    [DisallowConcurrentExecution]
    public class JobManage
    {
        private static ISchedulerFactory sf = new StdSchedulerFactory();
        //调度器
        private static IScheduler scheduler;
        /// <summary>
        /// 读取调度器配置文件的开始时间
        /// </summary>
        public static void StartScheduleFromConfigAsync()
        {
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                XDocument xDoc = XDocument.Load(Path.Combine(currentDir, "JobScheduler.config"));
                var jobScheduler = from x in xDoc.Descendants("JobScheduler") select x;

                var jobs = jobScheduler.Elements("Job");
                XElement jobDetailXElement, triggerXElement;
                //获取调度器
                scheduler = sf.GetScheduler();
                //声明触发器
                CronTriggerImpl cronTrigger;
                foreach (var job in jobs)
                {
                    //加载程序集joblibaray  
                    Assembly ass = Assembly.GetAssembly(typeof(JobManage));  //Assembly.LoadFrom(Path.Combine(currentDir + "\\bin", job.Element("DllName").Value));
                    //获取任务名字
                    jobDetailXElement = job.Element("JobDetail");
                    
                    //获取任务触发的时间
                    triggerXElement = job.Element("Trigger");
                    JobDetailImpl jobDetail = new JobDetailImpl(jobDetailXElement.Attribute("job").Value,
                                                           jobDetailXElement.Attribute("group").Value,
                                                           ass.GetType(jobDetailXElement.Attribute("jobtype").Value));

                    if (triggerXElement.Attribute("type").Value.Equals("CronTrigger"))
                    {
                        cronTrigger = new CronTriggerImpl(triggerXElement.Attribute("name").Value,
                                                       triggerXElement.Attribute("group").Value,
                                                       triggerXElement.Attribute("expression").Value);
                        //添加定时器
                        scheduler.ScheduleJob(jobDetail, cronTrigger);
                    }
                }
                //开始执行定时器
                scheduler.Start();
            }
            catch (Exception E)
            {
                LogHelper.WriteLog(E.Message);
                throw new Exception(E.Message);
            }
        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        public static void ShutDown()
        {
            if (scheduler != null && !scheduler.IsShutdown)
            {
                scheduler.Shutdown();
            }
        }

        /// <summary>
        /// 从Scheduler 移除当前的Job,修改Trigger   
        /// </summary>
        /// <param name="jobExecution"></param>
        /// <param name="time"></param>
        public static void ModifyJobTime(IJobExecutionContext jobExecution, String time)
        {
            scheduler = jobExecution.Scheduler;
            ITrigger trigger = jobExecution.Trigger;
            IJobDetail jobDetail = jobExecution.JobDetail;
            if (trigger != null)
            {
                CronTriggerImpl ct = (CronTriggerImpl)trigger;
                // 移除当前进程的Job     
                scheduler.DeleteJob(jobDetail.Key);
                // 修改Trigger     
                ct.CronExpressionString = time;
                Console.WriteLine("CronTrigger getName " + ct.JobName);
                // 重新调度jobDetail     
                scheduler.ScheduleJob(jobDetail, ct);
            }
        }
    }
}
