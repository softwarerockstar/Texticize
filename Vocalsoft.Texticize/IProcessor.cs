﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    public interface IProcessor
    {
        ProcessorOutput Process(ProcessorInput input);
    }
}
