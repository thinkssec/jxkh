using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// 文件名:  IKhXsData.cs
    /// 功能描述: 数据层-系数数据访问接口
	/// 创建人：代码生成器
	/// 创建时间：2015/10/27 14:17:54
    /// </summary>
    public interface IKhXsData : IDataPerfo<KhXsModel>
    {
        #region 代码生成器
        
        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhXsModel> GetListBySQL(string sql);

        #endregion
    }

}
