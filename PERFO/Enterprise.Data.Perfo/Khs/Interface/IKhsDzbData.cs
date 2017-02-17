using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{	

    /// <summary>
    /// �ļ���:  IKhsDzbData.cs
    /// ��������: ���ݲ�-���˴�ָ�����ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/4 20:47:10
    /// </summary>
    public interface IKhsDzbData : IDataPerfo<KhsDzbModel>
    {
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhsDzbModel> GetListBySQL(string sql);

        #endregion
    }

}