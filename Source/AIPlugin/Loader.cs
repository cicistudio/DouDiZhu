using System;
using System.Reflection;
using System.Collections.Generic;

namespace AIPlugin
{
    public class Loader : IDisposable
    {
        private AppDomain m_Domain = null;
        /// <summary>
        /// 该Loader负责的Domain
        /// </summary>
        public AppDomain Domain
        {
            get { return m_Domain; }
            set { m_Domain = value; }
        }

        private Dictionary<string, string> m_FullNameDict = new Dictionary<string, string>();

        public Dictionary<string, string> FullNameDict
        {
            get { return m_FullNameDict; }
            set { m_FullNameDict = value; }
        }

        /// <summary>
        /// 该Domain中的RemoteLoader对象。
        /// </summary>
        private RemoteLoader m_RemoteLoader = null;

        /// <summary>
        /// 一个Loader只负责一个Domain
        /// </summary>
        /// <param name="domainFriendlyName">Domain的Name</param>
        public Loader(string domainFriendlyName)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ShadowCopyFiles = "true";
            m_Domain = AppDomain.CreateDomain(domainFriendlyName, null, setup);
         
            //object[] parms = { domainFriendlyName };
            BindingFlags bindings = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public;
            try
            {
                //生成这个Domain中的RemoteLoader对象。可用于和其他Domain或主程序交换
                m_RemoteLoader = (RemoteLoader)m_Domain.CreateInstanceFromAndUnwrap(
                    "AIPlugin.dll", "AIPlugin.RemoteLoader", true, bindings,
                    null, null, null, null, null);
                //null, parms, null, null, null);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 向该Domain中增加一个程序集
        /// </summary>
        /// <param name="keyName">这个程序集定义的key</param>
        /// <param name="assemblyPath">程序集路径</param>
        /// <returns></returns>
        public void AddAssembly(string keyName, string assemblyPath)
        {
            /*有的程序集回加载失败，需要检查以下内容。 
             * Check the dependencies of the assembly you're trying to load. If those dependancies are not in the GAC 
             *  and not in the base directory of the running application, you'll get load failures. For some reason, the exception 
             *   that gets thrown back across the AppDomain boundary triggers a SerializationException when it gets remoted. 
             */
            m_RemoteLoader.LoadAssembly(keyName,assemblyPath);
            //不要从m_RemoteLoader中返回Assembly并赋值给主AppDomain，这样的话，主Domain也会加载该Dll文件。
            //直接invoke里面的方法就好。
        }

        public object Invoke(string key, string methodName)
        {
            return Invoke(key, m_FullNameDict[key], methodName, null);
        }

        public object Invoke(string key, string methodName, params Object[] args)
        {
            return m_RemoteLoader.Invoke(key, m_FullNameDict[key], methodName, args);
        }

        /// <summary>
        /// 卸载这个Domain
        /// </summary>
        public void Unload()
        {
            AppDomain.Unload(Domain);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Unload();
        }

        #endregion

    }
}
