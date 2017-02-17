using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// �ļ���:  IKhsMbjgData.cs
    /// ��������: ���ݲ�-ģ��������ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/5 22:09:24
    /// </summary>
    public interface IKhsMbjgData : IDataPerfo<KhsMbjgModel>
    {
        bool ExecuteSQL(string sql);
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsMbjgModel> GetListBySQL(string sql);

        #endregion
    }

}
