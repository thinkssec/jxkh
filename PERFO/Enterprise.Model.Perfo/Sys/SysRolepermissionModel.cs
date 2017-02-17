using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// ��ɫȨ�ޱ�
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysRolepermissionModel : PerfoSuperModel
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
        ///ģ��ID
        /// </summary>
        public virtual string MID
        {
            get;
            set;
        }

        /// <summary>
        /// ��Ȩ��
        /// </summary>
        public virtual int? MODULEPERMISSION
        {
            get;
            set;
        }

        #endregion

        #region �Զ�������

        /// <summary>
        /// ��ɫ����
        /// </summary>
        public virtual SysRoleModel Role { get; set; }

        /// <summary>
        /// ģ�����
        /// </summary>
        public virtual SysModuleModel Module { get; set; }

        #endregion
    }

}
