using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// �����ƶȻ��
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhKindhbModel : PerfoSuperModel
    {

        #region ����������

        /// <summary>
        ///�ļ�ID
        /// </summary>
        public virtual int WJID
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
        ///�ļ�����
        /// </summary>
        public virtual string WJMC
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual string ZXLL
        {
            get;
            set;
        }

        /// <summary>
        ///�ļ�����
        /// </summary>
        public virtual string WJFJ
        {
            get;
            set;
        }

        /// <summary>
        ///�ļ�״̬
        /// </summary>
        public virtual string WJZT
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

        #region ������

        /// <summary>
        /// ��������ʵ��
        /// </summary>
        public virtual KhKindModel Kind { get; set; }

        #endregion

    }

}
