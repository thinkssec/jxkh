using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// ����ָ�������������ݶ�Ӧ��
    /// ������:����������
    /// ����ʱ��:2014/12/14 9:19:20
    /// </summary>
    [Serializable]
    public class ZbkCwjcsjglzbModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///��Ӧ��ID
			/// </summary>
			public virtual string ID
			{
				get;
				set;
			}

			/// <summary>
			///ָ��������
			/// </summary>
			public virtual string ZBXMC
			{
				get;
				set;
			}

			/// <summary>
			///�����������ָ��
			/// </summary>
			public virtual string JCSJZB
			{
				get;
				set;
			}

			/// <summary>
			///��������
			/// </summary>
			public virtual string JCSJLX
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

        #endregion
    }

}
