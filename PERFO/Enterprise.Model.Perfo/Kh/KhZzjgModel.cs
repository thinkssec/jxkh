using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���ս����
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:53
    /// </summary>
    [Serializable]
    public class KhZzjgModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///���ID
			/// </summary>
			public virtual int JGID
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
			///��������
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///Լ���۷�
			/// </summary>
            public virtual decimal? YSZBKF
			{
				get;
				set;
			}

			/// <summary>
			///�ӷֵ÷�
			/// </summary>
            public virtual decimal? JFZBDF
			{
				get;
				set;
			}

			/// <summary>
			///�Ӽ��ֵ÷�
			/// </summary>
            public virtual decimal? JJFDF
			{
				get;
				set;
			}

			/// <summary>
			///����ָ��÷�
			/// </summary>
			public virtual decimal? LHZBDF
			{
				get;
				set;
			}

			/// <summary>
			///ԭʼ�÷�
			/// </summary>
			public virtual decimal? YSDF
			{
				get;
				set;
			}

			/// <summary>
			///�Ѷ�ϵ��
			/// </summary>
			public virtual decimal? NDXS
			{
				get;
				set;
			}

			/// <summary>
			///���յ÷�
			/// </summary>
			public virtual decimal? ZZDF
			{
				get;
				set;
			}

			/// <summary>
			///�÷ֵȼ�
			/// </summary>
			public virtual string DFDJ
			{
				get;
				set;
			}

			/// <summary>
			///���ֱ���
			/// </summary>
			public virtual decimal? DXBS
			{
				get;
				set;
			}

			/// <summary>
			///��������
			/// </summary>
			public virtual DateTime? SCRQ
			{
				get;
				set;
			}

			/// <summary>
			///��������
			/// </summary>
			public virtual string KHLX
			{
				get;
				set;
			}

			/// <summary>
			///����˳��
			/// </summary>
			public virtual int? PMSX
			{
				get;
				set;
			}

        #endregion
    }

}
