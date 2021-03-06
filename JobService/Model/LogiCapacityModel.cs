﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Model
{
    public class LogiCapacityModel 
    {
        public string TimeRange { get; set; }
        public decimal PASS_QTY { get; set; }
        public decimal FAIL_QTY { get; set; }
        public decimal REPASS_QTY { get; set; }
        public decimal REFAIL_QTY { get; set; }
        public decimal OUTPUT_QTY { get; set; }
        public decimal TotalInput
        {
            get { return OUTPUT_QTY + FAIL_QTY + REFAIL_QTY; }
        }
        public decimal TotalFAIL
        {
            get { return FAIL_QTY + REFAIL_QTY; }
        }
        public string StationName { get; set; }
        public decimal StationSEQ { get; set; }
    }
}
