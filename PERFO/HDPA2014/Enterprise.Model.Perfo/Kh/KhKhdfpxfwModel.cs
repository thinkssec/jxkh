using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���˵÷�����Χ����
    /// ������:����������
    /// ����ʱ��:2014/12/2 13:41:05
    /// </summary>
    [Serializable]
    public class KhKhdfpxfwModel : PerfoSuperModel
    {
        #region ����������
        
			/// <summary>
			///ID
			/// </summary>
			public virtual string ID
			{
				get;
				set;
			}

			/// <summary>
			///����ID
			/// </summary>
			public virtual int KHID
			{
				get;
				set;
			}

			/// <summary>
			///������λ
			/// </summary>
			public virtual string GSDW
			{
				get;
				set;
			}

			/// <summary>
			///���˶���
			/// </summary>
			public virtual string KHDX
			{
				get;
				set;
			}

        #endregion
    }

}
