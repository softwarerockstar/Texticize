//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        public IList<Exception> Exceptions { get {return _exceptions; } } 
    }
}
