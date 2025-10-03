# NetMQ.ReactiveExtensions

[![Build](https://img.shields.io/appveyor/ci/drewnoakes/netmq-reactiveextensions.svg)](https://ci.appveyor.com/project/drewnoakes/netmq-reactiveextensions) [![NuGet](https://img.shields.io/nuget/v/NetMQ.ReactiveExtensions.svg)](https://www.nuget.org/packages/NetMQ.ReactiveExtensions/) [![NuGet prerelease](https://img.shields.io/nuget/vpre/NetMQ.ReactiveExtensions.svg)](https://www.nuget.org/packages/NetMQ.ReactiveExtensions/)

Effortlessly send messages anywhere on the network using Reactive Extensions (RX). Uses NetMQ as the transport layer. 

Fast! Runs at >120,000 messages per second on localhost (by comparison, Tibco runs at 100,000 on the same machine).

## Sample Code

The API is a drop-in replacement for `Subject<T>` from Reactive Extensions (RX).

As a refresher, to use `Subject<T>` in Reactive Extensions (RX):

```csharp 
var subject = new Subject<int>();
subject.Subscribe(message =>
{
    // If we get an error "Cannot convert lambda ... not a delegate type", install Reactive Extensions from NuGet.
    Console.Write(message); // Prints "42".
});
subject.OnNext(42);
```

The new API starts with a drop-in replacement for `Subject<T>`:

```csharp
var subject = new SubjectNetMQ<int>("tcp://127.0.0.1:56001");
subject.Subscribe(message =>
{
    Console.Write(message); // Prints "42".
});
subject.OnNext(42); // Sends 42.
```

This is great for a demo, but is not recommended for any real life application.

For those of us familiar with Reactive Extensions (RX), `Subject<T>` is a combination of a publisher and a subscriber. If we are running a real-life application, we should separate out the publisher and the subscriber, because this means we can create the connection earlier which makes the transport setup more deterministic:

```csharp
var publisher = new PublisherNetMq<int>("tcp://127.0.0.1:56001");
var subscriber = new SubscriberNetMq<int>("tcp://127.0.0.1:56001");
subscriber.Subscribe(message =>
{
	Console.Write(message); // Prints "42".
});
publisher.OnNext(42); // Sends 42.
```

If we want to run in separate applications:

```csharp
// Application 1 (subscriber)
var subscriber1 = new SubscriberNetMq<int>("tcp://127.0.0.1:56001");
subscriber1.Subscribe(message =>
{
    Console.Write(message); // Prints "42".
});

// Application 2 (subscriber)
var subscriber2 = new SubscriberNetMq<int>("tcp://127.0.0.1:56001");
subscriber2.Subscribe(message =>
{
    Console.Write(message); // Prints "42".
});

// Application 3 (publisher)
var publisher = new PublisherNetMq<int>("tcp://127.0.0.1:56001");
publisher.OnNext(42); // Sends 42.
```

Currently, serialization is performed using [ProtoBuf](https://github.com/mgravell/protobuf-net "ProtoBuf"). It will handle simple types such as `int` without annotation, but if we want to send more complex classes, we have to annotate like this:

```csharp
// For Protobuf support, include NuGet package protobuf-net from Marc Gravell.
[ProtoContract]
public struct MyMessage
{
    [ProtoMember(1)]
    public int Num { get; set; }
    [ProtoMember(2)]
    public string Name { get; set; }
}

var publisher = new PublisherNetMq<MyMessage>("tcp://127.0.0.1:56001");
var subscriber = new SubscriberNetMq<MyMessage>("tcp://127.0.0.1:56001");
subscriber.Subscribe(message =>
{
    Console.Write(message.Num); // Prints "42".
    Console.Write(message.Name); // Prints "Bill".
});
publisher.OnNext(new MyMessage(42, "Bill"); 
```

## NuGet Package

[![Build](https://img.shields.io/appveyor/ci/drewnoakes/netmq-reactiveextensions.svg)](https://ci.appveyor.com/project/drewnoakes/netmq-reactiveextensions) [![NuGet](https://img.shields.io/nuget/v/NetMQ.ReactiveExtensions.svg)](https://www.nuget.org/packages/NetMQ.ReactiveExtensions/) [![NuGet prerelease](https://img.shields.io/nuget/vpre/NetMQ.ReactiveExtensions.svg)](https://www.nuget.org/packages/NetMQ.ReactiveExtensions/)

See [NetMQ.ReactiveExtensions](https://www.nuget.org/packages/NetMQ.ReactiveExtensions/).

This library has been migrated to modern .NET and now targets .NET 9, using the latest versions of dependencies:
- System.Reactive 6.1.0
- NetMQ 4.0.2.1
- protobuf-net 3.2.56

This library is compatible with:
- Windows
- Linux
- Mac

All projects use modern SDK-style .csproj files and .NET 9 for optimal performance and compatibility.

## Compiling from source

Requirements:
- Install [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- Visual Studio 2022, Visual Studio Code, or any IDE that supports .NET 9

Build steps:
```bash
dotnet restore
dotnet build
dotnet test
```

If the project does not compile on your machine, raise an issue here on GitHub.

## Demos

To check out the demos, see:
- Publisher: Project `NetMQ.ReactiveExtensions.SamplePublisher`
- Subscriber: Project `NetMQ.ReactiveExtensions.SampleSubscriber`
- Sample unit tests: Project `NetMQ.ReactiveExtensions.Tests`

## Performance

- Runs at >120,000 messages per second on localhost.

## 100% compatible with Reactive Extensions (RX) 

- Compatible with all existing Reactive Extensions code, as it implements IObservable<T> and IObserver<T> from Microsoft.
- Can use `.Where()`, `.Select()`, `.Buffer()`, `.Throttle()`, etc.
- Supports `.OnNext()`, `.OnException()`, and `.OnCompleted()`.
- Properly passes exceptions across the wire.

## Unit tests

- Supported by a full suite of unit tests.

## Projects like this one that do messaging

- See [Obvs](https://github.com/inter8ection/Obvs), an fantastic RX wrapper which supports many transport layers including NetMQ, RabbitMQ and Azure, and many serialization methods including ProtoBuf and MsgPack.
- See [Obvs.NetMQ](https://github.com/inter8ection/Obvs.Netmq), the RX wrapper with NetMQ as the transport layer. 
- Search for [all packages on NuGet that depend on RX](http://nugetmusthaves.com/Dependencies/Rx-Linq), and pick out the ones that are related to message buses.
- Check out Kafka. It provides many-to-many messaging, with persistance, and multi-node redundancy. And its blindingly fast.

## Wiki

See the [Wiki with more documentation](https://github.com/NetMQ/NetMQ.ReactiveExtensions/wiki).



