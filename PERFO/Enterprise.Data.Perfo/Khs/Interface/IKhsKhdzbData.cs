using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// �ļ���:  IKhsKhdzbData.cs
    /// ��������: ���ݲ�-����ָ����ձ����ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/4 20:47:10
    /// </summary>
    public interface IKhsKhdzbData : IDataPerfo<KhsKhdzbModel>
    {
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsKhdzbModel> GetListBySQL(string sql);
        bool ExecuteSQL(string sql);
        #endregion
    }

}
