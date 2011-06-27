using System;
using System.Collections.Generic;
using Vocalsoft.ComponentModel;

namespace Vocalsoft.Texticize
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
        ITemplateProcessor SetTemplate(string template);
        ITemplateProcessor SetVariable(object variable);
        ITemplateProcessor SetVariables(params KeyValuePair<string, object>[] variables);
        ProcessorOutput Process();
    }
}
