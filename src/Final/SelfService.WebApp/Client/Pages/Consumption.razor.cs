using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Client.Api;
using SelfService.WebApp.Shared.Models;
using System.Timers;

namespace SelfService.WebApp.Client.Pages;

public partial class Consumption
{
    [Inject]
    public ConsumptionService? ConsumptionService { get; set; }
    public IEnumerable<MeterReading>? MeterReadings { get; set; }

    private static System.Timers.Timer timer;

    protected async override Task OnInitializedAsync()
    {
        MeterReadings = await ConsumptionService!.GetConsumption(1);
        StartTimer();

    }

    public void StartTimer()
    {
        timer = new System.Timers.Timer(10_000);
        timer.Elapsed += RefreshConsumption;
        timer.Enabled = true;
    }

    private async void RefreshConsumption(object? sender, ElapsedEventArgs e)
    {
        await InvokeAsync(async () =>
        {
            MeterReadings = await ConsumptionService!.GetConsumption(1);
            StateHasChanged();
        });
    }
}
