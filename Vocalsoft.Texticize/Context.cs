using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    public class Context<T> : IContext
    {
        public T Variable { get; private set; }
        public IList<string> RegexGroups { get; private set; }
        public string VariableName { get; private set; }
        public string Expression { get; private set; }

        public void SetProperties(object variable, string variableName, string expression, IList<string> regexGroups)
        {
            Variable = (T)variable;
            RegexGroups = regexGroups;
            VariableName = variableName;
            Expression = expression;
        }
    }
}
