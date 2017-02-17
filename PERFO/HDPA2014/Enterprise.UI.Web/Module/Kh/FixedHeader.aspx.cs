using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;
using System.Text;
using Enterprise.Model.Perfo;
using System.IO;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    ///固定表头和冻结列
    /// </summary>
    public partial class FixedHeader : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid() 
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataRow dr;
            //添加列名
            dt.Columns.Add(new System.Data.DataColumn("学生班级", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("学生姓名", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("语文", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("数学", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("英语", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("计算机", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("物理", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("化学", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("生物", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("地理", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("历史", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("美术", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("政治", typeof(System.Decimal)));

            //用循环添加行数据
            for (int i = 0; i < 50; i++)
            {
                System.Random rd = new System.Random(Environment.TickCount * i); ;
                dr = dt.NewRow();
                dr[0] = "班级" + i.ToString();
                dr[1] = "虚拟人" + i.ToString();
                dr[2] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[3] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[4] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[5] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[6] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[7] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[8] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[9] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[10] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[11] = System.Math.Round(rd.NextDouble() * 100, 2);
                dr[12] = System.Math.Round(rd.NextDouble() * 100, 2);
                dt.Rows.Add(dr);
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        #endregion

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //判断创建的行是否为表头行
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //DynamicTHeaderHepler dHelper = new DynamicTHeaderHepler();
                ////"归属单位#序号#单位名称#效益类指标|标准分,考核得分#管理类指标|标准分,考核得分#综合得分#扣分情况#加分情况#最终得分#排名"
                //string header = "归属单位#序号#单位名称#效益类指标|标准分,考核得分#管理类指标|标准分,考核得分#综合得分#扣分情况#加分情况#最终得分#排名";
                //dHelper.SplitTableHeader(e.Row, header);
            }
        }
    }
}