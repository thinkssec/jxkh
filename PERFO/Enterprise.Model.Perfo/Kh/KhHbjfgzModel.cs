using Enterprise.Model.Perfo.Zbk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// �ϲ��Ʒֹ����
    /// ������:����������
    /// ����ʱ��:2014/12/2 13:41:05
    /// </summary>
    [Serializable]
    public class KhHbjfgzModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        /// �ϲ��Ʒ�ID
        /// </summary>
        public virtual string HBJFID
        {
            get;
            set;
        }

        /// <summary>
        ///�ϲ��Ʒ�����
        /// </summary>
        public virtual string HBJFMC
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
        ///������λ
        /// </summary>
        public virtual string HBJFDW
        {
            get;
            set;
        }

        /// <summary>
        ///��������ID
        /// </summary>
        public virtual string GZID
        {
            get;
            set;
        }

        #endregion

        #region ������

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        public virtual ZbkJsgzModel Jsgz { get; set; }

        #endregion
    }

}
