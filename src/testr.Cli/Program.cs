using Microsoft.Extensions.DependencyInjection;

using tomware.TestR;

var services = new ServiceCollection()
    .AddCliCommand<TestCaseCommand>()
    .AddCliCommand<TestCaseResultCommand>()
    .AddCliCommand<RunCommand>()
    .AddCliCommand<ValidateCommand>()
    .AddSingleton<Cli>();

var provider = services.BuildServiceProvider();
var cli = provider.GetRequiredService<Cli>();
cli.Name = "testR";
cli.Description = "A cli tool to manage executable test cases.";

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
  Console.WriteLine("Cancelling...");
  cts.Cancel();
  e.Cancel = true;
};

return await cli.ExecuteAsync(args, cts.Token);