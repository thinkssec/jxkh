using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// ϵͳ�������
    /// ������:����������
    /// ����ʱ��:	2014/10/31 10:02:24
    /// </summary>
    [Serializable]
    public class SysCacheModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///����������
        /// </summary>
        public virtual string CACHENAME
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
        ///���ݱ���
        /// </summary>
        public virtual string TABLENAME
        {
            get;
            set;
        }

        /// <summary>
        /// ���������������ִ�
        /// </summary>
        public virtual string RELATIONCACHE
        {
            get;
            set;
        }

        #endregion
    }

}
