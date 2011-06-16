//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses///      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    static class Utility
    {
        private static IDictionary<string, IContext> ContextCache = new Dictionary<string, IContext>();

        public static IContext CreateContext(object variable, string variableName, string expression, Dictionary<string, string> parameters)
        {
            Type elementType = variable.GetType();
            string cacheKey = variableName;

            if (!ContextCache.ContainsKey(cacheKey))
            {
                Type[] types = new Type[] { elementType };

                Type listType = typeof(Context<>);
                Type genericType = listType.MakeGenericType(types);
                var context = Activator.CreateInstance(genericType) as IContext;                
                ContextCache.Add(cacheKey, context);
            }

            var cachedContext = ContextCache[cacheKey];
            cachedContext.SetProperties(variable, variableName, expression, parameters);

            return cachedContext;
        }
    }
}
