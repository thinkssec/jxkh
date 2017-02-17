using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// �û���
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysUserModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///��¼ID
        /// </summary>
        public virtual string LOGINID
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual int JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///��ɫID
        /// </summary>
        public virtual string ROLEID
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
        ///����
        /// </summary>
        public virtual string PASSWORD
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
        ///IP��ַ
        /// </summary>
        public virtual string IPADDR
        {
            get;
            set;
        }

        /// <summary>
        ///ְ��
        /// </summary>
        public virtual string DUTY
        {
            get;
            set;
        }

        /// <summary>
        ///�������
        /// </summary>
        public virtual DateTime? ADDDATE
        {
            get;
            set;
        }

        /// <summary>
        ///�û��˵����
        /// </summary>
        public virtual string MENU
        {
            get;
            set;
        }

        /// <summary>
        /// ��ע
        /// </summary>
        public virtual string BZ
        {
            get;
            set;
        }

        #endregion


        #region �Զ�������

        /// <summary>
        /// ��ɫ
        /// </summary>
        public virtual SysRoleModel Role { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// �ֹܵĻ�����Ϣ����
        /// </summary>
        public virtual IList<SysFgbmjgModel> FgbmjgLst { get; set; }

        #endregion

    }

}
