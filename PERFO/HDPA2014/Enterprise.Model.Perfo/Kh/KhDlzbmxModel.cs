using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ����ָ�꿼�˱�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhDlzbmxModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///�������˱�ID
        /// </summary>
        public virtual string ID
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
        ///���Ŀ��ֵ
        /// </summary>
        public virtual decimal? NCMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///����Ŀ������ֵ
        /// </summary>
        public virtual decimal? MBZSQZ
        {
            get;
            set;
        }

        /// <summary>
        ///Ŀ��ֵ����˵��
        /// </summary>
        public virtual string MBZSQBZ
        {
            get;
            set;
        }

        /// <summary>
        ///����Ŀ��ֵ
        /// </summary>
        public virtual decimal? MBZ
        {
            get;
            set;
        }

        /// <summary>
        ///Ŀ��ֵ˵��
        /// </summary>
        public virtual string MBZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///Ŀ��ֵȷ����
        /// </summary>
        public virtual string MBZQRR
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ����ֵ
        /// </summary>
        public virtual decimal? WCZSQZ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ������
        /// </summary>
        public virtual string WCZSQR
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ����˵��
        /// </summary>
        public virtual string WCZSQBZ
        {
            get;
            set;
        }

        /// <summary>
        ///�������ֵ
        /// </summary>
        public virtual decimal? WCZ
        {
            get;
            set;
        }

        /// <summary>
        /// ���ֵ˵��
        /// </summary>
        public virtual string WCZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ���ֵ
        /// </summary>
        public virtual decimal? WCZSHZ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ�����
        /// </summary>
        public virtual string WCZSHR
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ���˵��
        /// </summary>
        public virtual string WCZSHBZ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ��ֵ
        /// </summary>
        public virtual decimal? WCZSDZ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ��˵��
        /// </summary>
        public virtual string WCZSDBZ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ����
        /// </summary>
        public virtual string WCZSDR
        {
            get;
            set;
        }

        /// <summary>
        ///ָ�꿼��״̬ 
        ///0=δ�ύ 1=���ύ|δ��� 2=�����|δ�� 3=�����
        /// </summary>
        public virtual string ZBKHZT
        {
            get;
            set;
        }

        /// <summary>
        ///Ŀ��ֵȷ������
        /// </summary>
        public virtual DateTime? MBZQRRQ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ��������
        /// </summary>
        public virtual DateTime? WCZSQRQ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ�������
        /// </summary>
        public virtual DateTime? WCZSHRQ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ������
        /// </summary>
        public virtual DateTime? WCZSDRQ
        {
            get;
            set;
        }

        /// <summary>
        ///�ٷ�����ֵ
        /// </summary>
        public virtual decimal? BFSFZ
        {
            get;
            set;
        }

        /// <summary>
        ///ʵ�ʵ÷�
        /// </summary>
        public virtual decimal? SJDF
        {
            get;
            set;
        }

        /// <summary>
        ///���˶��� 
        ///������λ = 1,�쵼���� = 2,���ز��� = 3,���Ÿ����� = 4
        /// </summary>
        public virtual string KHDX
        {
            get;
            set;
        }

        /// <summary>
        /// �÷ּ����������
        /// </summary>
        public virtual string DFJSQK
        {
            get;
            set;
        }

        /// <summary>
        /// �Ƿ����¼���--��־
        /// </summary>
        public virtual bool IsCalculate
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
        /// ����ָ��MODEL
        /// </summary>
        public virtual ZbkLhzbModel LhzbModel { get; set; }

        /// <summary>
        /// ����˴�ֽ������
        /// </summary>
        public virtual IList<KhJgbmdfbModel> KhJgbmdfbLst { get; set; }

        /// <summary>
        /// ����ָ��������ݱ�
        /// </summary>
        public virtual IList<KhLhzbjcsjModel> LhzbjcsjLst { get; set; }

        #endregion
    }

}
