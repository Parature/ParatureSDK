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

## Version Note
The latest version of the SDK is a large refactor. Many methods have been changed, classes added/removed, and code paths cleaned up. While this provides us a much easier codebase to react to change requests and bugs, we do not recommend this latest release for production-critical deployments. It likewise won't be a drop-in replacement for the old ParatureAPI dll. PLEASE stick with the OSS_R2 branch from GitHub or the 1.0.X packages on NuGet if you can't handle a little cutting edge. 

This branch is the future of this SDK. So please take your development into consideration - eventually the 1.0.X packages and the OSS_RCX branches will be deprecated. 

## NuGet
We are available on [NuGet](https://www.nuget.org/packages/ParatureSDK/)!
To install using the console:
```
PM> Install-Package ParatureSDK
```

# Sample Usage
Connection information is stored in an instantiated ParaCredentials object. Instantiating is easy:

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
```

### Retrieving Entities from the Server
Retrieval is quite simple. Using the new "creds" object, you use the ApiHandler class to make requests from the server.

```
//This will return the first 25 tickets in the Server, unordered
//It is functionally identical to a GET to:
//          https://{hostname}/api/v1/{account id}/{department id}/Ticket
var tickets = ApiHandler.Ticket.GetList(creds);
```

Queries can be filtered as well. For example, retrieving customers based off of their email is simple:

```
var email = "test@example.com";
var customerQuery = new ModuleQuery.CustomerQuery();
customerQuery.RetrieveAllRecords = true; //automatically retrieve ALL data that match filter
customerQuery.AddStaticFieldFilter(ModuleQuery.CustomerQuery.CustomerStaticFields.CustomerEmail, Paraenums.QueryCriteria.Equal, email);

var customers = ApiHandler.Customer.GetList(creds, customerQuery);
```

Note the option to iteratively retrieve all records. It will default to the page size of 25 entities per GET.

### Entity Schema
Parature Entities support custom fields to be specified within the Service Desk. This is dynamically retrievable as well, similar to the /Schema call in the REST API.

```
var ticketSchema = ApiHandler.Ticket.TicketSchema(creds);
var customFields = ticketSchema.CustomFields; //all custom fields

var requiredFields = ticket.CustomFields
    .Where(f => f.Required); //all custom fields which are required

//Field Metadata also available for custom fields
var textFields = ticket.CustomFields
    .Where(f => f.DataType == "string");

var fieldLengths = ticket.CustomFields
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
var customer = ApiHandler.Customer.GetList(creds).Data.First();
ticketSchema.Ticket_Customer = new CustomerReference() {
    Entity = customer
};
```

### Entity Creation/Update
The ApiHandler class has methods for different HTTP methods (GET, PUT, POST, DELETE). They are each very similar, and should be self-explanatory.

```
//create the ticket
var response = ApiHandler.Ticket.Insert(ticketSchema, creds);
if (response.HasException)
{
    //something failed
    Console.WriteLine(resp.ExceptionDetails);
}
```

Please note that all API requests are Synchronous and do block the current thread.