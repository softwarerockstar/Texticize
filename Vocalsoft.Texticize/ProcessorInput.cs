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

namespace Vocalsoft.Texticize
{
    [Serializable]
    public class ProcessorInput
    {
        public string Target { get; set; }
        public Configuration Configuration { get; set; }
        public Dictionary<string, Delegate> Maps { get; private set; }
        public Dictionary<string, object> Variables { get; private set; }        

        public ProcessorInput()
        {
            Maps = new Dictionary<string, Delegate>();
            Variables = new Dictionary<string, object>();
            Configuration = new Configuration();
        }
    }
}
