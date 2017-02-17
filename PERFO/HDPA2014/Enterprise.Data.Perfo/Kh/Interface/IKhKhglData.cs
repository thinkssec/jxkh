using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{	

    /// <summary>
    /// �ļ���:  IKhKhglData.cs
    /// ��������: ���ݲ�-���˹������ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/11/1 0:35:45
    /// </summary>
    public interface IKhKhglData : IDataPerfo<KhKhglModel>
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
