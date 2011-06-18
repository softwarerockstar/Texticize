using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    public class ProcessorOutput
    {
        List<Exception> _exceptions = new List<Exception>();

        public string Result { get; set; }
        public bool IsSuccess { get; set; }
        public List<Exception> Exceptions { get {return _exceptions; } } 
    }
}
