using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.DependencyInjection;

using Template.Cli;

var services = new ServiceCollection()
    .AddCliCommand<SampleCommand>()
    .AddSingleton<Cli>();

var provider = services.BuildServiceProvider();
var cli = provider.GetRequiredService<Cli>();
cli.Name = "cli-template";
cli.Description = "A .NET CLI template.";

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
  Console.WriteLine("Canceling...");
  cts.Cancel();
  e.Cancel = true;
};

return await cli.ExecuteAsync(args, cts.Token);

public class Cli : CommandLineApplication
{
  public Cli(IEnumerable<CommandLineApplication> commands)
  {
    HelpOption("-? | -h | --help", true);

    foreach (var cmd in commands)
    {
      AddSubcommand(cmd);
    }
  }
}

public static class ServiceConfiguration
{
  public static IServiceCollection AddCliCommand<TCommand>(this IServiceCollection services)
      where TCommand : CommandLineApplication
  {
    services.AddSingleton<CommandLineApplication, TCommand>();

    return services;
  }
}