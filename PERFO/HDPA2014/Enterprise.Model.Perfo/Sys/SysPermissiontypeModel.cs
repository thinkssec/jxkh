using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// Ȩ�����ͱ�
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysPermissiontypeModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///����ֵ
        /// </summary>
        public virtual int ACTIONKEY
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual string ACTIONABBR
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string ACTIONNAME
        {
            get;
            set;
        }

        #endregion

        #region �Զ�������

        /// <summary>
        /// ��ɫ����
        /// </summary>
        public virtual SysRoleModel Role
        {
            get;
            set;
        }

        /// <summary>
        /// ģ�����
        /// </summary>
        public virtual SysModuleModel Module
        {
            get;
            set;
        }

        #endregion

    }

}
