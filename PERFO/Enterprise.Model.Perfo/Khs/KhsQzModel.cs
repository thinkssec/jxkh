using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 
	/// ������:����������
	/// ����ʱ��:	2015/11/7 19:54:34
    /// </summary>
    [Serializable]
    public class KhsQzModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///
			/// </summary>
			public virtual string QZID
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHDZBID
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///Ȩ
			/// </summary>
			public virtual decimal? QZ
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual int? KHID
			{
				get;
				set;
			}

        #endregion
    }

}
