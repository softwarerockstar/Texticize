using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    [Flags]
    public enum TemplateSaveOptions
    {
        None = 0x0,
        PreFetchIncludes = 0x1
    }
}
