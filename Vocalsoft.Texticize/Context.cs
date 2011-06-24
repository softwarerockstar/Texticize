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

namespace Vocalsoft.Texticize
{
    [Serializable]
    public class Context<T> : IContext
    {
        public T Variable { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }
        public string VariableName { get; private set; }
        public string Expression { get; private set; }

        public void SetProperties(object variable, string variableName, string expression, Dictionary<string, string> parameters)
        {
            Variable = (T)variable;
            Parameters = parameters;
            VariableName = variableName;
            Expression = expression;
        }
    }
}
