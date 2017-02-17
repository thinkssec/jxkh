using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// ��������
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class SysBmjgModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///��������
        /// </summary>
        public virtual int JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string JGMC
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string JGLX
        {
            get;
            set;
        }

        /// <summary>
        ///�Ƿ񿼺� 1=�� 0=��
        /// </summary>
        public virtual string SFKH
        {
            get;
            set;
        }

        /// <summary>
        ///��ʾ���
        /// </summary>
        public virtual string XSXH
        {
            get;
            set;
        }

        /// <summary>
        ///��ע==ͬ������
        /// </summary>
        public virtual string BZ
        {
            get;
            set;
        }

        #endregion
    }

}
