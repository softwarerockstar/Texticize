using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize.Factories
{
    public static class ProcessorFactory
    {
        public static IProcessor GetProcessor(string processorName)
        {
            var plugins = ExtensibilityHelper<IProcessor, IProcessorMetaData>.Current;

            return plugins
                .GetPlugins(s => s.Metadata.Processor == processorName)
                .FirstOrDefault();
        }
    }
}
