using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 
	/// ������:����������
	/// ����ʱ��:	2015/11/5 19:18:56
    /// </summary>
    [Serializable]
    public class KhsKhbzModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///
			/// </summary>
			public virtual string ID
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
			public virtual string MC
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual int? STATUS
			{
				get;
				set;
			}

			/// <summary>
			///������
			/// </summary>
			public virtual int? FZBM
			{
				get;
				set;
			}

        #endregion
    }

}
