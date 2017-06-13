# RiotHangfireDemo
https://github.com/pwhe23/RiotHangfireDemo

Demo Asp.net application using Riot.js and Hangfire.io created for a talk at Modern Devs Charlotte.

## RiotHangfireDemo.Web

RiotHangfireDemo.Web is an standard Asp.net MVC website which bootstraps and hosts our application. The views are created using Riot.js, so there is only a single Home/Index controller action. SignalR is used for notifying the user-interface of job status changes from Hangfire. The service interface implementations also reside here to keep as much external code out of our Domain project as possible.

* [ASP.NET MVC](https://www.asp.net/mvc) gives you a powerful, patterns-based way to build dynamic websites that enables a clean separation of concerns and that gives you full control over markup for enjoyable, agile development.
* [Riot.js](http://riotjs.com/) is a simple and elegant component-based UI library that brings custom tags to all browsers. A custom tag glues relevant HTML and JavaScript together forming a reusable component. Think React + Polymer but with enjoyable syntax and a small learning curve.
* [Hangfire](https://www.hangfire.io/) is an easy way to perform background processing in .NET and .NET Core applications. No Windows Service or separate process required. It is backed by persistent storage and open and free for commercial use.
* [Bootstrap](http://getbootstrap.com/) is the most popular HTML, CSS, and JS framework for developing responsive, mobile first projects on the web.
* [Font Awesome](http://fontawesome.io/) gives you scalable vector icons that can instantly be customized - size, color, drop shadow, and anything that can be done with the power of CSS.
* [Moment.js](https://momentjs.com/) Parse, validate, manipulate, and display dates and times in JavaScript.
* [JsonRPC](http://www.jsonrpc.org/specification) JSON-RPC is a stateless, light-weight remote procedure call (RPC) protocol
* [SignalR](https://www.asp.net/signalr) makes developing real-time web functionality for Asp.net truly easy. SignalR allows bi-directional communication between server and client.
* [Simple Injector](https://simpleinjector.org/) is an easy-to-use Dependency Injection (DI) library for .NET 4+ that supports Silverlight, Windows Phone, Windows 8 including Universal apps and Mono. Simple Injector is highly optimized for performance and concurrent use.
* [Json.NET](http://www.newtonsoft.com/json) is a popular high-performance JSON framework for .NET that can serialize and deserialize any .NET object.

## RiotHangfireDemo.Domain

RiotHangfireDemo.Domain contains the majority of our business logic via the Commands. It also contains the Model classes and the DemoDb database context class. Finally, the Service interfaces are defined here which must be implemented by our application host, the Mvc website.

* [MediatR](https://github.com/jbogard/MediatR) is a simple, unambitious mediator implementation in .NET. Supports request/response, commands, queries, notifications and events, synchronous and async with intelligent dispatching via C# generic variance.
* [Faker.cs](https://github.com/oriches/faker-cs) is used to easily generate fake data: names, addresses, phone numbers, etc.
* [Entity Framework](https://docs.microsoft.com/en-us/ef/) is an object-relational mapper (O/RM) that enables .NET developers to work with a database using .NET objects. It eliminates the need for most of the data-access code that developers usually need to write.)
* [BatMap](https://dogusteknoloji.github.io/BatMap/) is an opininated (yet another) mapper, mainly to convert between EF Entities and DTOs.

## RiotHangfireDemo.Tests

RiotHangfireDemo.Tests contains a simple example of how commands can be unit tested since this is a common question.

* [xUnit.net](https://xunit.github.io/) is a free, open source, community-focused unit testing tool for the .NET Framework.
* [FakeItEasy](https://fakeiteasy.github.io/) is a .Net dynamic fake framework for creating all types of fake objects, mocks, stubs etc.
* [Shouldly](https://github.com/shouldly/shouldly) is an assertion framework which focuses on giving great error messages when the assertion fails while being simple and terse.

## What's missing

* Token authentication
