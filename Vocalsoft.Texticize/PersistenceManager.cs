//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using Vocalsoft.ComponentModel.Serialization;

namespace Vocalsoft.Texticize
{
    public static class PersistenceManager
    {
        public static void Save(ITemplateProcessor processor, Uri localPath)
        {
            if (processor != null)
                Save(processor, localPath, TemplateSaveOptions.None);
        }

        public static void Save(ITemplateProcessor processor, Uri localPath, TemplateSaveOptions options)
        {
            if (processor != null)
            {
                if (options.HasFlag(TemplateSaveOptions.PreFetchIncludes))
                    processor.FetchIncludes();

                BinarySerializer.ObjectToFile(localPath, processor);
            }
        }

        public static ITemplateProcessor LoadFrom(ITemplateProcessor source)
        {
            return BinarySerializer.Base64StringToObject<ITemplateProcessor>(BinarySerializer.ObjectToBase64String(source));
        }

        public static ITemplateProcessor LoadFrom(Uri localPath)
        {
            return BinarySerializer.FileToObject<ITemplateProcessor>(localPath);
        }

        public static ITemplateProcessor LoadFrom(string base64String)
        {
            return BinarySerializer.Base64StringToObject<ITemplateProcessor>(base64String);
        }
    }
}
