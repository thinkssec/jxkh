using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// �ϼ���������
	/// ������:����������
	/// ����ʱ��:	2015/11/9 10:04:44
    /// </summary>
    [Serializable]
    public class KhsSjkhModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///uid
			/// </summary>
			public virtual string ID
			{
				get;
				set;
			}

			/// <summary>
			///����ID
			/// </summary>
			public virtual int? KHID
			{
				get;
				set;
			}

			/// <summary>
			///������
			/// </summary>
			public virtual string LOGINID
			{
				get;
				set;
			}

			/// <summary>
			///��������
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///����
			/// </summary>
			public virtual decimal? PF
			{
				get;
				set;
			}

			/// <summary>
			///��ע
			/// </summary>
			public virtual string BZ
			{
				get;
				set;
			}

			/// <summary>
			///¼��ʱ��
			/// </summary>
			public virtual DateTime? LRSJ
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

        #endregion
    }

}
