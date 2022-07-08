using AcmePowerSolutions.MeterDataManagement.Api.Model;
using AcmePowerSolutions.MeterDataManagement.Proto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NServiceBus;
using static AcmePowerSolutions.MeterDataManagement.Proto.PowerMeterReading;

namespace AcmePowerSolutions.MeterDataManagement.Api.Grpc.v1_0;

public class PowerMeterReadingService : PowerMeterReadingBase
{
    private readonly ILogger<PowerMeterReadingService> logger;
    private readonly IMeterReadingRepository repository;
    private readonly IMessageSession messageSession;

    public PowerMeterReadingService(ILogger<PowerMeterReadingService> logger, IMeterReadingRepository repository, IMessageSession messageSession)
    {
        this.logger = logger;
        this.repository = repository;
        this.messageSession = messageSession;
    }

    public override async Task<Empty> AddPowerReading(PowerMeterReadingMessage message, ServerCallContext context)
    {
        logger.LogDebug("Handling '{messageType}'. Message was: {request}", nameof(PowerMeterReadingMessage), message);
        await repository.AddMeterReading(
            new MeterReading(message.CustomerId, message.MeterId, message.ReadingTime.ToDateTimeOffset(), message.Value));
        
        return new Empty();
    }

    public async override Task<Empty> AbnormalPowerConsumptionDetected(PowerMeterReadingMessage request, ServerCallContext context)
    {
        //await messageSession.Publish(
        //    new AbnormalConsumptionDetected(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), request.ReadingTime.ToDateTimeOffset()));
        return new Empty();
    }
}