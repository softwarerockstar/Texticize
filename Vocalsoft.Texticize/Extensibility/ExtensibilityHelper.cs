//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Vocalsoft.ComponentModel
{
    public class ExtensibilityHelper<T> : IDisposable
    {
        [ImportMany]
        private IEnumerable<Lazy<T, IExtensionUniqueName>> _plugins;

        private CompositionContainer _container;

        private static ExtensibilityHelper<T> _instance = new ExtensibilityHelper<T>();

        public static ExtensibilityHelper<T> Current 
        { 
            get 
            {
                var isStateful = (typeof(T).GetCustomAttributes(typeof(StatefulExtension), true).Length > 0);
                return (isStateful) ? new ExtensibilityHelper<T>() : _instance;
            } 
        }

        private ExtensibilityHelper()
        {
            using (AggregateCatalog catalog = new AggregateCatalog())
            {
                catalog.Catalogs.Add(new DirectoryCatalog("."));

                _container = new CompositionContainer(catalog);
                _container.ComposeParts(this);
            }
        }

        public IEnumerable<T> GetPlugins()
        {
            foreach (var plugin in _plugins)
                yield return plugin.Value;
        }

        public IEnumerable<T> GetPlugins(Func<Lazy<T, IExtensionUniqueName>, bool> selector)
        {
            foreach (Lazy<T, IExtensionUniqueName> processor in _plugins)
                if (selector(processor))
                    yield return processor.Value;
        }

        public T GetPluginByUniqueName(string uniqueName)
        {
            T toReturn = default(T);
            var plugins = new List<T>(GetPlugins(s => s.Metadata.UniqueName == uniqueName));

            if (plugins.Count > 0)
                toReturn = plugins[0];

            return toReturn;
        }

        public void Dispose()
        {   
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_container != null)
                {
                    _container.Dispose();
                    _container = null;
                }
            }
        }
    }
}
