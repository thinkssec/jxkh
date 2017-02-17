using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// 文件名:  IKhsJgmydData.cs
    /// 功能描述: 数据层-机关满意度数据访问接口
	/// 创建人：代码生成器
	/// 创建时间：2015/11/8 22:00:44
    /// </summary>
    public interface IKhsJgmydData : IDataPerfo<KhsJgmydModel>
    {
        #region 代码生成器
        
        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsJgmydModel> GetListBySQL(string sql);

        #endregion
    }

}
