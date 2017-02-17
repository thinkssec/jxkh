using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ������
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhMessageModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///����ID
        /// </summary>
        public virtual string MSGID
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
        ///��¼ID
        /// </summary>
        public virtual string LOGINID
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string DBMC
        {
            get;
            set;
        }

        /// <summary>
        ///����˵��
        /// </summary>
        public virtual string DBSM
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string DBLJ
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual DateTime? JSRQ
        {
            get;
            set;
        }

        /// <summary>
        ///��ǰ״̬
        /// </summary>
        public virtual string DQZT
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual DateTime? WCRQ
        {
            get;
            set;
        }

        /// <summary>
        /// ģ��ID
        /// </summary>
        public virtual string MID
        {
            get;
            set;
        }

        /// <summary>
        /// ������
        /// </summary>
        public virtual string FSR
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// �����ڶ���
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        /// <summary>
        /// �û�MODEL
        /// </summary>
        public virtual SysUserModel User { get; set; }

        #endregion
    }

}