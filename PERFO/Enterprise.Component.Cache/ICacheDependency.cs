using System.Web.Caching;

namespace Enterprise.Component.Cache
{
    /// <summary>
    /// 这个接口是从属工厂（工厂模式）返回的
    /// 开发人员可以执行这个接口将不同类型的从属缓存添加到pet shop
    /// 
    /// 用于实现简单工厂模式中的抽象接口，开发者可以实现该接口成员，扩展缓存依赖功能应用。
    /// </summary>
    public interface ICacheDependency
    {

        /// <summary>
        ///此方法创建从属缓存适当的执行
        /// 
        ///该方法用于获取创建聚合缓存依赖类AggregateCacheDependency的实例
        /// </summary>
        /// <returns>从属缓存集合里深入的从属缓存，聚合缓存依赖类AggregateCacheDependency</returns>
        AggregateCacheDependency GetDependency();
    }
}
