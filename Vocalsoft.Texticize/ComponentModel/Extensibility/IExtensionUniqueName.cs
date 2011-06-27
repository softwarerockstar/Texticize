//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Abstract Extension Factory .NET
//      Codeplex Project: http://aef.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
namespace Vocalsoft.ComponentModel.Extensibility
{
    public interface IUniquenessEvidence
    {
        string UniqueName { get; }        
    }

    public static class UniquenessEvidenceFields
    {
        public const string UniqueName = "UniqueName";
    }
}
