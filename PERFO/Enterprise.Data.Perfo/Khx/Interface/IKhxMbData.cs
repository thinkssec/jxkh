using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Khx;

namespace Enterprise.Data.Perfo.Khx
{	

    /// <summary>
    /// �ļ���:  IKhxMbData.cs
    /// ��������: ���ݲ�-����ģ�����ݷ��ʽӿ�
	/// �����ˣ�����������
	/// ����ʱ�䣺2015/11/5 20:46:51
    /// </summary>
    public interface IKhxMbData : IDataPerfo<KhxMbModel>
    {
        #region ����������
        
        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<KhxMbModel> GetListBySQL(string sql);

        #endregion
    }

}
