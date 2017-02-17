using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Data.Perfo.Zbk
{	

    /// <summary>
    /// �ļ���:  IZbkCwjcsjglzbData.cs
    /// ��������: ���ݲ�-����ָ�������������ݶ�Ӧ�����ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/12/14 9:19:20
    /// </summary>
    public interface IZbkCwjcsjglzbData : IDataPerfo<ZbkCwjcsjglzbModel>
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
