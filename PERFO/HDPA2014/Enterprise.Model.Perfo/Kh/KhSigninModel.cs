using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ֪ͨǩ�ձ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhSigninModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///ǩ��ID
			/// </summary>
			public virtual int QSID
			{
				get;
				set;
			}

			/// <summary>
			///֪ͨID
			/// </summary>
			public virtual string TZID
			{
				get;
				set;
			}

			/// <summary>
			///ǩ�յ�λ
			/// </summary>
			public virtual string QSDW
			{
				get;
				set;
			}

			/// <summary>
			///ǩ����
			/// </summary>
			public virtual string QSR
			{
				get;
				set;
			}

			/// <summary>
			///ǩ������
			/// </summary>
			public virtual DateTime? QSRQ
			{
				get;
				set;
			}

        #endregion
    }

}
