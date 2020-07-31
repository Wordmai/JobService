using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Model
{
    public class LuxSolder
    {
        /// <summary>
        /// 锡膏料卷号
        /// </summary>
        public string REEL_NO { set; get; }
        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime DATE_CODE { set; get; }
        /// <summary>
        /// 当前状态
        /// 'F','冰库'
        /// 'W','回温'
        /// 'M','搅拌'
        /// 'P','领用'
        /// 'O','开封'
        /// 'U','上料'
        /// 'T','用完'
        /// 'S','报废'
        /// 'R','回冰'
        /// 'B','下料'
        /// </summary>
        public string CURRENT_STATUS { set; get; }
    }
}
