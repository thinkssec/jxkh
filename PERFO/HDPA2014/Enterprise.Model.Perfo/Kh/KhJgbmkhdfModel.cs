using Enterprise.Model.Perfo.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ���ز��ſ��˵÷ֱ�
    /// ������:����������
    /// ����ʱ��:2014/11/28 16:45:02
    /// </summary>
    [Serializable]
    public class KhJgbmkhdfModel : PerfoSuperModel
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
        public virtual int JGBM
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
        ///���˵÷�
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
        ///�����ܵ÷�
        /// </summary>
        public virtual decimal? BMZDF
        {
            get;
            set;
        }

        /// <summary>
        ///����ƽ����
        /// </summary>
        public virtual decimal? BMPJF
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual int? BMPM
        {
            get;
            set;
        }

        /// <summary>
        ///���Ŷ��ֱ���
        /// </summary>
        public virtual decimal? BMDXBS
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
        ///�����˶��ֱ���
        /// </summary>
        public virtual decimal? FZRDXBS
        {
            get;
            set;
        }

        /// <summary>
        ///�����˿�������
        /// </summary>
        public virtual int? FZRPM
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

        #endregion

        #region �Զ��������

        /// <summary>
        /// ���Ż�������
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        #endregion
    }

}
