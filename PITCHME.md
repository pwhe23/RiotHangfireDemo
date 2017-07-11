
#### Background Tasks in Asp.net using Hangfire and Riot.js

Paul Wheeler <paul@paulwheeler.com>

Solutions Architect at Martus Solutions

@PaulWheeler

https://github.com/pwhe23/RiotHangfireDemo

---

#### Dependencies

- Hangfire - BackgroundTask processing (SQL Server)
- Riot.js - custom-tag based html/js UI
- Asp.net Identity - User authorization
- SimpleInjector - IoC container
- Routemeister - dispatches handlers for commands
- SignalR - push notifications from server to client
- Entity Framework - QueueItem, User tables

---

#### Hangfire.io

- Supports: Asp.net, web apps, console apps, Windows services, etc
- Job Types: Fire-and-forget, delayed, and recurring
- Cancellation: IJobCancellationToken.ThrowIfCancellationRequested()
- Best Practices: methods small and simple, re-entrant, app always running

---

#### Hangfire Parts

1. Client - BackgroundJobClient instance, or BackgroundJob static
2. Persistent Storage - Redis, SQL Server (including Azure); supports MSMQ, RabbitMQ
3. Server - BackgroundJobServer, app.UseHangfireServer; same process, separate server, multiple instances
4. Dashboard - Asp.net OWIN app, app.UseHangfireDashboard

---
 
#### Hangfire Setup

- Startup.ConfigureHangfire
- HangfireSimpleInjectorJobActivator
- DemoHangfireJobFilter
- DemoHangfireAuthorizationFilter
- Queue, QueueItem
- BackgroundTask, TaskResult
- GenerateReportTask, SendEmailTask

---

#### Command Architecture

- ICommander, IRequest, IRequestHandler
- Command.cs - Command, Query<T>, CommandResponse, CommandHandler, PagedList
- JsonRpcController - receives /jsonrpc POSTs
- Commander - maintains dictionary of commands by name
- Routemeister - calls command handlers by command type
- UserContext - current UserId (Asp.net Identity)
- IDb - generic database functions (EF)

---

#### Asp.net MVC

- Startup - app configuration (Hangfire & SignalR use OWIN)
- DemoConfig - strongly-typed AppSettings class
- Pusher - sends notifications to client (SignalR)
- Identity - ConfigureAuthentication, User, LoginUser, LogoutUser, PasswordHash
- Controllers - HomeController, JsonRpcController, PushHub
- Views - Index, Login, _Layout, _Navbar, _CommandResponse

---

#### Riot.js

- Custom tags - supports attributes, expressions, scripts, styles
- Mixins: shared, global
- Nested tags
- Transclusion - &lt;yield /&gt;
- Supports server-side rendering
- Virtual tag, data-is attribute

---

#### Riot Syntax

- riot.mount("*")
- &lt;CommandButton command="RequeueItems" data={ {ItemIds:selectedTasks} } /&gt;
- if={ hasPrevious }, show={ truthy }, hide={ truthy }
- each={ item in result.Items }
- ListQueueItems: ref="Log"
- onclick={ nextClicked }
- Tags - CommandButton, ListQueueItems, Pager, SelectAll

---

#### Riot - more

- Lifecycle: before-mount, mount, update, updated, before-unmount, unmount, *
- No self-closing in html
- Classes: string, shorthand, objects
- Styles: string, objects
- Simple debugging

---

#### Testing

- RiotHangfireDemo.Tests
- TestQueryQueueItems (Xunit, Shouldly, FakeItEasy)

---

#### Resources

- https://github.com/pwhe23/RiotHangfireDemo
- http://docs.hangfire.io/en/latest/
- http://riotjs.com/guide/
