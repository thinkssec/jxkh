using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khx;

namespace Enterprise.Data.Perfo.Khx
{	

    /// <summary>
    /// 文件名:  IKhxMbData.cs
    /// 功能描述: 数据层-考核模板数据访问接口
	/// 创建人：代码生成器
	/// 创建时间：2015/11/5 20:46:51
    /// </summary>
    public interface IKhxMbData : IDataPerfo<KhxMbModel>
    {
        #region 代码生成器
        
        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhxMbModel> GetListBySQL(string sql);

        #endregion
    }

}
