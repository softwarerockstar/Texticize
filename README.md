# Texticize

>Please note that this project started it's life on CodePlex, but since CodePlex is shutting down, it has found it's new home on GitHub.

Texticize is an extensible and intuitive  object-to-text template engine for .NET. You can use Texticize to quickly create dynamic e-mails, letters, source code, or any other text documents using predefined text templates substituting placeholders with properties of CLR objects in realtime.

Texticize is really easy to use.  Simply create a template with place-holders, create one or more maps that specify how each placeholder should be rendered, set one or more variables, and call the Process method.  Consider the following simple example:

```c#
var reader = TemplateReaderFactory.CreateStringTemplateReader("{MyDate!Today} is a gift, that's why it's called Present.");

string result = TemplateProcessorFactory
    .CreateDefault(reader)
    .SetMaps
    (
        "MyDate!Today".MapTo<DateTime>(s => s.Variable.ToShortDateString())
    )
    .SetVariables("MyDate".ToVariable(DateTime.Now))
    .Process()
    .Result;
```

### Features
Texticize is a complete template engine with a lot of features.  Following is a short list of such features:

* Fluid interface
* Lambda expressions, anonymous methods, or regular methods specify replacements
* Support for conditionals
* Support for loops
* Support for multiple variables
* Default vocabulary can easily be overridden; developers can define their own vocabulary
* Support for Include files
* Support for Macros
* Completely extensible design

## Loading Templates
Templates can be loaded in a number of ways including, in-memory objects, files, or embedded resources.  You can also easily extend Texticize to load templates from other sources.

```c#
// Load template from a string object
var stringReader = TemplateReaderFactory.CreateStringTemplateReader("Your order number is {OrderNumber}");

// Load template from a file
var fileReader = TemplateReaderFactory.CreateFileTemplateReader(@"C:\Documents\Template1.txt");

// Load template from an embedded resource
var resourceReader = TemplateReaderFactory.CreateEmbeddedResourceTemplateReader(
                                              this.GetType().Assembly.GetName().CodeBase,
                                              "Resource.Namespace",
                                              "ResourceName");
```

## Simple Substitutions
Texticize can perform from the most basic to the most complex template processing.  For example, let's look at the following substitution:

```c#
var reader = TemplateReaderFactory.CreateStringTemplateReader("Your order number is {OrderNumber}");
string toCompare = "Your order number is 00201767";

string result = TemplateProcessorFactory
    .CreateDefault(reader)
    .SetMaps("OrderNumber".MapTo(s => "00201767"))
    .Process()
    .Result;

Assert.AreEqual<string>(toCompare, result);
```

## Auto Dictionary Substitutions
Texticize can also auto processes any object that implements IDictionary inferring the vocabulary from dictionary keys and subsitution values from dictionary values.

```c#
var reader = TemplateReaderFactory.CreateStringTemplateReader("Your order number is {OrderNumber}. Your order total is ${OrderTotal}.");
string toCompare = "Your order number is 00201767. Your order total is $147.99.";

Dictionary<string, string> testData = new Dictionary<string, string>
{
    {"OrderNumber", "00201767"},
    {"OrderTotal", "147.99"}
};

string result = TemplateProcessorFactory
    .CreateDefault(reader)
    .SetDefaultVariable(testData)
    .Process()
    .Result;

Assert.AreEqual<string>(toCompare, result);
```

## Using Maps for Complex Substitutions
For complex object mappings, you can specify lambda expressions, anonymous methods, or regular methods that specify what the placeholders will be replaced with.  You can specify any number of maps, each mapping to a different object type.  Maps correspond to a variables, and at run-time methods on variables are invoked to obtain substitution values.

```c#
var reader = TemplateReaderFactory.CreateStringTemplateReader("MyDate!Today} is a gift, that's why it's called Present.");
var toCompare = String.Format("{0} is a gift, that's why it's called Present.", DateTime.Today.ToShortDateString());

string result = TemplateProcessorFactory
    .CreateDefault(reader)
    .SetMaps
    (
        "MyDate!Today".MapTo<DateTime>(s => s.Variable.ToShortDateString())
    )
    .SetVariables("MyDate".ToVariable(DateTime.Now))
    .Process()
    .Result;

Assert.AreEqual<string>(toCompare, result);
```

## Conditional Templates
Templates can contain conditions or lookups.  For example, a template can specify to render a property of a particular item in a list.

```c#
var reader =  TemplateReaderFactory.CreateStringTemplateReader("Price for 15MP Camera is {Product!Price[Description = 15MP Camera](Description-=-15MP-Camera)}.");
string toCompare = "Price for 15MP Camera is $150.29.";

string result = TemplateProcessorFactory
	.CreateDefault(reader)                
    .SetMaps
    (
        "Product!Price".MapTo<List<ProductDto>>
        (
            s => s.Variable.Lookup(
                condition: q => q.Description == s.Parameters["Description"](_Description_),
                value: u => u.Price.ToString("C"))
        )
    )                
    .SetVariables("Product".ToVariable(_products))
    .Process()
    .Result;

Assert.AreEqual<string>(toCompare, result);
```

## Structured Text
Texticize can easily render structured text (XML, HTML, CSV, etc.).  The best part is that the template itself can specify how an item should be rendered by specifying parameters for row and column delimiters.

For example, the following example shows how a template can render a list of products into it's HTML representation:

```c#
var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"Following products are currently in inventory<br/> " +
    "{Products!List[ColSep=</td><td>;RowBegin=<tr><td>;RowEnd=</td></tr>](ColSep=__td__td_;RowBegin=_tr__td_;RowEnd=__td___tr_)}");

string toCompare = "Following products are currently in inventory<br/> " +
    "<tr><td>50MP Camera</td><td>$150.29</td></tr><tr><td>20MP Camera</td><td>$150.29</td></tr>" +
    "<tr><td>15MP Camera</td><td>$150.29</td></tr><tr><td>12MP Camera</td><td>$150.29</td></tr>" +
    "<tr><td>10MP Camera</td><td>$150.29</td></tr>";

string result = TemplateProcessorFactory
	.CreateDefault(reader)                
    .SetMaps(
        "Products!List".MapTo<List<ProductDto>>(
            s => s.Variable.ToStructuredText(
                columns: new Func<ProductDto, string>[]() { q => q.Description, q => q.Price.ToString("C") },
                colSeperator: s.Parameters["ColSep"](_ColSep_),                            
                rowBeginDelimiter: s.Parameters["RowBegin"](_RowBegin_),
                rowEndDelimiter: s.Parameters["RowEnd"](_RowEnd_)
            )
        )
    )
    .SetVariables("Products".ToVariable(_products))
    .Process()
    .Result;
            
Assert.AreEqual<string>(toCompare, result);
```

If the template in the code above is modified with different delimiters, the same list of products can be rendered into various other formats.  For example, the following template will render a Comma Separated Values (CSV) representation of the list of products used in the example above:

```c#
var reader = TemplateReaderFactory.CreateStringTemplateReader(@"Following products are currently in inventory %NewLine%" +
    "{Products!List[ColSep=,;RowBegin=;RowEnd=%NewLine%](ColSep=,;RowBegin=;RowEnd=%NewLine%)}");
```

## Macros
Texticize provides a library of reusable macros that can be used to generate substitution text in a standard way.  In addition to macros provided by Texticize, users can create their own libraries of reusable macros.

```c#
var reader =  TemplateReaderFactory.CreateStringTemplateReader("Dear Mr. Doe:%NewLine%Your order has been received.");
string toCompare = String.Format(
    "Dear Mr. Doe:{0}Your order has been received.", 
    System.Environment.NewLine);

string result = TemplateProcessorFactory
	.CreateDefault(reader)                
    .Process()
    .Result;

Assert.AreEqual<string>(toCompare, result);
```

Macros can have parameters.  For example in the code below the DateTime macro accepts an optional format parameter that maps to [standard .NET DateTime formats](http://msdn.microsoft.com/en-us/library/az4se3k1.aspx).

```c#
var reader =  TemplateReaderFactory.CreateStringTemplateReader("Today is %DateTime%.  Right now it is %DateTime T%");
```

## Includes
Templates can have "include" files.  Each include file can nest other include files as needed.  Include files can contain both static text as well as vocabulary placeholders.

```c#
var reader =  TemplateReaderFactory.CreateStringTemplateReader(@"%Include C:\Documents\Template.txt%");
```

## Configuration
Texticize is completely configurable and does not require any predefined vocabulary.  All aspects of Texticize can be easily configured using the provided Configuration object.  For example, in the default vocabulary macros are surrounded by % character.  You can easily customize this to use any other characters.

```c#
var reader = TemplateReaderFactory.CreateStringTemplateReader("(DateTime MM/dd/yyyy)");
string toCompare = DateTime.Today.ToString("MM/dd/yyyy");

Configuration config = new Configuration { MacroBeginChar = '(', MacroEndChar = ')' };

string result = TemplateProcessorFactory
    .CreateDefault(reader)
    .SetConfiguration(config)
    .Process()
    .Result;

Assert.AreEqual<string>(toCompare, result);
```

## Persistence
Template processor object can be persisted to file and loaded back at a later time.

```c#
var reader =  TemplateReaderFactory.CreateStringTemplateReader("...");

var processor = TemplateProcessorFactory
    .CreateDefault(reader);

// Save to file...
PersistenceManager.Save(processor, @"C:\Documents\TemplateProcessor.bin");

// Load from file...
var newProcessor = PersistenceManager.LoadFromFile(@"C:\Documents\TemplateProcessor.bin"));
```

## Customizing Processor Pipeline
Texticize uses a process pipeline of substitution processors that act upon a given template in succession in order to completely process the template.  By default, first the vocabulary processor acts upon the template.  The output from the vocabulary processor is then sent to the macro processor which substitutes macro placeholders.  You can easily customize the pipeline by adding, removing, or re-ordering processes that act upon your given template.  You can also create custom substitution processors that can be added to the pipeline.

In the example below we remove the macro processor from the pipeline, hence the macro provided in the template is not processed.

```c#
var reader =  TemplateReaderFactory.CreateStringTemplateReader("Today is a %DateTime MM/dd/yyyy%.");
string toCompare = "Today is a %DateTime MM/dd/yyyy%.";

// Remove macro processor so that macros are not processed
Configuration config = new Configuration();
config.ProcessorPipeline.Remove(SystemSubstitutionProcessorNames.Macro);

string result = TemplateProcessorFactory
	.CreateDefault(reader)                
    .SetConfiguration(config)                
    .Process()
    .Result;

Assert.AreEqual<string>(toCompare, result);
```
