using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// �������罨�迼�˻��ܱ�
    /// ������:����������
    /// ����ʱ��:2014/11/28 16:45:02
    /// </summary>
    [Serializable]
    public class KhJgzfbModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///��������ID
        /// </summary>
        public virtual string ZFID
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
        ///��������
        /// </summary>
        public virtual int JGBM
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
        ///ָ������
        /// </summary>
        public virtual string ZBMC
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
        public virtual string DFZLX
        {
            get;
            set;
        }

        /// <summary>
        ///���������
        /// </summary>
        public virtual int? DFZSL
        {
            get;
            set;
        }

        /// <summary>
        /// ���翼�˵÷�
        /// </summary>
        public virtual decimal? ZFKHDF
        {
            get;
            set;
        }

        /// <summary>
        /// �ܵ÷�
        /// </summary>
        public virtual decimal? ZDF
        {
            get;
            set;
        }

        /// <summary>
        ///����
        /// </summary>
        public virtual int? ZFPM
        {
            get;
            set;
        }

        /// <summary>
        ///�Ӽ���
        /// </summary>
        public virtual decimal? SJDF
        {
            get;
            set;
        }

        /// <summary>
        ///ָ�����
        /// </summary>
        public virtual string DFZBXH
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
