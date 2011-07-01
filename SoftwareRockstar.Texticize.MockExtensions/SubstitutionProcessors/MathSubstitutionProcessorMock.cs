//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System.ComponentModel.Composition;
using SoftwareRockstar.ComponentModel.Extensibility;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SoftwareRockstar.Texticize.MockExtensions
{
    [Export(typeof(ISubstitutionProcessor))]
    [ExportMetadata(UniquenessEvidenceFields.UniqueName, "SoftwareRockstar.Texticize.MockExtensions.MathSubstitutionProcessorMock")]    
    public class MathSubstitutionProcessorMock : ISubstitutionProcessor
    {
        /// <summary>
        /// Processes macros and performs substitutions
        /// </summary>
        public ProcessorOutput Process(ProcessorInput input)
        {
            ProcessorOutput output = new ProcessorOutput { Result = input.Target };

            if (input != null && !String.IsNullOrEmpty(input.Target))
            {
                try
                {
                    Regex regex = new Regex(@"\[(?:\s?)+(?<op1>[0-9]+)(?:\s?)+(?<opr>\+|\-|\*|\/)(?:\s?)+(?<op2>[0-9]+)(?:\s?)+\]");
                    var match = regex.Match(input.Target);                    

                    decimal operand1 = 0;
                    decimal operand2 = 0;

                    Decimal.TryParse(match.Groups["op1"].Value, out operand1);
                    Decimal.TryParse(match.Groups["op2"].Value, out operand2);

                    switch (match.Groups["opr"].Value[0])
                    {
                        case '/':
                            output.Result = regex.Replace(output.Result, (operand1 / operand2).ToString());
                            break;
                        case '-':
                            output.Result = regex.Replace(output.Result, (operand1 - operand2).ToString());
                            break;
                        case '*':
                            output.Result = regex.Replace(output.Result, (operand1 * operand2).ToString());
                            break;
                        case '+':
                            output.Result = regex.Replace(output.Result, (operand1 + operand2).ToString());
                            break;
                    }

                }
                catch (ApplicationException ex)
                {
                    output.IsSuccess = false;
                    output.Exceptions.Add(ex);
                }
            }
            return output;
        }
    }
}
