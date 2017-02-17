using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 
    /// ������:����������
    /// ����ʱ��:2014/12/1 9:32:18
    /// </summary>
    [Serializable]
    public class SysUserpermissionModel : PerfoSuperModel
    {
        #region ����������

        /// <summary>
        ///�û���¼��
        /// </summary>
        public virtual string LOGINID
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
        ///Ȩ��ֵ
        /// </summary>
        public virtual int? MODULEPERMISSION
        {
            get;
            set;
        }

        #endregion

        #region �Զ�������

        /// <summary>
        /// ģ�����
        /// </summary>
        public virtual SysModuleModel Module { get; set; }

        #endregion
    }

}
