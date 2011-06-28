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
using SoftwareRockstar.ComponentModel.Extensibility;

namespace SoftwareRockstar.Texticize
{    
    /// <summary>
    /// ITemplateProcessor
    /// </summary>    
    public interface ITemplateProcessor
    {
        ITemplateProcessor ClearMaps();
        ITemplateProcessor ClearVariables();
        ITemplateProcessor FetchIncludes();        
        ITemplateProcessor SetConfiguration(Configuration configuration);
        ITemplateProcessor SetMaps(params KeyValuePair<string, Delegate>[] maps);
        ITemplateProcessor SetTemplateReader(ITemplateReader reader);
        ITemplateProcessor SetDefaultVariable(object variable);
        ITemplateProcessor SetVariables(params KeyValuePair<string, object>[] variables);
        ProcessorOutput Process();
    }
}
