using Enterprise.Model.Perfo.Zbk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// �������˻������ݱ�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhLhzbjcsjModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///����ָ��ID
        /// </summary>
        public virtual string JCZBID
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
        ///ָ�����
        /// </summary>
        public virtual string ZBDH
        {
            get;
            set;
        }

        /// <summary>
        ///ָ������
        /// </summary>
        public virtual string ZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///ָ���ϱ�ֵ
        /// </summary>
        public virtual decimal? ZBSBZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ�����ֵ
        /// </summary>
        public virtual decimal? ZBSHZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ����ֵ
        /// </summary>
        public virtual decimal? ZBSDZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ��ֵ
        /// </summary>
        public virtual decimal? ZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ���ϱ�˵��
        /// </summary>
        public virtual string ZBSBBZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ�����˵��
        /// </summary>
        public virtual string ZBSHBZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ����˵��
        /// </summary>
        public virtual string ZBSDBZ
        {
            get;
            set;
        }

        /// <summary>
        ///˳���
        /// </summary>
        public virtual int? XH
        {
            get;
            set;
        }

        #endregion

        #region ����������

        ///// <summary>
        ///// ����ָ��MODEL
        ///// </summary>
        //public virtual ZbkLhzbModel LhzbModel { get; set; }

        #endregion
    }

}