//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
using SoftwareRockstar.ComponentModel.Extensibility;

namespace SoftwareRockstar.Texticize.Factories
{
    /// <summary>
    /// Factory class for subsitution processors.
    /// </summary>
    public class SubstitutionProcessorFactory : AbstractExtensionFactory<ISubstitutionProcessor>
    {
        /// <summary>
        /// Creates the specified substitution processor using provided unique name.
        /// </summary>
        /// <param name="substitutionProcessorUniqueName">Name of the substitution processor unique.</param>
        /// <returns></returns>
        public static ISubstitutionProcessor Create(string substitutionProcessorUniqueName)
        {
            using (var factory = new SubstitutionProcessorFactory())
            {
                return factory.GetGetExtensionByUniqueName(substitutionProcessorUniqueName);
            }
        }

    }
}
