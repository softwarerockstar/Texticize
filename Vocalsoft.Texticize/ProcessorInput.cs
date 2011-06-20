using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize
{
    [Serializable]
    public class ProcessorInput
    {
        public string Target { get; set; }
        public Configuration Configuration { get; set; }
        public Dictionary<string, Delegate> Maps { get; set; }
        public Dictionary<string, object> Variables { get; set; }        

        public ProcessorInput(Configuration configuration)
        {
            Maps = new Dictionary<string, Delegate>();
            Variables = new Dictionary<string, object>();
            Configuration = configuration;
        }
    }
}
