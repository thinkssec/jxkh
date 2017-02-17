using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Data.Perfo.Zbk
{	

    /// <summary>
    /// 文件名:  IZbkDfzData.cs
    /// 功能描述: 数据层-打分者数据访问接口
    /// 创建人：代码生成器
    /// 创建时间：2014/11/1 0:35:46
    /// </summary>
    public interface IZbkDfzData : IDataPerfo<ZbkDfzModel>
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
