using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// ���˴�ָ��
	/// ������:����������
	/// ����ʱ��:	2015/11/10 20:02:20
    /// </summary>
    [Serializable]
    public class KhsDzbModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///��ָ��ID
			/// </summary>
			public virtual string DZBID
			{
				get;
				set;
			}

			/// <summary>
			///ָ������
			/// </summary>
			public virtual string ZBMC
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

			/// <summary>
			///
			/// </summary>
			public virtual string KHXM
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHNR
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHFS
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHBZ
			{
				get;
				set;
			}

			/// <summary>
			///��������
			/// </summary>
			public virtual string KHZQ
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual int? PX
			{
				get;
				set;
			}

        #endregion
    }

}
