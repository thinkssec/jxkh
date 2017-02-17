using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Data.Perfo
{

    /// <summary>
    /// 文件名:  IDataBasis.cs
    /// 功能描述: 公共基础平台数据访问接口
    /// 创建人：qw
    /// 创建日期: 2013.1.21
    /// </summary>
    public interface IDataPerfo<T>
    {

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        IList<T> GetList();

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Execute(T t);

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<T> GetListBySQL(string sql);

        /// <summary>
        /// 根据条件返回数据集合
        /// </summary>
        /// <param name="hql">HQL</param>
        /// <returns></returns>
        IList<T> GetListByHQL(string hql);

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetSingle(string key);

    }

}
