using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using System.Reflection;

namespace Enterprise.Service.Perfo.Zbk
{

    /// <summary>
    /// 文件名:  ZbkJsgzHandler.cs
    /// 功能描述: 业务逻辑层-指标计算规则的调用处理类
    /// 创建人：QW
    /// 创建时间 ：2014/11/20
    /// </summary>
    public sealed class ZbkJsgzHandler
    {

        /// <summary>
        /// 计算规则逻辑类名称
        /// </summary>
        public static readonly string CLASS_NAME = "Enterprise.Service.Perfo.Zbk.ZbkJsgzLogic";

        /// <summary>
        /// 调用计算规则逻辑类中的指定方法并获取返回值
        /// </summary>
        /// <param name="e">参数对象</param>
        /// <param name="methodName">执行方法名</param>
        /// <returns></returns>
        public static object CallJsgzMethod(object e, string methodName)
        {
            try
            {
                //根据类型创建对象
                string[] classStrs = CLASS_NAME.Split('.');
                if (classStrs != null && classStrs.Length >= 3)
                {
                    string assemblyPath = string.Format("{0}.{1}.{2}", classStrs[0], classStrs[1], classStrs[2]);//生成的DLL文件名
                    object dObj = Assembly.Load(assemblyPath).CreateInstance(CLASS_NAME);
                    Type attr_t = dObj.GetType();
                    //if (typeof(ZbkJsgzLogic).IsAssignableFrom(attr_t))
                    //{
                    //    //生成的实例为 ZbkJsgzLogic
                    //}
                    //获取方法的信息
                    MethodInfo method = attr_t.GetMethod(methodName);
                    //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
                    BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
                    //调用方法
                    object[] attr_parameters = new object[] { e };
                    return method.Invoke(dObj, flag, Type.DefaultBinder, attr_parameters, null);
                }
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log("执行CallJsgzMethod方法出错!", ex.Message);
            }
            return null;
        }
    }


    #region 返回结果类

    /// <summary>
    /// 计算规则处理结果类
    /// </summary>
    public sealed class JsgzLogicResult
    {

        /// <summary>
        /// 是否有值
        /// </summary>
        public bool HasValue
        {
            get
            {
                return (ReturnValue != null);
            }
        }
        /// <summary>
        /// 返回值
        /// </summary>
        public decimal? ReturnValue { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 执行方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 方法参数
        /// </summary>
        public object ParamObj { get; set; }

    }

    #endregion


}
