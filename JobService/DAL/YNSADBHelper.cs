using System;
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
    public class YNSADBHelper
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



        /// <summary>
        /// 获取锡膏的全部信息
        /// </summary>
        /// <returns>锡膏全部信息</returns>
        public List<LuxSolder> GetAllSolderInfo()
        {
            List<LuxSolder> pasteList = new List<LuxSolder>();
            using (OracleConnection connection = ConnectionFactory.CreatYNConnection())
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
            }

            return pasteList;
        }

        /// <summary>
        /// 获取发送邮件接收人
        /// </summary>
        /// <returns>发送人和抄送人的全部信息</returns>
        public List<dynamic> GetEmailPerson()
        {
            List<dynamic> emailList = new List<dynamic>();
            using (OracleConnection connection = ConnectionFactory.CreatYNConnection())
            {
                emailList = connection.Query<dynamic>(S_SelectEmailInfo).ToList();
            }
            return emailList;
        }

        /// <summary>
        /// 备份锡膏信息
        /// </summary>
        /// <param name="solder">锡膏对象</param>
        public void CopySolderMess(LuxSolder solder)
        {
            using (OracleConnection connection = ConnectionFactory.CreatYNConnection())
            {
                connection.Execute(I_InsertPasteInfo, new { REEL_NO = solder.REEL_NO });
            }
        }

        /// <summary>
        /// 报废锡膏
        /// </summary>
        /// <param name="solder">锡膏对象</param>
        public void UpdateSolderStatusToScrap(LuxSolder solder)
        {
            using (OracleConnection connection = ConnectionFactory.CreatYNConnection())
            {
                connection.Execute(U_UpdatePasteStatus, new { REEL_NO = solder.REEL_NO });
            }
        }
    }
}
