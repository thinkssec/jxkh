using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核得分排序范围设置
    /// 创建人:代码生成器
    /// 创建时间:2014/12/2 13:41:05
    /// </summary>
    [Serializable]
    public class KhKhdfpxfwModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///ID
			/// </summary>
			public virtual string ID
			{
				get;
				set;
			}

			/// <summary>
			///考核ID
			/// </summary>
			public virtual int KHID
			{
				get;
				set;
			}

			/// <summary>
			///归属单位
			/// </summary>
			public virtual string GSDW
			{
				get;
				set;
			}

			/// <summary>
			///考核对象
			/// </summary>
			public virtual string KHDX
			{
				get;
				set;
			}

        #endregion
    }

}
