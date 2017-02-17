using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// �����
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkDfzModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///���ָ�����
			/// </summary>
			public virtual string DFZBBM
			{
				get;
				set;
			}

			/// <summary>
			///������
			/// </summary>
			public virtual string OPERATOR
			{
				get;
				set;
			}

			/// <summary>
			///���������� 1=���� 0=ָ���û�
			/// </summary>
			public virtual string OPERTYPE
			{
				get;
				set;
			}

        #endregion
    }

}
