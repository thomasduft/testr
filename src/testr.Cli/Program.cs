using Microsoft.Extensions.DependencyInjection;

using tomware.TestR;

var services = new ServiceCollection()
    .AddCliCommand<TestCaseCommand>()
    .AddCliCommand<TestCaseResultCommand>()
    .AddSingleton<Cli>();

var provider = services.BuildServiceProvider();
var cli = provider.GetRequiredService<Cli>();
cli.Name = "cli-template";
cli.Description = "A .NET CLI template.";

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
  Console.WriteLine("Cancelling...");
  cts.Cancel();
  e.Cancel = true;
};

return await cli.ExecuteAsync(args, cts.Token);
