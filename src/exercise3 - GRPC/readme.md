
# Exercise 3 - gRPC

## Introduction to the solution

Since the last exercise there has been added some new project to the solution

### SmartMeter

A simple console app to simulate a Power Meter. The creates a new meter reading every 5 second. You can press Enter in the console window to create an abormal high reading.

### Meter Reading Service

The Meter Reading Service is responsible for collecting data from the SmartMeter, Persist those in LiteDB and expose the readings via a REST API.

The REST API and Data repository is already in place, but we need to build the gPRC for communication with the Meters.


## Step 1 - Add proto file

First we need to build the Protobuf contracts defining out Meter Reading Service.

In the _MeterReadingService / WebApi_ project add a new _Protos_ folder. Righht-Click click the new folder select _Add.. > New Item.._

In the dialog find the _Protocol Buffer File_ and name the new file _powermeterreading.proto_.

Replace the file with this:

```
syntax = "proto3";

option csharp_namespace = "MeterReadingService.WebApi";

import "google/protobuf/Timestamp.proto";
import "google/protobuf/Empty.proto";

service PowerMeterReading {
  rpc AddPowerReading (PowerMeterReadingMessage) returns (google.protobuf.Empty);
  rpc AbnormalPowerConsumptionDetected (PowerMeterReadingMessage) returns (google.protobuf.Empty);
}

message PowerMeterReadingMessage {
  int32 customerId = 1;
  string meterId = 2;
  double value = 3;
  google.protobuf.Timestamp readingTime = 4;
}
```

Right-Click the _powermeterreading.proto_ file and select properties. In the properties pane set the these values:
|   |   |
|---|---|
| **Build Action** | Protobuf Compiler |
| **gRPC Stub Classes** | Server Only |

Build the project.

## Step 2 - Implement Server side Service

Next step is to implement server side implementation of the Protocol Buffer.

In the _MeterReadingService / WebApi_ project add a new _Services_ folder. Righht-Click click the new folder select _Add.. > New Class.._ name the class _PowerMeterReadingService_.

Replace the file with this:

```
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MeterReadingService.Data;
using MeterReadingService.Models;
using static MeterReadingService.WebApi.PowerMeterReading;

namespace MeterReadingService.WebApi.Services;

public class PowerMeterReadingService : PowerMeterReadingBase
{
    private readonly IMeterReadingRepository repository;

    public PowerMeterReadingService(IMeterReadingRepository repository)
    {
        this.repository = repository;
    }

    public override Task<Empty> AddPowerReading(PowerMeterReadingMessage request, ServerCallContext context)
    {
        repository.Add(
            new MeterReading(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value));

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> AbnormalPowerConsumptionDetected(PowerMeterReadingMessage request, ServerCallContext context)
    {
        // TODO: Raise event
        Console.WriteLine("!!");
        return Task.FromResult(new Empty());
    }
}
```

Open the _Program.cs_ file and above the _app.MapControllers();_ add:

```
app.MapGrpcService<PowerMeterReadingService>();
```
Build the project.

## Step 3 - Implent the Client in SmartMeter

With the Server Side Service in place lets look at the client implementation.

Right-Click the _SmartMeter_ project, select _Add > Service Reference..._ in the dialog select _gRPC_ and click Next. Select the _File_ option and browse to the _MeterReadingService \WebApi \ Protos \ powermeterreading.proto_ and click open. Select _Client_ in the _Select the type of class to be generated_ list and click _Finish_.


Open the _SmartMeter \ MeterReadingService.cs_ file and update with this code:

```
using Grpc.Net.Client;
using MeterReadingService.WebApi;
using Microsoft.Extensions.Hosting;
using static MeterReadingService.WebApi.PowerMeterReading;

namespace SmartMeter;

internal class MeterReadingService : BackgroundService
{
    private readonly PowerMeterReadingClient client;

    public MeterReadingService()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:8003");
        this.client = new PowerMeterReadingClient(channel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random consumptionRandom = new();
        double value = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumption = Math.Round(consumptionRandom.NextDouble(), 2);
            value += consumption;
            value = Math.Round(value, 2);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");

            await client.AddPowerReadingAsync(
                new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                }, cancellationToken: stoppingToken);

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                value += 20;
                value = Math.Round(value, 2);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");

                await client.AddPowerReadingAsync(
                    new PowerMeterReadingMessage
                    {
                        CustomerId = 1,
                        MeterId = Guid.NewGuid().ToString(),
                        ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                        Value = value
                    }, cancellationToken: stoppingToken);

                await client.AbnormalPowerConsumptionDetectedAsync(new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                }, cancellationToken: stoppingToken);
                Console.ResetColor();
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}
```

Build the solution. Right-Click the solution and make sure that all the WebApi's, ApiGateway SmartMeter and Client are set to start. Press _F5_ to run the app. Navigate to the Consumption page and you should begin to se the chart be filled with data.

Go to the Console window of the SmartMeter application and press enter and wait a few seconds. This should trigger a very high amount of consumption and be visible on the chart.

## Step 4 - gRPC Streaming

For better performance of our app we want to leverage the streaming capabilities of gRPC.

Open the _MeterReadingService / WebApi / Protos / powermeterreading.proto_ and add this service definition:

```
rpc StreamPowerReadings (stream PowerMeterReadingMessage) returns (google.protobuf.Empty);
```

Build the project. Open the _MeterReadingService / WebApi / Services/ PowerMeterReadingService.cs_ and add this method:

```
public override async Task<Empty> StreamPowerReadings(IAsyncStreamReader<PowerMeterReadingMessage> requestStream, ServerCallContext context)
{
    await foreach (var request in requestStream.ReadAllAsync())
    {
        repository.Add(
            new MeterReading(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value));
    }

    return new Empty();
}
```
Now open the _SmartMeter / MeterReadingService.cs_ and replace it with this:

```
using Grpc.Net.Client;
using MeterReadingService.WebApi;
using Microsoft.Extensions.Hosting;
using static MeterReadingService.WebApi.PowerMeterReading;

namespace SmartMeter;

internal class MeterReadingService : BackgroundService
{
    private readonly PowerMeterReadingClient client;

    public MeterReadingService()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:8003");
        this.client = new PowerMeterReadingClient(channel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random consumptionRandom = new();
        double value = 0;
        
        var stream = client.StreamPowerReadings(cancellationToken: stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumption = Math.Round(consumptionRandom.NextDouble(), 2);
            value += consumption;
            value = Math.Round(value, 2);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");
            
            await stream.RequestStream.WriteAsync(new PowerMeterReadingMessage
            {
                CustomerId = 1,
                MeterId = Guid.NewGuid().ToString(),
                ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                Value = value
            });

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                value += 20;
                value = Math.Round(value, 2);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");

                await stream.RequestStream.WriteAsync(new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                });

                await client.AbnormalPowerConsumptionDetectedAsync(new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                }, cancellationToken: stoppingToken);
                Console.ResetColor();
            }
            await Task.Delay(5000, stoppingToken);
        }

        await stream.RequestStream.CompleteAsync();
    }
}
```

Press _F5_ to run.