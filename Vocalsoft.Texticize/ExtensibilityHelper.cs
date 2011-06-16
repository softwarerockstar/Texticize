using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace Vocalsoft.ComponentModel
{
    public class ExtensibilityHelper<TPluginInterface, KMetadataInterface>
    {
        [ImportMany]
        private IEnumerable<Lazy<TPluginInterface, KMetadataInterface>> _plugins;

        private CompositionContainer _container;

        private static ExtensibilityHelper<TPluginInterface, KMetadataInterface> _instance;

        static ExtensibilityHelper()
        {
            _instance = new ExtensibilityHelper<TPluginInterface, KMetadataInterface>();
        }

        public static ExtensibilityHelper<TPluginInterface, KMetadataInterface> Current { get { return _instance; } }

        private ExtensibilityHelper()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            //string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
            //string path = Path.GetDirectoryName(codeBase);
            catalog.Catalogs.Add(new DirectoryCatalog("."));

            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        public IEnumerable<TPluginInterface> GetPlugins()
        {
            foreach (var plugin in _plugins)
                yield return plugin.Value;
        }

        public IEnumerable<TPluginInterface> GetPlugins(Func<Lazy<TPluginInterface, KMetadataInterface>, bool> selector)
        {
            foreach (Lazy<TPluginInterface, KMetadataInterface> processor in _plugins)
                if (selector(processor))
                    yield return processor.Value;
        }

        public IEnumerable<TPluginInterface> GetPlugins<T>(Func<KMetadataInterface, T> metadataProperty, T metedataPropertyValue)
        {
            foreach (Lazy<TPluginInterface, KMetadataInterface> processor in _plugins)
                if (metadataProperty(processor.Metadata).Equals(metedataPropertyValue))
                    yield return processor.Value;

        }
    }
}
