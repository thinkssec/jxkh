using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// ָ�����
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkZbxxModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///ָ��ID
        /// </summary>
        public virtual int ZBID
        {
            get;
            set;
        }

        /// <summary>
        ///ָ������
        /// </summary>
        public virtual string ZBLX
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
        ///һ������
        /// </summary>
        public virtual string YJZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string EJZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string SJZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///˳���
        /// </summary>
        public virtual string SXH
        {
            get;
            set;
        }

        /// <summary>
        ///ָ��״̬ Y=��Ч N=��Ч
        /// </summary>
        public virtual string ZBZT
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual DateTime? TJRQ
        {
            get;
            set;
        }

        #endregion
    }

}
