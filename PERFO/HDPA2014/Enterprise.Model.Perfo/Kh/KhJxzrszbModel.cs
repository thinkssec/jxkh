using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ��ȼ�Ч������ָ��
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhJxzrszbModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///������ָ��ID
        /// </summary>
        public virtual string ZRSZBID
        {
            get;
            set;
        }

        /// <summary>
        ///����ָ�����
        /// </summary>
        public virtual string LHZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///���ָ�����
        /// </summary>
        public virtual string DFZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///������ID
        /// </summary>
        public virtual int? ZRSID
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual int? ZSZND
        {
            get;
            set;
        }

        /// <summary>
        ///ָ��Ȩ��
        /// </summary>
        public virtual decimal? ZZBQZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ���ֵ
        /// </summary>
        public virtual decimal? ZZBFZ
        {
            get;
            set;
        }

        /// <summary>
        ///��ʾ���
        /// </summary>
        public virtual int? ZXSXH
        {
            get;
            set;
        }

        /// <summary>
        ///���Ŀ��ֵ
        /// </summary>
        public virtual decimal? ZNCMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///����Ŀ��ֵ
        /// </summary>
        public virtual decimal? ZNZMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///��ĩĿ��ֵ
        /// </summary>
        public virtual decimal? ZNMMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///Ŀ��ֵ
        /// </summary>
        public virtual decimal? ZMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ�����
        /// </summary>
        public virtual string ZZBDH
        {
            get;
            set;
        }

        /// <summary>
        ///ָ������
        /// </summary>
        public virtual string ZZBXZ
        {
            get;
            set;
        }

        /// <summary>
        ///�����ϵʽ
        /// </summary>
        public virtual string ZJSGXS
        {
            get;
            set;
        }

        /// <summary>
        ///�ϼ�ָ��
        /// </summary>
        public virtual string ZSJZB
        {
            get;
            set;
        }

        /// <summary>
        ///�ύ����
        /// </summary>
        public virtual DateTime? ZTJRQ
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual int? ZJGBM
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
        /// Ŀ��ֵ��ע
        /// </summary>
        public virtual string MBZBZ
        {
            get;
            set;
        }

        /// <summary>
        /// ���ز��ſ�����Ҫ����
        /// </summary>
        public virtual string JGKHNR
        {
            get;
            set;
        }

        /// <summary>
        /// ���ز��ſ���Ŀ��
        /// </summary>
        public virtual string JGKHMB
        {
            get;
            set;
        }

        /// <summary>
        /// ���ز������ʱ��˵��
        /// </summary>
        public virtual string JGWCSJ
        {
            get;
            set;
        }

        /// <summary>
        /// ���ز��ſ������ֱ�׼
        /// </summary>
        public virtual string JGPFBZ
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// ��֯����
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// ��Ч������
        /// </summary>
        public virtual KhJxzrsModel Jxzrs { get; set; }

        /// <summary>
        /// ����ָ��
        /// </summary>
        public virtual ZbkLhzbModel Lhzb { get; set; }

        /// <summary>
        /// ���ָ��
        /// </summary>
        public virtual ZbkDfzbModel Dfzb { get; set; }

        #endregion
    }

}
