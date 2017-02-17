using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// ����ģ���
    /// ������:����������
    /// ����ʱ��:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class SysModuleModel : PerfoSuperModel
    {

        #region ����������

        /// <summary>
        ///ģ��ID
        /// </summary>
        public virtual string MID
        {
            get;
            set;
        }

        /// <summary>
        ///��������
        /// </summary>
        public virtual string MNAME
        {
            get;
            set;
        }

        /// <summary>
        ///����ҳ��
        /// </summary>
        public virtual string MURL
        {
            get;
            set;
        }

        /// <summary>
        ///�Ƿ���� 1=�� 0=��
        /// </summary>
        public virtual string DISABLE
        {
            get;
            set;
        }

        /// <summary>
        ///������ҳ��ID
        /// </summary>
        public virtual string MPARENTID
        {
            get;
            set;
        }

        /// <summary>
        ///��ʾ���
        /// </summary>
        public virtual string XSXH
        {
            get;
            set;
        }

        /// <summary>
        ///��ע
        /// </summary>
        public virtual string BZ
        {
            get;
            set;
        }

        /// <summary>
        ///ӳ��·��
        /// </summary>
        public virtual string WEBURL
        {
            get;
            set;
        }

        #endregion

    }

}
