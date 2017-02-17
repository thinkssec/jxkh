using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���ݽ���
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhUnlockModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///��ID
        /// </summary>
        public virtual string SID
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
        ///������
        /// </summary>
        public virtual string CZZ
        {
            get;
            set;
        }

        /// <summary>
        ///���������� 1=���� 0=ָ���û�
        /// </summary>
        public virtual string CZZLX
        {
            get;
            set;
        }

        /// <summary>
        ///�ύʱ��
        /// </summary>
        public virtual DateTime? TJSJ
        {
            get;
            set;
        }

        /// <summary>
        ///������־
        /// </summary>
        public virtual string SDBZ
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

        #region ����������

        /// <summary>
        /// ���Ż���ʵ��
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        #endregion
    }

}
