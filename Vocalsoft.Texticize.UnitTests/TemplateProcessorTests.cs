//-----------------------------------------------------------------------
// <copyright author="Muhammad Haroon">
//      Texticize
//      Codeplex Project: http://texticize.codeplex.com/
//      Copyright (c) Muhammad Haroon, http://www.softwarerockstar.com/
//      Released under Apache License Version 2.0, http://www.apache.org/licenses/      
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Data;
using Vocalsoft.Texticize.UnitTests.DTO;
using System.Diagnostics;

namespace Vocalsoft.Texticize.UnitTests
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
            string template = "Your age is {Age}";
            string toCompare = "Your age is 21";

            string result = new TemplateProcessor(template)
                .CreateMap("{Age}", s => "21")
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        public void PlainStringDictionaryTest()
        {
            Dictionary<string, string> testData = new Dictionary<string, string>
            {
                {"Name", "John Doe"},
                {"Age", "27"}
            };

            string template = "My name is {Name}. I am {Age} years old.";

            string result = new TemplateProcessor(template)
                .CreateMap<Dictionary<string, string>>("{Name}", s => s.Variable["Name"])
                .CreateMap<Dictionary<string, string>>("{Age}", s => s.Variable["Age"])
                .SetVariable<Dictionary<string, string>>(testData)
                .Process();

            Assert.AreEqual<string>(result, "My name is John Doe. I am 27 years old.");
        }
        
        [TestMethod]
        public void SingleVariableDateTest()
        {
            string template = "{MyDate!Yesterday} is history, {MyDate!Tomorrow} is a mystery. {MyDate!Today} is a gift, that's why it's called Present.";

            string result = new TemplateProcessor(template)                
                .CreateMap<DateTime>("{MyDate!Today}", s => s.Variable.ToShortDateString())
                .CreateMap<DateTime>("{MyDate!Yesterday}", s => s.Variable.AddDays(-1).ToShortDateString())
                .CreateMap<DateTime>("{MyDate!Tomorrow}", s => s.Variable.AddDays(1).ToShortDateString())
                .SetVariable<DateTime>("MyDate", DateTime.Now)
                .Process();

            var toCompare = String.Format("{0} is history, {1} is a mystery. {2} is a gift, that's why it's called Present.",
                DateTime.Today.AddDays(-1).ToShortDateString(),
                DateTime.Today.AddDays(1).ToShortDateString(),
                DateTime.Today.ToShortDateString());

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        public void MultiVariableTest()
        {
            string template = "Dear Mr. {Customer!LastName}:\nYour order # {Order!OrderID} has been received. Your order total is: ${Order!OrderTotal}.";

            string result = new TemplateProcessor(template)
                .CreateMap<CustomerDto>("{Customer!LastName}", s => s.Variable.LastName)
                .CreateMap<OrderDto>("{Order!OrderID}", s => s.Variable.OrderID.ToString())
                .CreateMap<OrderDto>("{Order!OrderTotal}", s => s.Variable.Total.ToString())

                .SetVariable<CustomerDto>("Customer", _customers[0])
                .SetVariable<OrderDto>("Order", _orders[0])
                
                .Process();

            Assert.AreEqual<string>(result, "Dear Mr. Doe:\nYour order # 1 has been received. Your order total is: $275.49.");
        }

        [TestMethod]
        public void ConditionalTest()
        {
            string template = "Price for 15MP Camera is {Product!Price[Description=15MP Camera]}.";

            string result = new TemplateProcessor(template)
                
                .CreateMap<List<ProductDto>>(@"{Product!Price}",
                    s => s.Variable.Lookup(
                        condition: q => q.Description == s.Parameters["Description"],
                        value: u => u.Price.ToString("C")
                    )
                )
                
                .SetVariable<List<ProductDto>>("Product", _products)
                .Process();

            Assert.AreEqual<string>(result, "Price for 15MP Camera is $150.29.");

        }

        [TestMethod]        
        public void LoopingTest()
        {
            string template = @"Following products are currently in inventory<br/> {Products!List[ColBegin=<td>,ColEnd=</td>,RowBegin=<tr>,RowEnd=</tr>]}";
            string toCompare = "Following products are currently in inventory<br/> <tr><td>50MP Camera</td><td>$150.29</td></tr><tr><td>20MP Camera</td><td>$150.29</td></tr><tr><td>15MP Camera</td><td>$150.29</td></tr><tr><td>12MP Camera</td><td>$150.29</td></tr><tr><td>10MP Camera</td><td>$150.29</td></tr>";

            string result = new TemplateProcessor(template)

                .CreateMap<List<ProductDto>>("{Products!List}",
                    s => s.Variable.ToDelimitedText(
                        columns: new Func<ProductDto, string>[] { q => q.Description, q => q.Price.ToString("C") },
                        colBeginDelimiter: s.Parameters["ColBegin"],
                        colEndDelimiter: s.Parameters["ColEnd"],
                        rowBeginDelimiter: s.Parameters["RowBegin"],
                        rowEndDelimiter: s.Parameters["RowEnd"]
                    )
                )

                .SetVariable<List<ProductDto>>("Products", _products)
                .Process();
            
            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Macros")]
        public void DateTimeMacroTest()
        {
            string template = "Today is %DateTime%.  Right not it is %DateTime T%";
            string toCompare = String.Format("Today is {0}.  Right not it is {1}", DateTime.Now.ToString("d"), DateTime.Now.ToString("T"));
            
            string result = new TemplateProcessor(template)
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Macros")]
        public void NewLineMacroTest()
        {
            string template = "Today is %DateTime%.%NewLine%Right not it is %DateTime T%";
            string toCompare = String.Format(
                "Today is {0}.{1}Right not it is {2}", 
                DateTime.Now.ToString("d"), System.Environment.NewLine, 
                DateTime.Now.ToString("T"));

            string result = new TemplateProcessor(template)
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Macros")]
        public void UserNameMacroTest()
        {
            string template = @"The domain and user name of logged on person is %UserDomainName%\%UserName%";
            string toCompare = String.Format(
                @"The domain and user name of logged on person is {0}\{1}", 
                System.Environment.UserDomainName, 
                System.Environment.UserName);

            string result = new TemplateProcessor(template)
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void ConditionalTestWithConfiguration()
        {
            string template = "Price for 15MP Camera is <Product.Price(Description=15MP Camera)>.";

            string result = new TemplateProcessor(
                template,
                new Configuration
                {
                    PropertySeperator = '.',
                    TemplateRegexParamBeginChar = '(',
                    TemplateRegexParamEndChar = ')'
                })

                .CreateMap<List<ProductDto>>(@"<Product.Price>",
                    s => s.Variable.Lookup(
                        condition: q => q.Description == s.Parameters["Description"],
                        value: u => u.Price.ToString("C")
                    )
                )

                .SetVariable<List<ProductDto>>("Product", _products)

                .Process();

            Assert.AreEqual<string>(result, "Price for 15MP Camera is $150.29.");
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void DateTimeMacroTestWithConfiguration()
        {
            string template = "Today is @DateTime^.  Right not it is @DateTime T^";
            string toCompare = String.Format("Today is {0}.  Right not it is {1}", DateTime.Now.ToString("d"), DateTime.Now.ToString("T"));

            string result = new TemplateProcessor(template, new Configuration { MacroRegexPatternBeginChar = '@', MacroRegexPatternEndChar = '^' })
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }


        [TestMethod]
        [TestCategory("Configuration")]
        public void ProcessPipelineRemoveTest()
        {
            string template = "[MyDate!Today] is a %DateTime MM/dd/yyyy%.";
            string toCompare = String.Format("[MyDate!Today] is a {0}.", DateTime.Now.ToString("MM/dd/yyyy"));

            // Remove vocabulary processor so that only macros will be processed
            Configuration config = new Configuration();
            config.ProcessorPipeline.Remove(SystemProcessors.Vocabulary);

            string result = new TemplateProcessor(template, config)            
                .CreateMap<DateTime>("[MyDate!Today]", s => s.Variable.ToShortDateString())
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void ProcessPipelineAddTest()
        {
            string template = "[MyDate!Today] is a %DateTime MM/dd/yyyy%.";
            string toCompare = String.Format("I am tested: {0} is a {1}.", DateTime.Now.ToShortDateString(), DateTime.Now.ToString("MM/dd/yyyy"));

            // Remove vocabulary processor so that only macros will be processed
            Configuration config = new Configuration();
            config.ProcessorPipeline.Add("Test");

            string result = new TemplateProcessor(template, config)
                .CreateMap<DateTime>("[MyDate!Today]", s => s.Variable.ToShortDateString())
                .SetVariable<DateTime>("MyDate", DateTime.Now)
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void ExceptionTest()
        {
            int i = 0;
            string template = "{Infitinity}";

            var processor = new TemplateProcessor(template);

            processor
            .CreateMap("{Infitinity}", s => (2 / i).ToString())     // This line should error out
            .Process();

            Assert.AreEqual<bool>(processor.IsSuccess, false);
            Assert.AreEqual<int>(processor.Exceptions.Count, 1);
        }


        [TestMethod]
        [TestCategory("Serialization")]
        public void SaveLoadTest()
        {
            string template = @"Following products are currently in inventory<br/> {Products!List[ColBegin=<td>,ColEnd=</td>,RowBegin=<tr>,RowEnd=</tr>]}";
            string toCompare = "Following products are currently in inventory<br/> <tr><td>50MP Camera</td><td>$150.29</td></tr><tr><td>20MP Camera</td><td>$150.29</td></tr><tr><td>15MP Camera</td><td>$150.29</td></tr><tr><td>12MP Camera</td><td>$150.29</td></tr><tr><td>10MP Camera</td><td>$150.29</td></tr>";
            Uri localPath = new Uri(@"C:\Users\MH\Documents\Temp\templateProcessor.bin");

            new TemplateProcessor(template)

                .CreateMap<List<ProductDto>>("{Products!List}",
                    s => s.Variable.ToDelimitedText(
                        columns: new Func<ProductDto, string>[] { q => q.Description, q => q.Price.ToString("C") },
                        colBeginDelimiter: s.Parameters["ColBegin"],
                        colEndDelimiter: s.Parameters["ColEnd"],
                        rowBeginDelimiter: s.Parameters["RowBegin"],
                        rowEndDelimiter: s.Parameters["RowEnd"]
                    )
                )
                .Save(localPath);

            var result = TemplateProcessor.LoadFrom(localPath)
                .SetVariable<List<ProductDto>>("Products", _products)
                .Process();

            Assert.AreEqual<string>(result, toCompare);
        }

        [TestMethod]
        [TestCategory("Macro")]
        public void IncludeTest()
        {
            string template = @"%Include C:\Users\MH\Documents\Temp\Level1.txt%";
            string toCompare = @"Level1 Level2";

            var result = new TemplateProcessor(template)
                .Process();

            Assert.AreEqual<string>(result, toCompare);

        }

        //[TestMethod]
        //public void PlainStringDictionaryArrayMapTest()
        //{
        //    Dictionary<string, string> testData = new Dictionary<string, string>
        //    {
        //        {"Name", "John Doe"},
        //        {"Age", "27"}
        //    };

        //    string template = "My name is {Name}. I am {Age} years old.";

        //    Dictionary<string, Func<Context<int>, string>> ff = {{"Name", s => s.Variable.ToString()}, {"Val", s => s.Variable.ToString()}};

        //    string result = new TemplateProcessor(template)
        //        .CreateMaps<Dictionary<string, string>>(
        //            {"Name", s => s.Variable["Name"]}
        //    );
                    

        //        //.CreateMap<Dictionary<string, string>>("{Age}", s => s.Variable["Age"])
        //        //.SetVariable<Dictionary<string, string>>(testData)
        //        //.Process();

        //    Assert.AreEqual<string>(result, "My name is John Doe. I am 27 years old.");
        //}


    }
}
