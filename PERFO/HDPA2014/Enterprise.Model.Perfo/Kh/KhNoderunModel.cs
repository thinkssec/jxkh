using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���˽ڵ����б�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhNoderunModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///���б�ID
        /// </summary>
        public virtual string YXID
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
        ///�ڵ�ID
        /// </summary>
        public virtual int? JDID
        {
            get;
            set;
        }

        /// <summary>
        ///�ڵ�����״̬
        /// </summary>
        public virtual string YXZT
        {
            get;
            set;
        }

        /// <summary>
        ///�ڵ㿪ʼʱ��
        /// </summary>
        public virtual DateTime? YXKSSJ
        {
            get;
            set;
        }

        /// <summary>
        ///�ڵ����ʱ��
        /// </summary>
        public virtual DateTime? YXWCSJ
        {
            get;
            set;
        }

        /// <summary>
        ///�ڵ����ֵ
        /// </summary>
        public virtual int? YXJDZ
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// �ڵ����ʵ��
        /// </summary>
        public virtual KhNodesModel JdNode { get; set; }

        #endregion

    }

}
