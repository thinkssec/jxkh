using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���˹���
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class KhKhglModel : PerfoSuperModel
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
        public virtual string KHMC
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual string KHND
        {
            get;
            set;
        }

        /// <summary>
        /// �汾����
        /// </summary>
        public virtual string BBMC
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string KHZQ
        {
            get;
            set;
        }

        /// <summary>
        ///��ʼʱ��
        /// </summary>
        public virtual DateTime? KSSJ
        {
            get;
            set;
        }

        /// <summary>
        ///�ر�ʱ��
        /// </summary>
        public virtual DateTime? GBSJ
        {
            get;
            set;
        }

        /// <summary>
        ///����״̬
        /// </summary>
        public virtual string KHZT
        {
            get;
            set;
        }

        /// <summary>
        ///���˷�����
        /// </summary>
        public virtual string FQR
        {
            get;
            set;
        }

        /// <summary>
        ///�Ƿ�ɲ�
        /// </summary>
        public virtual string SFKC
        {
            get;
            set;
        }

        /// <summary>
        ///�����¶�
        /// </summary>
        public virtual int? KHYD
        {
            get;
            set;
        }

        /// <summary>
        ///���˼���
        /// </summary>
        public virtual int? KHJD
        {
            get;
            set;
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��������ʵ��
        /// </summary>
        public virtual KhKindModel Kind { get; set; }

        #endregion
    }

}
