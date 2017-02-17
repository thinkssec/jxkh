using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ����ָ����ܱ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:53
    /// </summary>
    [Serializable]
    public class KhZbsxModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///ɸѡ��ID
        /// </summary>
        public virtual string SXID
        {
            get;
            set;
        }

        /// <summary>
        ///������ID
        /// </summary>
        public virtual string ZRSZBID
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
        ///ָ�����
        /// </summary>
        public virtual string SXZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///ָ��Ȩ��
        /// </summary>
        public virtual decimal? SXQZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ���ֵ
        /// </summary>
        public virtual decimal? SXFZ
        {
            get;
            set;
        }

        /// <summary>
        ///ɸѡ��ʾ���
        /// </summary>
        public virtual int? SXXH
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual int SXJGBM
        {
            get;
            set;
        }

        /// <summary>
        /// ��ʱͣ��----��׼��ֵ
        /// ���ز��ſ���
        /// </summary>
        public virtual decimal? SXJZFZ
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// ������
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        /// <summary>
        /// ��Ч������ָ��
        /// </summary>
        public virtual KhJxzrszbModel JxzrsZb { get; set; }

        #endregion

        /// <summary>
        ///��������
        /// </summary>
        public virtual string JGBM
        {
            get
            {
                return SXJGBM.ToString();
            }
        }

    }

}
