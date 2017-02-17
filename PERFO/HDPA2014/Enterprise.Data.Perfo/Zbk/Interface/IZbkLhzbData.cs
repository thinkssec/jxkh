using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Data.Perfo.Zbk
{	

    /// <summary>
    /// �ļ���:  IZbkLhzbData.cs
    /// ��������: ���ݲ�-����ָ��ά�����ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/11/1 0:35:46
    /// </summary>
    public interface IZbkLhzbData : IDataPerfo<ZbkLhzbModel>
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
