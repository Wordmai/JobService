using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace JobService.Common
{
    public class ConnectionFactory
    {
        private static readonly string ksConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KS_MES"].ToString();
        private static readonly string ynConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["YN_MES"].ToString();
        private static readonly string yndvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["YNDV_MES"].ToString();


        /// <summary>
        /// 创建昆山数据库连接
        /// </summary>
        /// <returns></returns>
        public static OracleConnection CreatKSConnection()
        {
            try
            {
                var connection = new OracleConnection(ksConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 创建越南数据库连接
        /// </summary>
        /// <returns></returns>
        public static OracleConnection CreatYNConnection()
        {
            try
            {
                var connection = new OracleConnection(ynConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 创建越南测试库连接
        /// </summary>
        /// <returns></returns>
        public static OracleConnection CreatYNDVConnection()
        {
            try
            {
                var connection = new OracleConnection(yndvConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }

    }
}