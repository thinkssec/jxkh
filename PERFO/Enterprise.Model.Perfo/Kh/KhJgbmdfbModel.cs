using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���ز��Ŵ�ֱ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhJgbmdfbModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///��ֱ�ID
        /// </summary>
        public virtual string DFBID
        {
            get;
            set;
        }

        /// <summary>
        ///�������˱�ID
        /// </summary>
        public virtual string ID
        {
            get;
            set;
        }

        /// <summary>
        ///���ָ��ID
        /// </summary>
        public virtual string DFZBID
        {
            get;
            set;
        }

        /// <summary>
        ///ָ�����
        /// </summary>
        public virtual string ZBBM
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
        ///����ID
        /// </summary>
        public virtual int? KHID
        {
            get;
            set;
        }

        /// <summary>
        ///��ɽ���
        /// </summary>
        public virtual int? WCJD
        {
            get;
            set;
        }

        /// <summary>
        ///���˵÷�
        /// </summary>
        public virtual decimal? KHDF
        {
            get;
            set;
        }

        /// <summary>
        ///�÷�˵��
        /// </summary>
        public virtual string DFSM
        {
            get;
            set;
        }

        /// <summary>
        ///�����
        /// </summary>
        public virtual string DFZ
        {
            get;
            set;
        }

        /// <summary>
        ///���ʱ��
        /// </summary>
        public virtual DateTime? DFSJ
        {
            get;
            set;
        }

        /// <summary>
        ///���������
        /// </summary>
        public virtual string DFZLX
        {
            get;
            set;
        }

        /// <summary>
        ///���Ȩ��
        /// </summary>
        public virtual decimal? DFQZ
        {
            get;
            set;
        }

        #endregion
    }

}
