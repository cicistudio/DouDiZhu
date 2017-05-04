using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace AIPlugin
{
    public class RemoteLoader : MarshalByRefObject
    {
        Dictionary<string, Assembly> m_AssembliesDict = new Dictionary<string, Assembly>();

        public void LoadAssembly(string key,string pluginPath)
        {
            if (m_AssembliesDict.ContainsKey(key))
            {
                throw new Exception("Domain中已经包含这个关键字Key:" + key);
            }
            m_AssembliesDict.Add(key, Assembly.LoadFile(pluginPath));
        }

        public object Invoke(string key, string fullClassName, string methodName, params Object[] args)
        {
            if (!m_AssembliesDict.ContainsKey(key))
            {
                throw new Exception("Domain中找不到关键字为Key的程序集:" + key);
            }
            Type tp = m_AssembliesDict[key].GetType(fullClassName);
            if (tp == null)
            {
                throw new Exception("找不到类:" + fullClassName);
            }
            MethodInfo meth = tp.GetMethod(methodName);
            if (meth == null)
            {
                throw new Exception("找不到方法名:" + methodName);
            }

            object instance = Activator.CreateInstance(tp);
            return meth.Invoke(instance, args);
        }
    }
}
