using Fluid;

using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class ManCommand : CommandLineApplication
{
  public ManCommand()
  {
    Name = "man";
    Description = "Displays a manual page for the available Test-Data syntax.";

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine("Test-Data Syntax");

    return await Task.FromResult(0);
  }
}