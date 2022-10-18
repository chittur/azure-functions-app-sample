# Overview
This is a sample project that demonstrates creating REST APIs with Azure Functions. Azure Functions is a Serverless solution that provides Functions as a Service (FaaS).

A few key terms to get familiar here are:
- **Trigger** causes a function to run. A trigger defines how a function is invoked and a function must have exactly one trigger. Triggers have associated data, which is often provided as the payload of the function. Examples of Triggers are HTTP, Timer, Message.

- **Binding** to a function is a way of declaratively connecting another resource to the function; bindings may be connected as input bindings, output bindings, or both. Data from bindings is provided to the function as parameters. Examples of Bindings are Queue, Cosmos DB, Table Storage, SendGrid.
- **Representational State Transfer (REST)** APIs are service endpoints that support sets of HTTP operations (methods), which provide create, retrieve, update, or delete access to the service's resources. REST provides a set of guidelines for creating stateless, reliable APIs. By statelessness, is implied that the server does not store any state about the client session on the server-side. Each call from the client to the server contains the required information to understand the request. The application’s session state is maintained entirely on the client.

This sample creates REST APIs with Azure functions, and employs HTTP trigger with Table Storage as the binding.

# Environment
The project builds and runs with Visual Studio Community 2022 when the required workloads are installed.

There are two projects in this solution:
- *ServerlessFunc* defines our Azure Functions APIs.
- *ServerlessFuncUnitTesting* defines the unit tests for these Azure Functions APIs.

# Running the project
To test locally
- Run the ServerlessFunc project. 
- Make sure that the [Azure Storage Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio) is also run. Run the following from an administrator command prompt: "%programfiles%\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\Microsoft\Azure Storage Emulator".
- Run the unit tests.

# Reference
- [Azure Functions](http://https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview "Azure Functions")
- [Triggers and Bindings](https://learn.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings?tabs=csharp)
- [Azure REST API reference](https://learn.microsoft.com/en-us/rest/api/azure/)
- [Build RESTful APIs](https://learn.microsoft.com/en-us/aspnet/web-api/overview/older-versions/build-restful-apis-with-aspnet-web-api)
