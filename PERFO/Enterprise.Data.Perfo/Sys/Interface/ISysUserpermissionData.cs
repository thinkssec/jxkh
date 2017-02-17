using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Data.Perfo.Sys
{	

    /// <summary>
    /// 文件名:  ISysUserpermissionData.cs
    /// 功能描述: 数据层-数据访问接口
    /// 创建人：代码生成器
    /// 创建时间：2014/12/1 9:32:19
    /// </summary>
    public interface ISysUserpermissionData : IDataPerfo<SysUserpermissionModel>
    {
        #region 代码生成器

        /// <summary>
        /// 执行基于SQL的原生操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool ExecuteSQL(string sql);
        
        #endregion
    }

}
