using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// ���ָ��ά��
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkDfzbModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///���ָ�����
        /// </summary>
        public virtual string DFZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///ָ��ID
        /// </summary>
        public virtual int ZBID
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
        ///���޷�ֵ
        /// </summary>
        public virtual decimal? JXFZ
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string PFLX
        {
            get;
            set;
        }

        /// <summary>
        ///�Ƿ�����
        /// </summary>
        public virtual string SFFJX
        {
            get;
            set;
        }

        /// <summary>
        ///���ֱ�׼
        /// </summary>
        public virtual string PFBZ
        {
            get;
            set;
        }

        /// <summary>
        ///�Ƿ���� 1=��
        /// </summary>
        public virtual string DISABLE
        {
            get;
            set;
        }

        /// <summary>
        ///���ʽ
        /// </summary>
        public virtual string DFBDS
        {
            get;
            set;
        }

        /// <summary>
        ///���ֵ
        /// </summary>
        public virtual decimal? MAXV
        {
            get;
            set;
        }

        /// <summary>
        ///��Сֵ
        /// </summary>
        public virtual decimal? MINV
        {
            get;
            set;
        }

        /// <summary>
        /// �������ID
        /// </summary>
        public virtual string GZID
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

        #region �Զ�������

        /// <summary>
        /// ָ�������Ϣ
        /// </summary>
        public virtual ZbkZbxxModel Zbxx { get; set; }
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        public virtual ZbkJsgzModel Jsgz { get; set; }

        //˵�����������ֶβ��Ǳ��е���������Ҫ�޸Ķ�Ӧ��hbm�ļ�����Ϊ������������
        /// <summary>
        /// ����ֻ�������
        /// </summary>
        public virtual IList<ZbkBdfjgModel> BdfjgLst { get; set; }
        /// <summary>
        /// ����߼���
        /// </summary>
        public virtual IList<ZbkDfzModel> DfzLst { get; set; }

        #endregion

    }

}
