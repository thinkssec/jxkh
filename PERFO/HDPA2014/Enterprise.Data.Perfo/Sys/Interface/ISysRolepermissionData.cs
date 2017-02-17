using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Data.Perfo.Sys
{	

    /// <summary>
    /// �ļ���:  ISysRolepermissionData.cs
    /// ��������: ���ݲ�-��ɫȨ�ޱ����ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/11/1 0:35:46
    /// </summary>
    public interface ISysRolepermissionData : IDataPerfo<SysRolepermissionModel>
    {
        #region �Զ��巽��

        /// <summary>
        /// ִ�л���SQL��ԭ������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool ExecuteSQL(string sql);
        
        #endregion
    }

}
