using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Data.Perfo.Zbk
{	

    /// <summary>
    /// �ļ���:  IZbkBdfjgData.cs
    /// ��������: ���ݲ�-����ֻ������ݷ��ʽӿ�
    /// �����ˣ�����������
    /// ����ʱ�䣺2014/11/1 0:35:46
    /// </summary>
    public interface IZbkBdfjgData : IDataPerfo<ZbkBdfjgModel>
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
