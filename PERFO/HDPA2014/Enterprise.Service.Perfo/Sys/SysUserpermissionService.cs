using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// 文件名:  SysUserpermissionService.cs
    /// 功能描述: 业务逻辑层-数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/1 9:32:19
    /// </summary>
    public class SysUserpermissionService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly ISysUserpermissionData dal = new SysUserpermissionData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysUserpermissionModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysUserpermissionModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysUserpermissionModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysUserpermissionModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysUserpermissionModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 删除指定用户账号的所有权限
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool DeleteUserPmByLoginId(string loginId)
        {
            string sql = "delete from [PERFO_SYS_USERPERMISSION] where LOGINID='" + loginId + "';";
            return dal.ExecuteSQL(sql);
        }

        #endregion
    }

}
