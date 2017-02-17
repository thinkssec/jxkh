using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ��Ӫ�Ѷ�ϵ��
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhNdxsModel : PerfoSuperModel
    {
        #region ����������

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
        ///�Ѷ�ϵ��
        /// </summary>
        public virtual decimal? NDXS
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// ���Ż���ʵ��
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// ���˹���ʵ��
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        #endregion
    }

}
