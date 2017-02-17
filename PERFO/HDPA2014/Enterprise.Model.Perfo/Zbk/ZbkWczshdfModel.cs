using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// ���ֵ��˼������
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkWczshdfModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        /// ����ID
        /// </summary>
        public virtual int OPERID
        {
            get;
            set;
        }

        /// <summary>
        ///����ָ�����
        /// </summary>
        public virtual string LHZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///������
        /// </summary>
        public virtual string OPERATOR
        {
            get;
            set;
        }

        /// <summary>
        ///���������� 1=������λ���� 0=���ز��ſ���
        /// </summary>
        public virtual string OPERTYPE
        {
            get;
            set;
        }

        /// <summary>
        ///����Ȩ��
        /// </summary>
        public virtual decimal? OPERQZ
        {
            get;
            set;
        }

        #endregion
    }

}
