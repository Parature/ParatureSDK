# Microsoft ParatureSDK for .NET
The Microsoft ParatureSDK for .NET allows you to integrate Parature with other systems to create amazing Agent and Customer experiences. It is a C# library which abstracts the [Parature REST API](https://support.parature.com/public/doc/api.html) to C# Classes. 

## Contribute
Please see the separate documentation on [contributing](CONTRIBUTING.md) to the ParatureSDK.

## Features
The ParatureAPI library is merely a layer over the Parature REST API, so no new functionality from the API's perspective is introduced. It does simplify use of the API in many ways:

* Class definitions with static fields for the following Entities: Account, Product, Asset, Chat, Csr, Customer, Ticket, Asset, Download, Article
* Built-in throttling to prevent [API server throttle limits](https://support.parature.com/public/doc/api.html#limits)
* Simple programmatic access of XML from all requests and responses (useful for reference)
* Create, update, delete, or retrieve all entities
* Classes for building complex queries

## Semantic Versioning
The latest minor version of the SDK, 2.2, introduces a move to [semantic versioning](http://semver.org), which will guide our version numbering and release standard going forward.

As such, backwards-incompatible changes have been introduced in this release alongside the old, backwards-compatible functionality. The exercises have been updated to the new way of calling the SDK, and deprecations have been added to all classes and public functions that will be removed in the next major version, 3.0.0.

The deprecations that have been added will result in compiler warnings, but will still allow your existing code to build and run without changes. The warning messages will tell you what class/method to call in the new functionality in order to facilitate smooth upgrading of existing code. In v3.0.0, these deprecations will be changed from warnings to errors, disallowing building using the old methods from 3.0 onward. In future releases, the old code will be removed altogether.    

## NuGet
We are available on [NuGet](https://www.nuget.org/packages/ParatureSDK/)!
To install using the console:
```
PM> Install-Package ParatureSDK
```

# Sample Usage
Connection information is stored in an instantiated ParaCredentials object, which is then passed into the ParaService object for use. Instantiating is easy:

```
//this would be the domain of your server farm where you log in to the service desk
string serverUrl = "https://demo.parature.com"; 
bool enforceRequiredFields = true;
// Find the following under the setup tab of the Service Desk
// Csr must have permissions to use the API
string token = "#################################";
int accountId = ####;
int deptId = ####;

var creds = new ParaCredentials(token, serverUrl, 
            Paraenums.ApiVersion.v1, accountId, deptId, false);
var paraService = new ParaService(creds);
```

### Retrieving Entities from the Server
Retrieval is quite simple. Using the new "creds" object, you use the ParaService class to make requests from the server.

```
//This will return the first 25 tickets in the Server, unordered
//It is functionally identical to a GET to:
//          https://{hostname}/api/v1/{account id}/{department id}/Ticket
var tickets = paraService.GetList<Ticket>();
```

Queries can be filtered as well. For example, retrieving customers based off of their email is simple:

```
var email = "test@example.com";
var customerQuery = new ModuleQuery.CustomerQuery();
customerQuery.RetrieveAllRecords = true; //automatically retrieve ALL data that match filter
customerQuery.AddStaticFieldFilter(ModuleQuery.CustomerQuery.CustomerStaticFields.CustomerEmail, Paraenums.QueryCriteria.Equal, email);

var customers = paraService.GetList<Customer>(customerQuery);
```

Note the option to iteratively retrieve all records. It will default to the page size of 25 entities per GET.

### Entity Schema
Parature Entities support custom fields to be specified within the Service Desk. This is dynamically retrievable as well, similar to the /Schema call in the REST API.

```
var emptyTicket = paraService.Create<Ticket>();
var customFields = emptyTicket.CustomFields; //all custom fields

var requiredFields = emptyTicket.CustomFields
    .Where(f => f.Required); //all custom fields which are required

//Field Metadata also available for custom fields
var textFields = emptyTicket.CustomFields
    .Where(f => f.DataType == "string");

var fieldLengths = emptyTicket.CustomFields
    .Select(f => f.MaxLength)
    .ToList();
```

### Field Retrieval and Modification
All entities inherit the base class "ParaEntity", which provided indexers for static and custom fields. 

```
// typecase not required, but demonstrates the inheritance
var entity = ticket as ParaEntity; 

//Static Fields don't have ids, but do have unique names. 
//Retrieve with an indexer by specifying the schema name
var dateCreated = entity["Date_Created"].GetFieldValue<DateTime>();

//Custom Fields all have unique ids, and are retrieved via the integer id
var summaryField = entity[22];
//Custom Field values are all represented as strings or a list of FieldOptions.
//As such, they don't need to use the typecast convenience method GetFieldValue<T>
var summaryText = summaryField.Value;
```

Custom Fields can have options instead of a string value. There are convenience methods for working with option sets.
```
//Field Options are stored as a list for custom fields.
var fieldsWithOptions = entity.CustomFields
                                   .Where(cf => cf.Options.Count > 0);

//Retrieving selected options
var selectedOptions = entity.First().GetSelectedOptions();

//Changing (or adding if multi value) the selected option
var newSelectedOptionId = 1;
var resetOtherOptions = true; //needed if this is a multi-value dropdown 
fieldsWithOptions.First().SetSelectedOption(newSelectedOptionId, resetOtherOptions);
```

### Entity Relationships
Parature Entities have hard-coded relationships to other Entities. There are no custom relationships in the API. This is reflected in the ParatureSDK library as Class Properties.

```
var customer = paraService.GetList<Customer>().Data.First();
ticketSchema.Ticket_Customer = new CustomerReference() {
    Entity = customer
};
```

### Entity Creation/Update
The ParaService class has methods for different HTTP methods (GET, PUT, POST, DELETE), Get, Update, Insert, and Delete respectively. They are each very similar, and should be self-explanatory.

```
//create the ticket
var response = paraService.Insert(paraService.Create<Ticket>());
if (response.HasException)
{
    //something failed
    Console.WriteLine(resp.ExceptionDetails);
}
```

Please note that all API requests are synchronous and do block the current thread.