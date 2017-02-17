using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// �ļ���:  IKhKhdfpxfwData.cs
    /// ��������: ���ݲ�-���˵÷�����Χ�������ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/12/2 13:41:05
    /// </summary>
    public interface IKhKhdfpxfwData : IDataPerfo<KhKhdfpxfwModel>
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
