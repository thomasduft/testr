using CliWrap;

using McMaster.Extensions.CommandLineUtils;

using static tomware.TestR.ConsoleHelper;

namespace tomware.TestR;

public class InstallPlaywrightCommand : CommandLineApplication
{
  public InstallPlaywrightCommand()
  {
    Name = "install-playwright";
    Description = "Installs Playwright as the underlying dependency.";

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
    var directory = new FileInfo(location).Directory!;
    var path = Path.Combine(directory.FullName, "playwright.ps1");

    if (!File.Exists(path))
    {
      Console.WriteLine($"Powershell file '{path}' does not exists!");
      return await Task.FromResult(1);
    }

    // TODO: Refactor so it also works on Linux environments
    await CliWrap.Cli.Wrap("powershell.exe") // cmd
      .WithArguments(args => args
        // .Add("pwsh")
        .Add(path)
        .Add("install")
      )
      .WithStandardOutputPipe(PipeTarget.ToDelegate(WriteLine))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(WriteLineError))
      .WithValidation(CommandResultValidation.None)
      .ExecuteAsync(cancellationToken);

    return await Task.FromResult(0);
  }
}
