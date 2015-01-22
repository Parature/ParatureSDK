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

## NuGet
We are available on [NuGet](https://www.nuget.org/packages/ParatureSDK/)!
To install using the console:
```
PM> Install-Package ParatureSDK
```

# Sample Usage
Connection information is stored in an instantiated ParaCredentials object. Instantiating is easy:

```
string serverUrl = "https://demo.parature.com";
bool enforceRequiredFields = true;
// Find the following under the setup tab of the Service Desk
// Csr must have permissions to use the API
string token = "#################################";
int accountId = "####";
int deptId = "####";

var creds = new ParaCredentials(token, serverUrl, 
            Paraenums.ApiVersion.v1, AcctID, DeptID, false);
```

### Retrieving Entities from the Server
Retrieval is quite simple. Using the new "creds" object, you use the ApiHandler class to make requests from the server.

```
//This will return the first 25 tickets in the Server, unordered
//It is functionally identical to a GET to:
//          https://{hostname}/api/v1/{account id}/{department id}/Ticket
var tickets = ApiHandler.Ticket.TicketsGetList(creds);
```

Queries can be filtered as well. For example, retrieving customers based off of their email is simple:

```
var email = "test@example.com";
var customerQuery = new ModuleQuery.CustomerQuery();
customerQuery.RetrieveAllRecords = true; //automatically retrieve ALL data that match filter
customerQuery.AddStaticFieldFilter(ModuleQuery.CustomerQuery.CustomerStaticFields.CustomerEmail, Paraenums.QueryCriteria.Equal, email);

var customers = ApiHandler.Customer.CustomersGetList(creds, customerQuery);
```

Note the option to iteratively retrieve all records. It will default to the page size of 25 entities per GET.

### Entity Schema
Parature Entities support custom fields to be specified within the Service Desk. This is dynamically retrievable as well, similar to the /Schema call in the REST API.

```
var ticketSchema = ApiHandler.Ticket.TicketSchema(creds);
var customFields = ticketSchema.CustomFields; //all custom fields

var requiredFields = ticket.CustomFields
    .Where(f => f.CustomFieldRequired); //all custom fields which are required

//Field types are also returned and supplied as an Enum
var textFields = ticket.CustomFields
    .Where(f => f.DataType == Paraenums.CustomFieldDataType.String);
```

### Entity Relationships
Parature Entities have hard-coded relationships to other Entities. There are no custom relationships in the API. This is reflected in the ParatureSDK library as Class Properties.

```
//get first customer
var cust = ApiHandler.Customer.CustomersGetList(creds).Customers[0];
ticketSchema.Ticket_Customer = cust;
```

### Entity Creation/Update
The ApiHandler class has methods for different HTTP methods (GET, PUT, POST, DELETE). They are each very similar, and should be self-explanatory.

```
//create the ticket
var resp = ApiHandler.Ticket.TicketInsert(ticketSchema, creds);
if (resp.HasException)
{
    //something failed
}
```

Please note that all API requests are Synchronous and do block the current thread.