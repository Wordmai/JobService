using JobService.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace JobService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.WriteLog("开始执行windows推送服务");
            //执行的任务
            JobManage.StartScheduleFromConfigAsync();
        }

        protected override void OnStop()
        {
            LogHelper.WriteLog("Job ShutDown");
            JobManage.ShutDown();
        }
    }
}
