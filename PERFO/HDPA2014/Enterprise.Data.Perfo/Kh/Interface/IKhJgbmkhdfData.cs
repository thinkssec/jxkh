using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// �ļ���:  IKhJgbmkhdfData.cs
    /// ��������: ���ݲ�-���ز��ſ��˵÷ֱ����ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/11/28 16:45:02
    /// </summary>
    public interface IKhJgbmkhdfData : IDataPerfo<KhJgbmkhdfModel>
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
