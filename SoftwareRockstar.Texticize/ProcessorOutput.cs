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

namespace SoftwareRockstar.Texticize
{
    [Serializable]
    public class ProcessorOutput
    {
        List<Exception> _exceptions;

        public ProcessorOutput(): this(null, true)
        {
        }

        public ProcessorOutput(string result, bool isSuccess)
        {
            _exceptions = new List<Exception>();
            this.Result = result;            
        }

        public ProcessorOutput Combine(ProcessorOutput input)
        {            
            this.Result = input.Result;
            _exceptions.AddRange(input.Exceptions);

            return this;
        }

        public string Result { get; set; }
        public bool IsSuccess { get { return (_exceptions.Count == 0); } }
        public IList<Exception> Exceptions { get {return _exceptions; } } 
    }
}
