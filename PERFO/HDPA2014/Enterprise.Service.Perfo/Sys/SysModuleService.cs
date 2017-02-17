using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.MVC;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// 文件名:  SysModuleService.cs
    /// 功能描述: 业务逻辑层-功能模块表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class SysModuleService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly ISysModuleData dal = new SysModuleData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysModuleModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysModuleModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysModuleModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysModuleModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysModuleModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 根据编码前缀获取其数量
        /// 用于添加操作
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int GetModuleAmount(string parentId)
        {
            string hql =
                "from SysModuleModel p where p.MPARENTID='" + parentId + "'";
            var q = dal.GetListByHQL(hql);
            int amount = 0;
            if (q != null)
            {
                amount = q.Count;
            }
            return amount;
        }

        #endregion

        #region 路由配置相关

        /// <summary>
        /// 加载所有模块的路由配置信息
        /// </summary>
        /// <returns></returns>
        public static List<UrlMapPageRoute> LoadUrlRoute()
        {
            List<UrlMapPageRoute> routeList = new List<UrlMapPageRoute>();
            var moduleList = dal.GetList();
            string urlPrefix = "~/";
            foreach (var m in moduleList)
            {
                if (!string.IsNullOrEmpty(m.WEBURL.Trim()) && !string.IsNullOrEmpty(m.MURL.Trim()))
                {
                    UrlMapPageRoute route = new UrlMapPageRoute(
                        m.MID, m.WEBURL, urlPrefix + m.MURL.TrimStart(urlPrefix.ToCharArray()));
                    routeList.Add(route);
                }
            }
            return routeList;
        }

        #endregion

    }

}
