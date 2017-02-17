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
    /// 文件名:  KhsKhdzbService.cs
    /// 功能描述: 业务逻辑层-考核指标对照表数据处理
    /// 创建人：代码生成器
	/// 创建时间 ：2015/11/4 20:47:10
    /// </summary>
    public class KhsKhdzbService
    {
        #region 代码生成器
        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhsKhdzbData dal = new KhsKhdzbData();

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsKhdzbModel model)
        {
            return dal.Execute(model);
        }

        public bool DeleteByKhId(string khId)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("delete from PERFO_KHS_KHDZB where KHID=" + khId + ";");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool DeleteMb(string mbid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("delete from PERFO_KHX_RESULT where mbjgid in (select id from  PERFO_KHS_MBJG where  mbid='" + mbid + "');delete from PERFO_KHS_MBJG where mbid='" + mbid + "';delete from PERFO_KHX_ZB where mbid='" + mbid + "';");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        #endregion

        public string GetStatus(KhsKhdzbModel model)
        {
            string sName = "制定中";
            //0=未审核 1=审核通过  2=审核不通过  3=打印完成
            switch (model.STATUS)
            {
                case 0:
                    sName = "已提交审核";
                    break;
                case 2:
                    sName = "审核不通过";
                    break;
                case 1:
                    sName = "审核通过";
                    break;
            }

            return sName;
        }
    }

}
