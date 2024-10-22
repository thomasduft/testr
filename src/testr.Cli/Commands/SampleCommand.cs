using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class SampleCommand : CommandLineApplication
{
  public SampleCommand()
  {
    Name = "sample";
    Description = "Sample command that greets from the console.";

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine("Hi, I am just a sample command.");

    return await Task.FromResult(0);
  }
}