using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核结果文件
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:53
    /// </summary>
    [Serializable]
    public class KhZzjgfileModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///考核ID
			/// </summary>
			public virtual int KHID
			{
				get;
				set;
			}

			/// <summary>
			///结果文件名称
			/// </summary>
			public virtual string ZZJGMC
			{
				get;
				set;
			}

			/// <summary>
			///结果文件附件
			/// </summary>
			public virtual string ZZJGFILE
			{
				get;
				set;
			}

        #endregion
    }

}
