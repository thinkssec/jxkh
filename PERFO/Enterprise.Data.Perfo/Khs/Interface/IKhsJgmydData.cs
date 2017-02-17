using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// �ļ���:  IKhsJgmydData.cs
    /// ��������: ���ݲ�-������������ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/8 22:00:44
    /// </summary>
    public interface IKhsJgmydData : IDataPerfo<KhsJgmydModel>
    {
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsJgmydModel> GetListBySQL(string sql);

        #endregion
    }

}
