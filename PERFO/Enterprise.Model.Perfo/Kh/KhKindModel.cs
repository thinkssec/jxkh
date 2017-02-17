using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// �������ͱ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhKindModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///��������ID
			/// </summary>
			public virtual string LXID
			{
				get;
				set;
			}

			/// <summary>
			///��������
			/// </summary>
			public virtual string LXMC
			{
				get;
				set;
			}

			/// <summary>
			///��Чʱ��
			/// </summary>
			public virtual DateTime? SXSJ
			{
				get;
				set;
			}

			/// <summary>
			///��ǰ״̬
			/// </summary>
			public virtual string DQZT
			{
				get;
				set;
			}

        #endregion
    }

}
