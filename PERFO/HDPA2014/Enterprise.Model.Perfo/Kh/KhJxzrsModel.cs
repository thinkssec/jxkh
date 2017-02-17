using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// ����ָ�꿼�˱�
    /// ������:����������
    /// ����ʱ��:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhJxzrsModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///������ID
        /// </summary>
        public virtual int ZRSID
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual int? JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///����������
        /// </summary>
        public virtual string ZRSMC
        {
            get;
            set;
        }

        /// <summary>
        ///������״̬
        /// </summary>
        public virtual string ZRSZT
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual int? SZND
        {
            get;
            set;
        }

        /// <summary>
        ///�����鸽��
        /// </summary>
        public virtual string ZRSFJ
        {
            get;
            set;
        }

        /// <summary>
        ///��������ID
        /// </summary>
        public virtual string LXID
        {
            get;
            set;
        }

        /// <summary>
        ///�ύ����
        /// </summary>
        public virtual DateTime? TJRQ
        {
            get;
            set;
        }

        /// <summary>
        ///������
        /// </summary>
        public virtual int? FZBM
        {
            get;
            set;
        }

        /// <summary>
        ///�´�����
        /// </summary>
        public virtual DateTime? XDRQ
        {
            get;
            set;
        }

        #endregion

        #region ��������

        /// <summary>
        /// ������ָ�꼯��
        /// </summary>
        public virtual IList<KhJxzrszbModel> JxzrszbLst { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// �������������Ļ���
        /// </summary>
        public virtual SysBmjgModel FzBmjg { get; set; }

        /// <summary>
        /// �������Ͷ���
        /// </summary>
        public virtual KhKindModel KhKind { get; set; }

        #endregion

    }

}
