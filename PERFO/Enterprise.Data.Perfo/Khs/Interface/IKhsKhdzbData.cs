using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// 文件名:  IKhsKhdzbData.cs
    /// 功能描述: 数据层-考核指标对照表数据访问接口
	/// 创建人：代码生成器
	/// 创建时间：2015/11/4 20:47:10
    /// </summary>
    public interface IKhsKhdzbData : IDataPerfo<KhsKhdzbModel>
    {
        #region 代码生成器
        
        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsKhdzbModel> GetListBySQL(string sql);
        bool ExecuteSQL(string sql);
        #endregion
    }

}
