**Project Description**
Texticize is an extensible and intuitive  object-to-text template engine for .NET. You can use Texticize to quickly create dynamic e-mails, letters, source code, or any other text documents using predefined text templates substituting placeholders with properties of CLR objects in realtime.

Texticize is really easy to use.  Simply create a template with place-holders, create one or more maps that specify how each placeholder should be rendered, set one or more variables, and call the Process method.  Consider the following simple example:

{code:c#}
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
{code:c#}

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

Checkout [Documentation](http://texticize.codeplex.com/documentation) for more info and examples.