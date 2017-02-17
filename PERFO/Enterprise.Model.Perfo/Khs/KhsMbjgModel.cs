using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// ģ�����
	/// ������:����������
	/// ����ʱ��:	2015/11/12 18:44:38
    /// </summary>
    [Serializable]
    public class KhsMbjgModel : PerfoSuperModel
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
			///ģ��id
			/// </summary>
			public virtual string MBID
			{
				get;
				set;
			}

			/// <summary>
			///����
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual decimal? PX
			{
				get;
				set;
			}

			/// <summary>
			///�����ļ�����
			/// </summary>
			public virtual string FNAMES
			{
				get;
				set;
			}

			/// <summary>
			///������������
			/// </summary>
			public virtual string FVIEWNAMES
			{
				get;
				set;
			}

			/// <summary>
			///����״̬��0�������У�1�����ύ��2��������
			/// </summary>
			public virtual string STATUS
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KEY1
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KEY2
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KEY3
			{
				get;
				set;
			}

        #endregion
    }

}
