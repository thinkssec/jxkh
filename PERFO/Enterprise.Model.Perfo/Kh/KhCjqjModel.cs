using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// �ɼ���������
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhCjqjModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///����ID
        /// </summary>
        public virtual int? KHID
        {
            get;
            set;
        }

        /// <summary>
        ///����ȼ�
        /// </summary>
        public virtual string QJDJ
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ
        /// </summary>
        public virtual decimal? UPPERV
        {
            get;
            set;
        }

        /// <summary>
        ///��Сֵ
        /// </summary>
        public virtual decimal? LOWERV
        {
            get;
            set;
        }

        #endregion

        #region ����������

        /// <summary>
        /// ������ʵ��
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        #endregion
    }

}
