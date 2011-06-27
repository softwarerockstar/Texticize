//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Abstract Extension Factory .NET
//      Codeplex Project: http://aef.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Vocalsoft.ComponentModel.Extensibility
{
    public abstract class AbstractExtensionFactory<T> : IDisposable
    {
        [ImportMany]
        private IEnumerable<Lazy<T, IUniquenessEvidence>> _extensions = null;

        private CompositionContainer _container;

        protected AbstractExtensionFactory()
        {
            using (AggregateCatalog catalog = new AggregateCatalog())
            {
                catalog.Catalogs.Add(new DirectoryCatalog("."));

                _container = new CompositionContainer(catalog);
                _container.ComposeParts(this);
            }
        }

        protected IEnumerable<T> GetExtensions()
        {
            foreach (var plugin in _extensions)
                yield return plugin.Value;
        }

        protected IEnumerable<T> GetExtensions(Func<Lazy<T, IUniquenessEvidence>, bool> selector)
        {
            foreach (Lazy<T, IUniquenessEvidence> processor in _extensions)
                if (selector(processor))
                    yield return processor.Value;
        }

        protected T GetGetExtensionByUniqueName(string uniqueName)
        {
            T toReturn = default(T);
            var plugins = new List<T>(GetExtensions(s => s.Metadata.UniqueName == uniqueName));

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
