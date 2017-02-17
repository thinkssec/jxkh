using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// ��ɫ����
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysRoleModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///��ɫID
        /// </summary>
        public virtual string ROLEID
        {
            get;
            set;
        }

        /// <summary>
        ///��ɫ����
        /// </summary>
        public virtual string ROLENAME
        {
            get;
            set;
        }

        /// <summary>
        ///�Ƿ���� 1=�� 0=��
        /// </summary>
        public virtual string ROLEDISABLE
        {
            get;
            set;
        }

        /// <summary>
        ///��ɫͼƬ
        /// </summary>
        public virtual string ROLEPICTURE
        {
            get;
            set;
        }

        #endregion
    }

}
