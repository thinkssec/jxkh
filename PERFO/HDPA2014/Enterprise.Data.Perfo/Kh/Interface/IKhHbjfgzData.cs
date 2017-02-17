using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// 文件名:  IKhHbjfgzData.cs
    /// 功能描述: 数据层-合并计分规则表数据访问接口
    /// 创建人：代码生成器
    /// 创建时间：2014/12/2 13:41:05
    /// </summary>
    public interface IKhHbjfgzData : IDataPerfo<KhHbjfgzModel>
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
