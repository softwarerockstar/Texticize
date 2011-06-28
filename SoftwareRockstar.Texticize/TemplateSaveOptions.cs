//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SoftwareRockstar.Texticize
{
    [Flags]
    public enum TemplateSaveOptions
    {
        None = 0x0,
        PreFetchIncludes = 0x1
    }
}
