//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
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
