using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ����ͳ�Ʊ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhTjbModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///����ID
			/// </summary>
			public virtual int KHID
			{
				get;
				set;
			}

			/// <summary>
			///δ�
			/// </summary>
			public virtual int? WTB
			{
				get;
				set;
			}

			/// <summary>
			///δ�ύ
			/// </summary>
			public virtual int? WTJ
			{
				get;
				set;
			}

			/// <summary>
			///�����
			/// </summary>
			public virtual int? SJTB
			{
				get;
				set;
			}

			/// <summary>
			///�ļ��ύ
			/// </summary>
			public virtual int? WJTJ
			{
				get;
				set;
			}

			/// <summary>
			///�����
			/// </summary>
			public virtual int? YSH
			{
				get;
				set;
			}

			/// <summary>
			///����
			/// </summary>
			public virtual int? YSD
			{
				get;
				set;
			}

			/// <summary>
			///���˽���
			/// </summary>
			public virtual int? KHJD
			{
				get;
				set;
			}

			/// <summary>
			///ͳ������
			/// </summary>
			public virtual DateTime? TJRQ
			{
				get;
				set;
			}

			/// <summary>
			///����1
			/// </summary>
			public virtual string BY1
			{
				get;
				set;
			}

			/// <summary>
			///����2
			/// </summary>
			public virtual string BY2
			{
				get;
				set;
			}

			/// <summary>
			///����3
			/// </summary>
			public virtual string BY3
			{
				get;
				set;
			}

			/// <summary>
			///����4
			/// </summary>
			public virtual string BY4
			{
				get;
				set;
			}

        #endregion
    }

}
