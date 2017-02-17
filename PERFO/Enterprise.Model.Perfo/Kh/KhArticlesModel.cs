using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ֪ͨ����
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:50
    /// </summary>
    [Serializable]
    public class KhArticlesModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///֪ͨID
        /// </summary>
        public virtual string TZID
        {
            get;
            set;
        }

        /// <summary>
        ///����
        /// </summary>
        public virtual string TZBT
        {
            get;
            set;
        }

        /// <summary>
        ///����
        /// </summary>
        public virtual string TZNR
        {
            get;
            set;
        }

        /// <summary>
        ///����
        /// </summary>
        public virtual string TZZZ
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

        /// <summary>
        ///����
        /// </summary>
        public virtual string TZFJ
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual int? LLCS
        {
            get;
            set;
        }

        /// <summary>
        ///֪ͨ����
        /// </summary>
        public virtual string TZLX
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// ǩ���б����
        /// </summary>
        public virtual IList<KhSigninModel> SigninLst { get; set; }

        #endregion
    }

}
