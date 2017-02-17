using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���˽ڵ��
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhNodesModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///�ڵ�ID
        /// </summary>
        public virtual int JDID
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
        ///�ڵ�����
        /// </summary>
        public virtual string JDMC
        {
            get;
            set;
        }

        /// <summary>
        ///�ڵ������
        /// </summary>
        public virtual string JDGLB
        {
            get;
            set;
        }

        /// <summary>
        ///�ڵ�˳���
        /// </summary>
        public virtual string JDXH
        {
            get;
            set;
        }

        /// <summary>
        ///�ڵ����ֵ
        /// </summary>
        public virtual int? JDZ
        {
            get;
            set;
        }

        /// <summary>
        ///��Ч��־
        /// </summary>
        public virtual string YXBZ
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual DateTime? TJRQ
        {
            get;
            set;
        }

        #endregion
    }

}
