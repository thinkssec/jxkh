using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// Ŀ��ֵ�
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkMbztbModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///����ָ�����
			/// </summary>
			public virtual string LHZBBM
			{
				get;
				set;
			}

			/// <summary>
			///��������
			/// </summary>
			public virtual int JGBM
			{
				get;
				set;
			}

        #endregion
    }

}
