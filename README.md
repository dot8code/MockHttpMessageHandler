# MockHttpMessageHandler

Mocking an HTTP client in C# is important for several reasons. First, it can help you write more effective unit tests. By mocking the HTTP client, you can test your code without actually making any network requests. This can help you write faster tests that are less prone to failure.

Mocking an HTTP client can help you test your code in isolation. By mocking the HTTP client, you can test your code without worrying about external dependencies. This can help you write more reliable tests that are less prone to failure.

Mocking an HTTP client can help you test your code more thoroughly. By mocking the HTTP client, you can simulate different responses and error conditions. This can help you identify issues early on and avoid costly mistakes down the line.

In summary, mocking an HTTP client in C# is important for ensuring that your code works as expected, is maintainable, and is secure. 

## Installation

```sh
dotnet add package dot8code.Tests.MockHttpMessageHandler
```

## Usage

Main class to mock HttpMessageHandler is `FakeHttpMessageHandler` this class is 
generic and we can provide any type of object for example `string`, `int`, `YourClass`.
```csharp
    var mockedMessage = new FakeHttpMessageHandler<T>(expectedT, HttpStatusCode);
    var httpClient = new HttpClient(mockedMessage);
```

## Basic Usage

```csharp
    const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
    const string expectedResponse = "expected rsponse";

    var mockedMessage = new FakeHttpMessageHandler<string>(expectedResponse, expectedStatusCode);
    var httpClient = new HttpClient(mockedMessage);
```