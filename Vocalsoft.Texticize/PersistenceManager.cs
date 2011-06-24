//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using Vocalsoft.Serialization;

namespace Vocalsoft.Texticize
{
    public class PersistenceManager
    {
        public static void Save(TemplateProcessor processor, Uri localPath)
        {
            Save(processor, localPath, TemplateSaveOptions.None);
        }

        public static void Save(TemplateProcessor processor, Uri localPath, TemplateSaveOptions options)
        {
            if (options.HasFlag(TemplateSaveOptions.PreFetchIncludes))
            {
                var output = new Processors.MacroProcessor().ProcessMacro(processor.ProcessInput, MacroNames.Include);

                if (output.IsSuccess)
                    processor.ProcessInput.Target = output.Result;
            }

            BinarySerializer.ObjectToFile(localPath, processor);
        }

        public static TemplateProcessor LoadFrom(TemplateProcessor source)
        {
            return BinarySerializer.Base64StringToObject<TemplateProcessor>(BinarySerializer.ObjectToBase64String(source));
        }

        public static TemplateProcessor LoadFrom(Uri localPath)
        {
            return BinarySerializer.FileToObject<TemplateProcessor>(localPath);
        }

        public static TemplateProcessor LoadFrom(string base64String)
        {
            return BinarySerializer.Base64StringToObject<TemplateProcessor>(base64String);
        }
    }
}
