using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// �����������
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkJsgzModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///����ID
        /// </summary>
        public virtual string GZID
        {
            get;
            set;
        }

        /// <summary>
        ///�汾����
        /// </summary>
        public virtual string BBMC
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string GZMC
        {
            get;
            set;
        }

        /// <summary>
        ///���ʽ
        /// </summary>
        public virtual string GZBDS
        {
            get;
            set;
        }

        /// <summary>
        ///�����߼�����ķ�������
        /// </summary>
        public virtual string METHODNAME
        {
            get;
            set;
        }

        /// <summary>
        ///�ⶥֵ
        /// </summary>
        public virtual decimal? MAXV
        {
            get;
            set;
        }

        /// <summary>
        ///����ֵ
        /// </summary>
        public virtual decimal? MINV
        {
            get;
            set;
        }

        /// <summary>
        ///����ֵ
        /// </summary>
        public virtual decimal? UPPER
        {
            get;
            set;
        }

        /// <summary>
        ///����ֵ
        /// </summary>
        public virtual decimal? LOWER
        {
            get;
            set;
        }

        /// <summary>
        /// �ɱ�ID
        /// </summary>
        public virtual string OLDID
        {
            get;
            set;
        }

        #endregion
    }

}
