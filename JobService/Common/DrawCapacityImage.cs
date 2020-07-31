using JobService.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace JobService.Common
{
    public class DrawCapacityImage
    {
        private List<LogiCapacityModel> showModelList;

        public DrawCapacityImage(List<LogiCapacityModel> showModelList)
        {
            this.showModelList = showModelList;
        }
        

        /// <summary>
        /// 产能分布图
        /// </summary>
        public void DrawImageByPorduction(string imageName)
        {
            Chart myChart = new Chart();
            InitializeChart(myChart, imageName);
            Series seriles01 = this.SetSeriesStyle(0); //投入
            Series seriles02 = this.SetSeriesStyle(1); //产出  
            Series seriles03 = this.SetSeriesStyle(2);//良率  
            seriles03.YAxisType = AxisType.Secondary;
            foreach (LogiCapacityModel showModel in showModelList)
            {
                DataPoint point = new DataPoint();
                point.SetValueXY(showModel.StationName, showModel.TotalInput);
                point.Label = showModel.TotalInput.ToString();
                seriles01.Points.Add(point);
            }

            foreach (LogiCapacityModel showModel in showModelList)
            {
                DataPoint point = new DataPoint();
                point.SetValueXY(showModel.StationName, showModel.OUTPUT_QTY);
                point.Label = showModel.OUTPUT_QTY.ToString();
                seriles02.Points.Add(point);
            }

            foreach (LogiCapacityModel showModel in showModelList)
            {
                decimal goodRate = Math.Round(showModel.OUTPUT_QTY * 100 / (showModel.TotalInput),2);
                DataPoint point = new DataPoint();
                point.SetValueXY(showModel.StationName, goodRate);
                point.Label = goodRate + "%";
                seriles03.Points.Add(point);
            }
            myChart.Series.Add(seriles01); myChart.Series.Add(seriles02); myChart.Series.Add(seriles03);
            //保存图片
            myChart.SaveImage(AppDomain.CurrentDomain.BaseDirectory + "Img\\" + DateTime.Now.ToString("yyyy-MM-dd") + $"{imageName}.jpeg",
                        System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        private Series SetSeriesStyle(int i)
        {
            Series series = new Series();
            switch (i)
            {
                case 0:
                    series.Name = "InPut";
                    series.ChartType = SeriesChartType.Column;
                    break;

                case 1:
                    series.Name = "Output";
                    series.ChartType = SeriesChartType.Column;
                    break;
                case 2:
                    series.Name = "Yield Rate(%)";
                    series.ChartType = SeriesChartType.Line;
                    break;
            }
            //Series的边框颜色
            series.BorderColor = Color.FromArgb(180, 26, 59, 105);
            //线条宽度
            series.BorderWidth = 3;
            //线条阴影颜色
            series.ShadowColor = Color.Black;
            //阴影宽度
            series.ShadowOffset = 2;
            //是否显示数据说明
            series.IsVisibleInLegend = true;
            //线条上数据点上是否有数据显示
            series.IsValueShownAsLabel = true;
            //线条上的数据点标志类型
            series.MarkerStyle = MarkerStyle.Circle;
            //线条数据点的大小
            series.MarkerSize = 12;
            // series.Font=
            //series.Label
            //线条颜色
            switch (i)
            {
                case 0:
                    series.Color = Color.FromArgb(220, 225, 128, 192);
                    series.Font = new System.Drawing.Font("Microsoft Sans Serif", 16, FontStyle.Bold);
                    break;
                case 1:
                    series.Color = Color.FromArgb(220, 65, 140, 240);
                    series.Font = new System.Drawing.Font("Microsoft Sans Serif", 16, FontStyle.Bold);
                    break;
                case 2:
                    series.Color = Color.FromArgb(220, 224, 64, 10);
                    series.Font = new System.Drawing.Font("Microsoft Sans Serif", 16, FontStyle.Bold);
                    break;
            }
            return series;
        }

        /// <summary>
        /// 初始化Char控件样式
        /// </summary>
        private void InitializeChart(Chart myChart,string imageName)
        {
            myChart.Height = 800;
            myChart.Width = 2000;
            #region 设置图表的属性
            //图表的背景色
            myChart.BackColor = Color.FromArgb(211, 223, 240);
            //图表背景色的渐变方式
            myChart.BackGradientStyle = GradientStyle.TopBottom;
            //图表的边框颜色、
            myChart.BorderlineColor = Color.FromArgb(26, 59, 105);
            //图表的边框线条样式
            myChart.BorderlineDashStyle = ChartDashStyle.Solid;
            //图表边框线条的宽度
            myChart.BorderlineWidth = 2;
            //图表边框的皮肤
            myChart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            #endregion

            #region 设置图表的Title
            Title title = new Title();
            //标题内容
            title.Text = $"{imageName}";
            //标题的字体
            title.Font = new System.Drawing.Font("Microsoft Sans Serif", 32, FontStyle.Bold);
            //标题字体颜色
            title.ForeColor = Color.FromArgb(26, 59, 105);
            //标题阴影颜色
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            //标题阴影偏移量
            title.ShadowOffset = 3;

            myChart.Titles.Add(title);
            #endregion

            #region 设置图表区属性
            //图表区的名字
            ChartArea chartArea = new ChartArea("Default");
            //背景色
            chartArea.BackColor = Color.FromArgb(64, 165, 191, 228);
            //背景渐变方式
            chartArea.BackGradientStyle = GradientStyle.TopBottom;
            //渐变和阴影的辅助背景色
            chartArea.BackSecondaryColor = Color.White;
            //边框颜色
            chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            //阴影颜色
            chartArea.ShadowColor = Color.Transparent;

            //设置X轴和Y轴线条的颜色和宽度
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LineWidth = 1;
            chartArea.AxisY2.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY2.LineWidth = 1;
            chartArea.AxisX.Interval = 1;


            chartArea.AxisX.LabelAutoFitMinFontSize = 16;
            chartArea.AxisY.LabelAutoFitMinFontSize = 16; //Y轴字体大小
            chartArea.AxisY2.LabelAutoFitMinFontSize = 16;
            chartArea.AxisY2.LabelStyle = new LabelStyle() { Format = "{#}.0%" }; //Y轴显示样式
                                                                                  //    chartArea.AxisX.LabelStyle = new LabelStyle() { Format = "{#}." }; //Y轴显示样式
                                                                                  // chartArea.AxisY.Maximum = 100;
                                                                                  //var s = from p in data[2]
                                                                                  //        where p != 0
                                                                                  //        select p;
                                                                                  //chartArea.AxisY.Minimum = (int)s.Min() - 2;  //找出Y轴非0的最小值
                                                                                  //   chartArea.AxisY.Interval = 500;

            //设置Y轴2
            chartArea.AxisY2.LabelAutoFitMinFontSize = 10;
            //chartArea.AxisY2.Maximum = data[0].Max() + 10000;
            //var min = from p in data[0]
            //          where p != 0
            //          select p;
            //if (min.Min() > 10000)
            //    chartArea.AxisY2.Minimum = min.Min() - 10000;
            //else
            //    chartArea.AxisY2.Minimum = 0;
            chartArea.AxisY2.Interval = 20;


            //chartArea.AxisY.MajorTickMark = new TickMark() {
            //    TickMarkStyle=TickMarkStyle.OutsideArea,Size=2
            //};


            //设置X轴和Y轴的标题
            //  chartArea.AxisX.Title = "天数";
            //  chartArea.AxisY.Title = "良率";
            chartArea.AxisY2.Title = "良率(%)";
            chartArea.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            chartArea.AxisY.Title = "数量";
            chartArea.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            //设置图表区网格横纵线条的颜色和宽度
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisY2.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY2.MajorGrid.LineWidth = 1;

            myChart.ChartAreas.Add(chartArea);
            #endregion

            #region 图例及图例的位置
            Legend legend = new Legend();
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Top;

            myChart.Legends.Add(legend);
            #endregion
        }
    }
}
