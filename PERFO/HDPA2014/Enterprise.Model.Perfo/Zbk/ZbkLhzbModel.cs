using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// ����ָ��ά��
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkLhzbModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///����ָ�����
        /// </summary>
        public virtual string LHZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///����ID
        /// </summary>
        public virtual string GZID
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
        ///���㵥λ
        /// </summary>
        public virtual string JSDW
        {
            get;
            set;
        }

        /// <summary>
        ///����Ȩ��
        /// </summary>
        public virtual decimal? BJQZ
        {
            get;
            set;
        }

        /// <summary>
        ///ָ��˵��
        /// </summary>
        public virtual string ZBSM
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
        ///�Ƿ����
        /// </summary>
        public virtual string SFJY
        {
            get;
            set;
        }

        /// <summary>
        ///�ϼ�ָ��
        /// </summary>
        public virtual string PARENTZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///����ָ��
        /// </summary>
        public virtual string FZZB
        {
            get;
            set;
        }

        /// <summary>
        ///������ʽ
        /// </summary>
        public virtual string JSBDS
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string JSMS
        {
            get;
            set;
        }

        /// <summary>
        ///��׼����
        /// </summary>
        public virtual decimal? JZFS
        {
            get;
            set;
        }

        /// <summary>
        ///ָ�����
        /// </summary>
        public virtual string ZBDH
        {
            get;
            set;
        }

        /// <summary>
        /// �Ƿ��ȡ����������Ŀ��ֵ
        /// </summary>
        public virtual string ISMBZ
        {
            get;
            set;
        }

        /// <summary>
        /// ָ����ʾ���
        /// </summary>
        public virtual string ZBXH
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
        /// ����������
        /// </summary>
        public virtual ZbkJsgzModel Jsgz { get; set; }
        /// <summary>
        /// ָ�������Ϣ
        /// </summary>
        public virtual ZbkZbxxModel Zbxx { get; set; }
        /// <summary>
        /// Ŀ��ֵ�
        /// </summary>
        public virtual IList<ZbkMbztbModel> MbztbLst { get; set; }
        /// <summary>
        /// Ŀ��ֵ���
        /// </summary>
        public virtual IList<ZbkMbzshModel> MbzshLst { get; set; }
        /// <summary>
        /// ���ֵ�
        /// </summary>
        public virtual IList<ZbkWcztbModel> WcztbLst { get; set; }
        /// <summary>
        /// ���ֵ���
        /// </summary>
        public virtual IList<ZbkWczshdfModel> WczshdfLst { get; set; }
        /// <summary>
        /// ����ָ��������ɱ�ʾ����
        /// </summary>
        public virtual string GradeSymbol
        {
            get
            {
                string s = string.Empty;
                if (!string.IsNullOrEmpty(ZBXH) && ZBXH.Length > 5)
                {
                    int cou = Convert.ToInt32((ZBXH.Length - 5) * 1M / 3M);
                    for (int i = 0; i < cou; i++)
                    {
                        s += "���";
                    }
                }
                return s;
            }
        }

        #endregion
    }

}
