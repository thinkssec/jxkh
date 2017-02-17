using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���˽���ļ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:53
    /// </summary>
    [Serializable]
    public class KhZzjgfileModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///����ID
			/// </summary>
			public virtual int KHID
			{
				get;
				set;
			}

			/// <summary>
			///����ļ�����
			/// </summary>
			public virtual string ZZJGMC
			{
				get;
				set;
			}

			/// <summary>
			///����ļ�����
			/// </summary>
			public virtual string ZZJGFILE
			{
				get;
				set;
			}

        #endregion
    }

}
