//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;

namespace SoftwareRockstar.Texticize
{
    interface IContext
    {
        void SetProperties(object variable, string variableName, string expression, Dictionary<string, string> parameters);
    }
}
