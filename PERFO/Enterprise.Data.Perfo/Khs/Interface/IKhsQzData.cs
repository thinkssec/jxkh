using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// �ļ���:  IKhsQzData.cs
    /// ��������: ���ݲ�-���ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/7 19:21:46
    /// </summary>
    public interface IKhsQzData : IDataPerfo<KhsQzModel>
    {
        bool ExecuteSQL(string sql);
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsQzModel> GetListBySQL(string sql);

        #endregion
    }

}
