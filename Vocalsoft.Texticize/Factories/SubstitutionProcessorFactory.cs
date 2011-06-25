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
    public static class SubstitutionProcessorFactory
    {
        public static ISubstitutionProcessor GetProcessor(string processorName)
        {
            return ExtensibilityHelper<ISubstitutionProcessor>.Current.GetPluginByUniqueName(processorName);
        }
    }
}
