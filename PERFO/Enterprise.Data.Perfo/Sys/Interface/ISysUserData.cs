using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Data.Perfo.Sys
{	

    /// <summary>
    /// �ļ���:  ISysUserData.cs
    /// ��������: ���ݲ�-�û������ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/11/1 0:35:46
    /// </summary>
    public interface ISysUserData : IDataPerfo<SysUserModel>
    {
        #region ����������
        
        /// <summary>
        /// ִ�л���SQL��ԭ������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool ExecuteSQL(string sql);

        #endregion
    }

}
