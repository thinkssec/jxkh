using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// �ļ���:  IKhXsData.cs
    /// ��������: ���ݲ�-ϵ�����ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/10/27 14:17:54
    /// </summary>
    public interface IKhXsData : IDataPerfo<KhXsModel>
    {
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhXsModel> GetListBySQL(string sql);

        #endregion
    }

}
