using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khx
{
    /// <summary>
    /// ����ģ��
	/// ������:����������
	/// ����ʱ��:	2015/11/5 20:46:51
    /// </summary>
    [Serializable]
    public class KhxMbModel : PerfoSuperModel
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
			///ģ������
			/// </summary>
			public virtual string NAME
			{
				get;
				set;
			}

			/// <summary>
			///��ע
			/// </summary>
			public virtual string MEMO
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual DateTime ADDTIME
			{
				get;
				set;
			}

			/// <summary>
			///Ȩ�أ�%��
			/// </summary>
			public virtual int? QZ
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
