using Enterprise.Model.Perfo.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ������λ���˵÷ֱ�
    /// ������:����������
    /// ����ʱ��:2014/12/2 13:41:05
    /// </summary>
    [Serializable]
    public class KhEjdwkhdfModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///�÷ֱ�ID
        /// </summary>
        public virtual string DFID
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
        /// ��������
        /// �ϲ��Ʒ�ʱ��ʾ�ϲ���������
        /// </summary>
        public virtual string JGMC
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
        ///����������
        /// </summary>
        public virtual string KHXMC
        {
            get;
            set;
        }

        /// <summary>
        ///���˱�׼��
        ///�ϲ��Ʒ��쵼����ʱ����Ӽ���ֵ
        /// </summary>
        public virtual decimal? KHBZF
        {
            get;
            set;
        }

        /// <summary>
        ///���˵÷�
        ///�ϲ��Ʒ��쵼����ʱ�����ۺϵ÷�
        /// </summary>
        public virtual decimal? KHDF
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string KHLX
        {
            get;
            set;
        }

        /// <summary>
        ///ͳ��ʱ��
        /// </summary>
        public virtual DateTime? TJSJ
        {
            get;
            set;
        }

        /// <summary>
        ///������
        /// </summary>
        public virtual string CZR
        {
            get;
            set;
        }

        /// <summary>
        ///���������
        /// </summary>
        public virtual string XMXH
        {
            get;
            set;
        }

        /// <summary>
        ///���˵�λ����
        /// </summary>
        public virtual int? KHDWSL
        {
            get;
            set;
        }

        /// <summary>
        ///��λ�ܵ÷�
        /// </summary>
        public virtual decimal? DWZDF
        {
            get;
            set;
        }

        /// <summary>
        ///��λƽ����
        ///������ ���ϲ��Ʒֵ�λ���ӷֺϼ�
        /// </summary>
        public virtual decimal? DWPJF
        {
            get;
            set;
        }

        /// <summary>
        ///��λ��������
        /// </summary>
        public virtual int? DWPM
        {
            get;
            set;
        }

        /// <summary>
        ///��λ���ֱ���
        ///������ ���ϲ��Ʒֵ�λ�����ֺϼ�
        /// </summary>
        public virtual decimal? DWDXBS
        {
            get;
            set;
        }

        /// <summary>
        ///��ע˵��
        /// </summary>
        public virtual string BZSM
        {
            get;
            set;
        }

        /// <summary>
        ///�������ܵ÷�
        /// </summary>
        public virtual decimal? FZRZDF
        {
            get;
            set;
        }

        /// <summary>
        ///������ƽ����
        /// </summary>
        public virtual decimal? FZRPJF
        {
            get;
            set;
        }

        /// <summary>
        ///���ӿ�������
        /// </summary>
        public virtual int? FZRPM
        {
            get;
            set;
        }

        /// <summary>
        ///���ӵ÷����
        /// </summary>
        public virtual string FZRDFLB
        {
            get;
            set;
        }

        /// <summary>
        ///���ֱ���
        /// </summary>
        public virtual decimal? FZRDXBS
        {
            get;
            set;
        }

        /// <summary>
        ///�Ѷ�ϵ��
        /// </summary>
        public virtual decimal? NDXS
        {
            get;
            set;
        }

        /// <summary>
        ///���ܱ�־
        /// </summary>
        public virtual string HZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///�ϲ��Ʒֹ���
        /// </summary>
        public virtual string HBJFID
        {
            get;
            set;
        }

        /// <summary>
        ///�ϲ��Ʒ���
        /// </summary>
        public virtual string ISHBJF
        {
            get;
            set;
        }

        /// <summary>
        /// ������λ����
        /// </summary>
        public virtual string GSDWMC
        {
            get;
            set;
        }

        #endregion

        #region ������

        /// <summary>
        /// ���Ż���ʵ��
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// �ϲ��Ʒֹ���ʵ��
        /// </summary>
        public virtual KhHbjfgzModel Hbjf { get; set; }

        #endregion
    }

}
