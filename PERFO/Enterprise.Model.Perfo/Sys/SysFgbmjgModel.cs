using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// �û��ֹܻ���
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class SysFgbmjgModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///��������
			/// </summary>
			public virtual int JGBM
			{
				get;
				set;
			}

			/// <summary>
			///��¼ID
			/// </summary>
			public virtual string LOGINID
			{
				get;
				set;
			}

        #endregion
    }

}
