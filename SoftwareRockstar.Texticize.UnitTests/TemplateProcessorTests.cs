﻿//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareRockstar.Texticize.TemplateReaders;
using SoftwareRockstar.Texticize.UnitTests.DTO;

namespace SoftwareRockstar.Texticize.UnitTests
{
    [TestClass]
    public class TemplateProcessorTests
    {
        List<CustomerDto> _customers;
        List<OrderDto> _orders;
        List<ProductDto> _products;

        [TestInitialize]
        public void Init()
        {
            _customers = new List<CustomerDto>
            {
                new CustomerDto
                {
                    CustomerID = 1,
                    FirstName = "John",
                    LastName = "Doe"
                },
                new CustomerDto
                {
                    CustomerID = 2,
                    FirstName = "Jessie",
                    LastName = "Doe"
                }
            };

            _orders = new List<OrderDto>
            {
                new OrderDto
                {
                    OrderID = 1,
                    CustomerID = 1,
                    SubTotal = 250.49M,
                    Tax = 25M,
                    Total = 275.49M
                     
                },
                new OrderDto
                {
                    OrderID = 2,
                    CustomerID = 1,
                    SubTotal = 200.20M,
                    Tax = 20M,
                    Total = 220.20M
                     
                }
            };

            _products = new List<ProductDto>
            {
                new ProductDto
                {
                    ProductID=1,
                    Description = "50MP Camera",
                    Price = 150.29M
                },
                new ProductDto
                {
                    ProductID=2,
                    Description = "20MP Camera",
                    Price = 150.29M
                },
                new ProductDto
                {
                    ProductID=3,
                    Description = "15MP Camera",
                    Price = 150.29M
                },
                new ProductDto
                {
                    ProductID=4,
                    Description = "12MP Camera",
                    Price = 150.29M
                },
                new ProductDto
                {
                    ProductID=5,
                    Description = "10MP Camera",
                    Price = 150.29M
                }
            };
        }


        [TestMethod]
        public void SimpleNonGenericTest()
        {
            var reader = TemplateReaderFactory.CreateStringTemplateReader("Your age is {Age}");
            string toCompare = "Your age is 21";

            string result = TemplateProcessorFactory
                .CreateDefault(reader)
                .SetMaps("{Age}".MapTo(s => "21"))
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        public void PlainStringDictionaryTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("My name is {Name}. I am {Age} years old.");
            string toCompare = "My name is John Doe. I am 27 years old.";

            Dictionary<string, string> testData = new Dictionary<string, string>
            {
                {"Name", "John Doe"},
                {"Age", "27"}
            };
            
            string result = TemplateProcessorFactory
				.CreateDefault(reader)               
                .SetMaps
                (
                    "{Name}".MapTo<Dictionary<string, string>>(s => s.Variable["Name"]),
                    "{Age}".MapTo<Dictionary<string, string>>(s => s.Variable["Age"])
                )
                .SetVariable(testData)                
                .Process().Result;

            Assert.AreEqual<string>(result, toCompare);
        }
        
        [TestMethod]
        public void SingleVariableDateTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("{MyDate!Yesterday} is history, {MyDate!Tomorrow} is a mystery. {MyDate!Today} is a gift, that's why it's called Present.");
            var toCompare = String.Format("{0} is history, {1} is a mystery. {2} is a gift, that's why it's called Present.",
                DateTime.Today.AddDays(-1).ToShortDateString(),
                DateTime.Today.AddDays(1).ToShortDateString(),
                DateTime.Today.ToShortDateString());

            string result = TemplateProcessorFactory
				.CreateDefault(reader)               
                .SetMaps
                (
                    "{MyDate!Today}".MapTo<DateTime>(s => s.Variable.ToShortDateString()),
                    "{MyDate!Yesterday}".MapTo<DateTime>(s => s.Variable.AddDays(-1).ToShortDateString()),
                    "{MyDate!Tomorrow}".MapTo<DateTime>( s => s.Variable.AddDays(1).ToShortDateString())
                )
                .SetVariables("MyDate".ToVariable(DateTime.Now))
                .Process().Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        public void MultiVariableTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("Dear Mr. {Customer!LastName}:\nYour order # {Order!OrderID} has been received. Your order total is: ${Order!OrderTotal}.");

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .SetMaps
                (
                    "{Customer!LastName}".MapTo<CustomerDto>(s => s.Variable.LastName),
                    "{Order!OrderID}".MapTo<OrderDto>(s => s.Variable.OrderID.ToString()),
                    "{Order!OrderTotal}".MapTo<OrderDto>(s => s.Variable.Total.ToString())
                )
                .SetVariables
                (
                    "Customer".ToVariable(_customers[0]),
                    "Order".ToVariable(_orders[0])
                )                
                .Process()
                .Result;

            Assert.AreEqual<string>(result, "Dear Mr. Doe:\nYour order # 1 has been received. Your order total is: $275.49.");
        }

        [TestMethod]
        public void ConditionalTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("Price for 15MP Camera is {Product!Price[Description = 15MP Camera]}.");
            string toCompare = "Price for 15MP Camera is $150.29.";

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .SetMaps
                (
                    "{Product!Price}".MapTo<List<ProductDto>>
                    (
                        s => s.Variable.Lookup(
                            condition: q => q.Description == s.Parameters["Description"],
                            value: u => u.Price.ToString("C"))
                    )
                )                
                .SetVariables("Product".ToVariable(_products))
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]        
        public void LoopingTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"Following products are currently in inventory<br/> {Products!List[ColBegin=<td>,ColEnd=</td>,RowBegin=<tr>,RowEnd=</tr>]}");
            string toCompare = "Following products are currently in inventory<br/> <tr><td>50MP Camera</td><td>$150.29</td></tr><tr><td>20MP Camera</td><td>$150.29</td></tr><tr><td>15MP Camera</td><td>$150.29</td></tr><tr><td>12MP Camera</td><td>$150.29</td></tr><tr><td>10MP Camera</td><td>$150.29</td></tr>";

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .SetMaps(
                    "{Products!List}".MapTo<List<ProductDto>>(
                        s => s.Variable.ToStructuredText(
                            columns: new Func<ProductDto, string>[] { q => q.Description, q => q.Price.ToString("C") },
                            colBeginDelimiter: s.Parameters["ColBegin"],
                            colEndDelimiter: s.Parameters["ColEnd"],
                            rowBeginDelimiter: s.Parameters["RowBegin"],
                            rowEndDelimiter: s.Parameters["RowEnd"]
                        )
                    )
                )
                .SetVariables("Products".ToVariable(_products))
                .Process()
                .Result;
            
            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Macros")]
        public void DateTimeMacroTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("Today is %DateTime%.  Right not it is %DateTime T%");
            string toCompare = String.Format("Today is {0}.  Right not it is {1}", DateTime.Now.ToString("d"), DateTime.Now.ToString("T"));

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Macros")]
        public void NewLineMacroTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("Today is %DateTime%.%NewLine%Right not it is %DateTime T%");
            string toCompare = String.Format(
                "Today is {0}.{1}Right not it is {2}", 
                DateTime.Now.ToString("d"), System.Environment.NewLine, 
                DateTime.Now.ToString("T"));

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Macros")]
        public void UserNameMacroTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"The domain and user name of logged on person is %UserDomainName%\%UserName%");
            string toCompare = String.Format(
                @"The domain and user name of logged on person is {0}\{1}", 
                System.Environment.UserDomainName, 
                System.Environment.UserName);

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void ConditionalTestWithConfiguration()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("Price for 15MP Camera is <Product.Price(Description=15MP Camera)>.");


            string result = TemplateProcessorFactory
				.CreateDefault(reader)                                
                .SetConfiguration(
                    new Configuration
                    {
                        PropertySeperator = '.',
                        TemplateRegexParamBeginChar = '(',
                        TemplateRegexParamEndChar = ')'
                    })
                .SetMaps
                (
                    "<Product.Price>".MapTo<List<ProductDto>>
                    (
                        s => s.Variable.Lookup(
                            condition: q => q.Description == s.Parameters["Description"],
                            value: u => u.Price.ToString("C")
                        )
                    )
                )
                .SetVariables("Product".ToVariable(_products))
                .Process()
                .Result;

            Assert.AreEqual<string>(result, "Price for 15MP Camera is $150.29.");
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void DateTimeMacroTestWithConfiguration()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("Today is @DateTime^.  Right not it is @DateTime T^");
            string toCompare = String.Format("Today is {0}.  Right not it is {1}", DateTime.Now.ToString("d"), DateTime.Now.ToString("T"));

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .SetConfiguration
                (
                    new Configuration 
                    { 
                        MacroRegexPatternBeginChar = '@', 
                        MacroRegexPatternEndChar = '^' 
                    }
                )
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }


        [TestMethod]
        [TestCategory("Configuration")]
        public void ProcessPipelineRemoveTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("[MyDate!Today] is a %DateTime MM/dd/yyyy%.");
            string toCompare = String.Format("[MyDate!Today] is a {0}.", DateTime.Now.ToString("MM/dd/yyyy"));

            // Remove vocabulary processor so that only macros will be processed
            Configuration config = new Configuration();
            config.ProcessorPipeline.Remove(SystemSubstitutionProcessorNames.Vocabulary);

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .SetConfiguration(config)
                .SetMaps("[MyDate!Today]".MapTo<DateTime>(s => s.Variable.ToShortDateString()))            
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void ProcessPipelineAddTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("[MyDate!Today] is a %DateTime MM/dd/yyyy%.");
            string toCompare = String.Format("I am tested: {0} is a {1}.", DateTime.Now.ToShortDateString(), DateTime.Now.ToString("MM/dd/yyyy"));

            // Add custom processor to the end of the pipeline
            Configuration config = new Configuration();
            config.ProcessorPipeline.Add("Test");

            string result = TemplateProcessorFactory
				.CreateDefault(reader)                
                .SetConfiguration(config)
                .SetMaps("[MyDate!Today]".MapTo<DateTime>(s => s.Variable.ToShortDateString()))
                .SetVariables("MyDate".ToVariable(DateTime.Now))
                .Process()
                .Result;                
           
            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void ExceptionTest()
        {
            int i = 0;
            var reader =  TemplateReaderFactory.CreateStringTemplateReader("{Infitinity}");

            var processor = TemplateProcessorFactory
                .CreateDefault(reader);

            var output = processor
                .SetMaps("{Infitinity}".MapTo(s => (2 / i).ToString()))     // This line should error out
                .Process();

            Assert.AreEqual<bool>(output.IsSuccess, false);
            Assert.AreEqual<int>(output.Exceptions.Count, 1);
        }


        [TestMethod]
        [TestCategory("Serialization")]
        public void SaveLoadTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"Following products are currently in inventory<br/> {Products!List[ColBegin=<td>,ColEnd=</td>,RowBegin=<tr>,RowEnd=</tr>]}");
            string toCompare = "Following products are currently in inventory<br/> <tr><td>50MP Camera</td><td>$150.29</td></tr><tr><td>20MP Camera</td><td>$150.29</td></tr><tr><td>15MP Camera</td><td>$150.29</td></tr><tr><td>12MP Camera</td><td>$150.29</td></tr><tr><td>10MP Camera</td><td>$150.29</td></tr>";
            Uri localPath = new Uri(@"C:\Users\MH\Documents\Temp\templateProcessor.bin");

            var processor = TemplateProcessorFactory
                .CreateDefault(reader)                
                .SetMaps
                (
                    "{Products!List}".MapTo <List<ProductDto>>
                    (
                        s => s.Variable.ToStructuredText(
                            columns: new Func<ProductDto, string>[] { q => q.Description, q => q.Price.ToString("C") },
                            colBeginDelimiter: s.Parameters["ColBegin"],
                            colEndDelimiter: s.Parameters["ColEnd"],
                            rowBeginDelimiter: s.Parameters["RowBegin"],
                            rowEndDelimiter: s.Parameters["RowEnd"]
                        )
                    )
                );

            PersistenceManager.Save(processor, localPath);

            var result = PersistenceManager.LoadFromFile(localPath)
                .SetVariables("Products".ToVariable(_products))
                .Process().Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Include")]
        public void IncludeTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"%Include C:\Users\MH\Documents\Temp\Level1.txt%");
            string toCompare = @"Level1 Level2";

            var result = TemplateProcessorFactory
                .CreateDefault(reader)                
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Include")]
        public void LoopingIncludeTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"Following products are currently in inventory%NewLine%{Products!List[ColBegin=<,ColEnd=>,RowEnd=%Include C:\Users\MH\Documents\Temp\NewLine.txt%]}");
            string toCompare = "Following products are currently in inventory\r\n<50MP Camera><$150.29>\r\n\r\n<20MP Camera><$150.29>\r\n\r\n<15MP Camera><$150.29>\r\n\r\n<12MP Camera><$150.29>\r\n\r\n<10MP Camera><$150.29>\r\n\r\n";

            string result = TemplateProcessorFactory
                .CreateDefault(reader)                
                .SetMaps
                (
                    "{Products!List}".MapTo<List<ProductDto>>
                    (
                        s => s.Variable.ToStructuredText(
                            columns: new Func<ProductDto, string>[] { q => q.Description, q => q.Price.ToString("C") },
                            colBeginDelimiter: s.Parameters["ColBegin"],
                            colEndDelimiter: s.Parameters["ColEnd"],
                            rowEndDelimiter: s.Parameters["RowEnd"]
                        )
                    )
                )
                .SetVariables("Products".ToVariable(_products))
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Include")]
        public void SaveTestWithPrefetchIncludesTest()
        {
            var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"%Include C:\Users\MH\Documents\Temp\Level1.txt%");
            string toCompare = @"Level1 Level2";
            Uri localPath = new Uri(@"C:\Users\MH\Documents\Temp\templateProcessor.bin");

            var processor = TemplateProcessorFactory
                .CreateDefault(reader);

            PersistenceManager.Save(processor, localPath, TemplateSaveOptions.PreFetchIncludes);

            var result = PersistenceManager.LoadFromFile(localPath)                
                .Process().Result;

            Assert.AreEqual<string>(result, toCompare);

        }

        [TestMethod]
        [TestCategory("TemplateLoader")]
        public void EmbeddedResourceTemplateLoaderTest()
        {
            var reader = TemplateReaderFactory.CreateEmbeddedResourceTemplateReader(
                        this.GetType().Assembly.GetName().CodeBase,
                        "SoftwareRockstar.Texticize.UnitTests.Resources.TestResource",
                        "Template");

            string toCompare = String.Format("{0} is the date.", DateTime.Now.ToString("MM/dd/yyyy"));

            var result = TemplateProcessorFactory
                .CreateDefault(reader)                
                .Process()
                .Result;            

            Assert.AreEqual<string>(result, toCompare);
        }


        [TestMethod]
        [TestCategory("TemplateLoader")]
        public void FileTemplateLoaderTest()
        {
            var reader = TemplateReaderFactory.CreateFileTemplateReader(@"C:\Users\MH\Documents\Temp\Level1.txt");
            string toCompare = @"Level1 Level2";

            var result = TemplateProcessorFactory
                .CreateDefault(reader)
                .Process()
                .Result;

            Assert.AreEqual<string>(result, toCompare);
        }
    }
}