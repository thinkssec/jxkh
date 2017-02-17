using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// 文件名:  IKhJgbmkhdfData.cs
    /// 功能描述: 数据层-机关部门考核得分表数据访问接口
    /// 创建人：代码生成器
    /// 创建时间：2014/11/28 16:45:02
    /// </summary>
    public interface IKhJgbmkhdfData : IDataPerfo<KhJgbmkhdfModel>
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
