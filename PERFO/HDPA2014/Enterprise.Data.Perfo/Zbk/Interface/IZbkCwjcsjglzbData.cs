using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Data.Perfo.Zbk
{	

    /// <summary>
    /// 文件名:  IZbkCwjcsjglzbData.cs
    /// 功能描述: 数据层-关联指标与财务基础数据对应表数据访问接口
    /// 创建人：代码生成器
    /// 创建时间：2014/12/14 9:19:20
    /// </summary>
    public interface IZbkCwjcsjglzbData : IDataPerfo<ZbkCwjcsjglzbModel>
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
