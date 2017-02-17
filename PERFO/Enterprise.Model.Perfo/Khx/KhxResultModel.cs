using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khx
{
    /// <summary>
    /// ���˵÷�
	/// ������:����������
	/// ����ʱ��:	2015/11/12 19:37:11
    /// </summary>
    [Serializable]
    public class KhxResultModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///�÷ֶ�Ӧָ�����ID
			/// </summary>
			public virtual string FORID
			{
				get;
				set;
			}

			/// <summary>
			///�÷�
			/// </summary>
			public virtual int? SCORE
			{
				get;
				set;
			}

			/// <summary>
			///��λ��ģ���ϵ��ID
			/// </summary>
			public virtual string MBJGID
			{
				get;
				set;
			}

			/// <summary>
			///�Ӽ�������
			/// </summary>
			public virtual string MS
			{
				get;
				set;
			}

			/// <summary>
			///�������̱�ID
			/// </summary>
			public virtual string KHID
			{
				get;
				set;
			}

			/// <summary>
			///�������,xm1,xm2,nr
			/// </summary>
			public virtual string TYPE
			{
				get;
				set;
			}

        #endregion
    }

}
