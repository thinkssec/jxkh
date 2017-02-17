using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// ������־��
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysVisitlogModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///ID
        /// </summary>
        public virtual string ID
        {
            get;
            set;
        }

        /// <summary>
        ///�û���
        /// </summary>
        public virtual string USERNAME
        {
            get;
            set;
        }

        /// <summary>
        ///����·��
        /// </summary>
        public virtual string VISITURL
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string OPERATIONTYPE
        {
            get;
            set;
        }

        /// <summary>
        ///����ʱ��
        /// </summary>
        public virtual DateTime? OPERATIONDATE
        {
            get;
            set;
        }

        /// <summary>
        ///��·IP
        /// </summary>
        public virtual string VISITIPADDR
        {
            get;
            set;
        }

        /// <summary>
        ///���������
        /// </summary>
        public virtual string BROWSERTYPE
        {
            get;
            set;
        }

        #endregion
    }

}
