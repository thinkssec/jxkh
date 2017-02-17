using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// �ļ���:  IKhCwjcsjData.cs
    /// ��������: ���ݲ�-����������ݱ����ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/12/13 10:23:51
    /// </summary>
    public interface IKhCwjcsjData : IDataPerfo<KhCwjcsjModel>
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
