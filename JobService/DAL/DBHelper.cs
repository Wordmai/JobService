﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using JobService.Model;
using System.Data;
using JobService.Common;
using Dapper;
using System.Data.SqlClient;

namespace JobService.DAL
{
    public class DBHelper
    {

        private const string S_SelectPasteInfo = @"SELECT REEL_NO,DATE_CODE,CURRENT_STATUS FROM SAJET.G_MATERIAL_SOLDER WHERE CURRENT_STATUS IN ('F','W','M')";

        private const string S_SelectEmailInfo = @"SELECT * FROM SAJET.SYS_BASE WHERE PARAM_NAME='BU23'";

        private const string I_InsertPasteInfo = @"INSERT INTO SAJET.G_HT_MATERIAL_SOLDER 
                                                  SELECT REEL_NO,PART_ID,PDLINE_ID,DATE_CODE,RETURN_COUNT,MIX_MIN,CURRENT_STATUS,
                                                  CURRENT_TIME,CURRENT_USERID,LAST_STATUS,LAST_TIME,LAST_USERID,CREATE_TIME,
                                                  CREATE_USERID,MEMO,TAKEOUT_TIME,TAKEOUT_USERID,OPENED,OPEN_TIME,OPEN_USERID,
                                                  PICK_EMP_ID,C_PART_ID 
                                                  FROM SAJET.G_MATERIAL_SOLDER 
                                                  WHERE REEL_NO =:REEL_NO";

        private const string U_UpdatePasteStatus = @"UPDATE SAJET.G_MATERIAL_SOLDER 
                                                   SET  LAST_TIME=CURRENT_TIME,LAST_STATUS=CURRENT_STATUS,CURRENT_STATUS='S',CURRENT_TIME=SYSDATE 
                                                   WHERE  REEL_NO=:REEL_NO";

        private const string S_SelectLogiCapacity = @"SELECT
                                                      	B.PROCESS_NAME,
                                                      	SUM( A.PASS_QTY ) AS PASS_QTY,
                                                      	SUM( A.FAIL_QTY ) AS FAIL_QTY,
                                                      	SUM( A.REPASS_QTY ) AS REPASS_QTY,
                                                      	SUM( A.REFAIL_QTY ) AS REFAIL_QTY,
                                                      	SUM( A.OUTPUT_QTY ) AS OUTPUT_QTY 
                                                      FROM
                                                      	SAJET.G_SN_COUNT A , SAJET.SYS_PROCESS B
                                                      WHERE
                                                      	A.PDLINE_ID IN ( :ASSLINE, :PACKLINE ) 
                                                      	AND A.WORK_DATE = :WORK_DATE
                                                      	AND A.WORK_TIME IN(:LASTHOUR,:HOUR)
                                                      	AND A.PROCESS_ID = B.PROCESS_ID
                                                      GROUP BY
                                                        	B.PROCESS_NAME";
                                                        
        private const string S_SelectProcessName = @"SELECT PROCESS_NAME FROM SAJET.SYS_PROCESS WHERE PROCESS_ID = :PROCESS_ID";

        private const string S_SelectStationSEQ = @"SELECT PROGRAM FROM SAJET.SYS_BASE 
                                                    WHERE PARAM_NAME = :PARAM_NAME 
                                                    AND   PARAM_VALUE = :PARAM_VALUE ";

        /// <summary>
        /// 获取冰库、回温、搅拌状态锡膏的全部信息
        /// </summary>
        /// <returns>锡膏全部信息</returns>
        public List<LuxSolder> GetAllSolderInfo(OracleConnection connection)
        {
            List<LuxSolder> pasteList = new List<LuxSolder>();
            using (connection)
            {
                pasteList = connection.Query(S_SelectPasteInfo).Select
                  (
                  p => new LuxSolder
                  {
                      REEL_NO = p.REEL_NO,
                      DATE_CODE = p.DATE_CODE,
                      CURRENT_STATUS = p.CURRENT_STATUS
                  }
                  ).ToList();
                return pasteList;
            }
        }

        /// <summary>
        /// 获取发送邮件接收人和抄送人
        /// </summary>
        /// <returns>发送人和抄送人的全部信息</returns>
        public void GetEmailPerson(OracleConnection connection, string program, ref List<string> recipientList, ref List<string> ccList)
        {
            using (connection)
            {
                List<dynamic> emailList = new List<dynamic>();
                emailList = connection.Query<dynamic>(S_SelectEmailInfo).ToList();
                foreach (var emailinfo in emailList)
                {
                    if (emailinfo.PROGRAM == program + "TO")
                        recipientList.Add(emailinfo.PARAM_VALUE);
                    else if (emailinfo.PROGRAM == program + "CC")
                        ccList.Add(emailinfo.PARAM_VALUE);
                }
            }

        }

        /// <summary>
        /// 备份锡膏信息
        /// </summary>
        /// <param name="solder">锡膏对象</param>
        public void CopySolderMess(OracleConnection connection, LuxSolder solder)
        {
            using (connection)
            {
                connection.Execute(I_InsertPasteInfo, new { REEL_NO = solder.REEL_NO });
            }

        }

        /// <summary>
        /// 报废锡膏
        /// </summary>
        /// <param name="solder">锡膏对象</param>
        public void UpdateSolderStatusToScrap(OracleConnection connection, LuxSolder solder)
        {
            using (connection)
            {
                connection.Execute(U_UpdatePasteStatus, new { REEL_NO = solder.REEL_NO });
            }

        }

        /// <summary>
        /// 取Logi两小时产能
        /// </summary>
        /// <param name="work_date"></param>
        /// <param name="work_time"></param>
        /// <returns></returns>
        public List<LogiCapacityModel> GetLogiCapacity(OracleConnection connection,string assLine,string packgeLine, string work_date, int work_time)
        {
            using (connection)
            {
                var quaryPara = new
                {
                    WORK_DATE = work_date,
                    LASTHOUR = work_time - 1,
                    HOUR = work_time,
                    ASSLINE = assLine,
                    PACKLINE = packgeLine
                };
                List<LogiCapacityModel> logiCapacityInfo = connection.Query(S_SelectLogiCapacity, quaryPara).Select
                    (
                    p => new LogiCapacityModel
                    {
                        TimeRange = $"{work_time - 1}-{work_time}",
                        PASS_QTY = p.PASS_QTY,
                        FAIL_QTY = p.FAIL_QTY,
                        REFAIL_QTY = p.REFAIL_QTY,
                        REPASS_QTY = p.REPASS_QTY,
                        OUTPUT_QTY = p.OUTPUT_QTY,
                        StationName = p.PROCESS_NAME,
                        StationSEQ = GetStationSeq(ConnectionFactory.CreatKSConnection(), "SL001_SEQ",p.PROCESS_NAME)
                    }
                    ).OrderBy(p => p.StationSEQ).ToList();
                return logiCapacityInfo;
            }


        }

        /// <summary>
        /// 根据ProcessID取ProcessName
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public string GetProcessName(OracleConnection connection, decimal processId)
        {
            using (connection)
            {
                string processName = connection.QueryFirst<string>(S_SelectProcessName, new { PROCESS_ID = processId });
                return processName;
            }

        }

        /// <summary>
        /// 根据站点名称获取排列顺序
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="param_name"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public decimal GetStationSeq(OracleConnection connection,string param_name ,string stationName)
        {
            using (connection)
            {
                decimal seq = 0;
                var queryPara = new
                {
                    PARAM_NAME = param_name,
                    PARAM_VALUE = stationName
                };
                seq = connection.QueryFirst<decimal>(S_SelectStationSEQ, queryPara);
                return seq;
            }
        }
    }
}
