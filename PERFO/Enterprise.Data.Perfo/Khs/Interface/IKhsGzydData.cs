using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// �ļ���:  IKhsGzydData.cs
    /// ��������: ���ݲ�-����Ҫ���������ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/8 23:49:46
    /// </summary>
    public interface IKhsGzydData : IDataPerfo<KhsGzydModel>
    {
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsGzydModel> GetListBySQL(string sql);

        #endregion
    }

}
