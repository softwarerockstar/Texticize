using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    [Serializable]
    public class ProcessorOutput
    {
        List<Exception> _exceptions;

        public ProcessorOutput(): this(null, false, null)
        {
        }

        public ProcessorOutput(string result, bool isSuccess, IEnumerable<Exception> exceptions)
        {
            _exceptions = new List<Exception>();
            this.Result = result;
            this.IsSuccess = isSuccess;

            if (exceptions != null)
                _exceptions.AddRange(exceptions);
        }
        
        

        public string Result { get; set; }
        public bool IsSuccess { get; set; }
        public List<Exception> Exceptions { get {return _exceptions; } } 
    }
}
