using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���ָ�꿼�˱�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhDfzbmxModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///���ָ��ID
        /// </summary>
        public virtual string DFZBID
        {
            get;
            set;
        }

        /// <summary>
        ///ɸѡ��ID
        /// </summary>
        public virtual string SXID
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
        ///���ֵ
        /// </summary>
        public virtual decimal? DFSZ
        {
            get;
            set;
        }

        /// <summary>
        ///���˵��
        /// </summary>
        public virtual string DFBZ
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual DateTime? DFRQ
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
        ///�÷�
        /// </summary>
        public virtual decimal? SJDF
        {
            get;
            set;
        }

        /// <summary>
        ///Լ��˵��
        /// </summary>
        public virtual string YSSM
        {
            get;
            set;
        }

        /// <summary>
        ///���˶���
        ///������λ = 1,
        ///�쵼���� = 2,
        ///���ز��� = 3,
        ///���Ÿ����� = 4
        /// </summary>
        public virtual string KHDX
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// ָ��ɸѡMODEL
        /// </summary>
        public virtual KhZbsxModel ZbsxModel { get; set; }

        /// <summary>
        /// ���ָ��MODEL
        /// </summary>
        public virtual ZbkDfzbModel DfzbModel { get; set; }

        /// <summary>
        /// ���Ż���
        /// </summary>
        public virtual SysBmjgModel Danwei { get; set; }

        /// <summary>
        /// ����˴�ֽ������
        /// </summary>
        public virtual IList<KhJgbmdfbModel> KhJgbmdfbLst { get; set; }

        #endregion
    }

}
