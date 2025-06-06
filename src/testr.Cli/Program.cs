﻿using Microsoft.Extensions.DependencyInjection;

using tomware.TestR;

var services = new ServiceCollection()
    .AddCliCommand<RunCommand>()
    .AddCliCommand<TestCaseCommand>()
    .AddCliCommand<ValidateCommand>()
    .AddCliCommand<ManCommand>()
    .AddCliCommand<PlaywrightCommand>()
    .AddSingleton<Cli>();

var provider = services.BuildServiceProvider();
var cli = provider.GetRequiredService<Cli>();
cli.Name = "testR";
cli.Description = "A cli tool to manage and run executable test cases.";

using var meterProvider = OtelHelper.CreateMeterProvider(ref args);
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
  Console.WriteLine("Cancelling...");
  cts.Cancel();
  e.Cancel = true;
};
var returnCode = await cli.ExecuteAsync(args, cts.Token);
meterProvider.Dispose();
return returnCode;
