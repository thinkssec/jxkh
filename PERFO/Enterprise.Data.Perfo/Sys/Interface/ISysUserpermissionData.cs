using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Data.Perfo.Sys
{	

    /// <summary>
    /// �ļ���:  ISysUserpermissionData.cs
    /// ��������: ���ݲ�-���ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/12/1 9:32:19
    /// </summary>
    public interface ISysUserpermissionData : IDataPerfo<SysUserpermissionModel>
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
