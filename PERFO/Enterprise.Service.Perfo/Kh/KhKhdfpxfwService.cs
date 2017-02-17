using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Service.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// �ļ���:  KhKhdfpxfwService.cs
    /// ��������: ҵ���߼���-���˵÷�����Χ�������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/12/2 13:41:05
    /// </summary>
    public class KhKhdfpxfwService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhKhdfpxfwData dal = new KhKhdfpxfwData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhKhdfpxfwModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhKhdfpxfwModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡ�뿼���ںͿ��˶�����Ӧ������
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="khType">��������</param>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetListByKhidAndKhdx(string khid, string khType)
        {
            string hql = "from KhKhdfpxfwModel p where p.KHID='" + khid + "' and p.KHDX='" + khType + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡ������λ����
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��������</param>
        /// <param name="khType">��������</param>
        /// <returns></returns>
        public string GetGsdwmc(string khid, int jgbm, string khType)
        {
            string gsdwmc = string.Empty;
            var list = GetListByKhidAndKhdx(khid, khType);
            SysBmjgService bmjgSrv = new SysBmjgService();
            var bmjg = bmjgSrv.GetSingle(jgbm.ToString());
            if (bmjg != null && bmjg.XSXH.Length >= 4)
            {
                var parentDw = bmjgSrv.GetModelByXsxh(bmjg.XSXH, 4);
                if (parentDw != null)
                {
                    var pxfw = list.FirstOrDefault(p => p.GSDW.Contains("," + parentDw.JGBM + ","));
                    if (pxfw != null)
                    {
                        string[] jgbms = pxfw.GSDW.TrimStart(',').TrimEnd(',').Split(',');
                        foreach (var dw in jgbms)
                        {
                            var bmjgModel = bmjgSrv.GetSingle(dw);
                            if (bmjgModel != null)
                            {
                                gsdwmc += bmjgModel.JGMC + "��";
                            }
                        }
                    }
                    else
                    {
                        gsdwmc = parentDw.JGMC;
                    }
                }
            }
            return gsdwmc.TrimEnd("��".ToCharArray());
        }

        #endregion
    }

}
