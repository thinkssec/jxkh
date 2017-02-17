using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khx;

namespace Enterprise.Data.Perfo.Khx
{	

    /// <summary>
    /// �ļ���:  IKhxResultData.cs
    /// ��������: ���ݲ�-���˵÷����ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/12 19:14:36
    /// </summary>
    public interface IKhxResultData : IDataPerfo<KhxResultModel>
    {
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhxResultModel> GetListBySQL(string sql);

        #endregion
    }

}
