using Enterprise.Model.Perfo.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// �����ϱ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhSjsbModel : PerfoSuperModel
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
        ///��������
        /// </summary>
        public virtual int JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///�ϱ�״̬
        /// </summary>
        public virtual string SBZT
        {
            get;
            set;
        }

        /// <summary>
        ///�ϱ�ʱ��
        /// </summary>
        public virtual DateTime? SBSJ
        {
            get;
            set;
        }

        /// <summary>
        ///�ϱ���
        /// </summary>
        public virtual string SBR
        {
            get;
            set;
        }

        /// <summary>
        ///����
        /// </summary>
        public virtual string SBFJ
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
