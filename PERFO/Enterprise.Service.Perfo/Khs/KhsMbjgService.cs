using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Khs;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Service.Perfo.Khs
{
	
    /// <summary>
    /// 文件名:  KhsMbjgService.cs
    /// 功能描述: 业务逻辑层-模板机构数据处理
    /// 创建人：代码生成器
	/// 创建时间 ：2015/11/5 22:09:24
    /// </summary>
    public class KhsMbjgService
    {
        public bool DeleteById(string Id,string kh)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("delete from PERFO_KHS_MBJG where MBID='" + Id + "' and KHDZBID='"+kh+"';");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update60(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='60' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid="+khid+");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        public bool Update42(string khid,string fgld)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='42' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + "and dzbid in (select dzbid from PERFO_KHS_DZB where fzbm in (select jgbm from PERFO_SYS_FGBMJG where loginid = '" + fgld + "')));");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update50(string khid,string fgld)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='50' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + "and dzbid in (select dzbid from PERFO_KHS_DZB where fzbm in (select jgbm from PERFO_SYS_FGBMJG where loginid = '"+fgld+"')));");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update50(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='50' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update52(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='52' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        public bool Update51(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='51' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        //public bool Update61(string khid)
        //{
        //    StringBuilder sqls = new StringBuilder();
        //    sqls.Append("begin ");
        //    sqls.Append("update PERFO_KHS_MBJG set status='61' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
        //    sqls.Append(" end;");
        //    return dal.ExecuteSQL(sqls.ToString());
        //}
        public bool Update61(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='61' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update62(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='62' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        #region 代码生成器
        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhsMbjgData dal = new KhsMbjgData();

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsMbjgModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsMbjgModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsMbjgModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsMbjgModel model)
        {
            return dal.Execute(model);
        }
        #endregion
        public string GetStatus(KhsMbjgModel model)
        {
            string sName = GetStatus2(model.STATUS);
            

            return sName;
        }
        public string GetStatus2(string status)
        {
            string sName = "正在打分";
            //0=未审核 1=审核通过  2=审核不通过  3=打印完成
            switch (status)
            {
                case "30":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>机关部门领导</span>审核</span>";
                    break;
                case "31":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>机关部门领导</span>审核通过</span>";
                    break;
                case "32":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>机关部门领导</span>审核不通过</span>";
                    break;
                case "40":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>基层单位</span>确认</span>";
                    break;
                case "41":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>基层单位</span>确认通过</span>";
                    break;
                case "42":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>基层单位</span>确认不通过</span>";
                    break;
                case "50":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>绩效考核办</span>审核</span>";
                    break;
                case "51":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>绩效考核办</span>审核通过</span>";
                    break;
                case "52":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>绩效考核办</span>审核不通过</span>";
                    break;
                case "60":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>分管领导</span>审核</span>";
                    break;
                case "61":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>分管领导</span>审核通过</span>";
                    break;
                case "62":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>分管领导</span>不通过</span>";
                    break;
                
            }

            return sName;
        }

        public string GetStatus2(KhsMbjgModel model)
        {
            string sName = GetStatus21(model.STATUS);


            return sName;
        }
        public string GetStatus21(string status)
        {
            string sName = "<span style='color:#ffcc00'>正在打分</span>";
            //0=未审核 1=审核通过  2=审核不通过  3=打印完成
            switch (status)
            {
                case "30":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>机关部门领导</span>审核</span>";
                    break;
                case "31":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>机关部门领导</span>审核通过</span>";
                    break;
                case "32":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>机关部门领导</span>审核不通过</span>";
                    break;
                case "40":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>专业分管领导</span>审核</span>";
                    break;
                case "41":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>专业分管领导</span>审核通过</span>";
                    break;
                case "42":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>专业分管领导</span>审核不通过</span>";
                    break;
                case "50":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>绩效考核办</span>汇总</span>";
                    break;
                case "51":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>绩效考核办</span>审核通过</span>";
                    break;
                case "52":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>绩效考核办</span>审核不通过</span>";
                    break;
                case "60":
                    sName = "<span style='color:#ffcc00'>提交<span style='color:#6600cc'>分管领导</span>审核</span>";
                    break;
                case "61":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>分管领导</span>审核通过</span>";
                    break;
                case "62":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>分管领导</span>不通过</span>";
                    break;
            }

            return sName;
        }
    }

}
