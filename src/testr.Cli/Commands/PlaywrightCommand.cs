using CliWrap;

using McMaster.Extensions.CommandLineUtils;

using static tomware.TestR.ConsoleHelper;

namespace tomware.TestR;

public class PlaywrightCommand : CommandLineApplication
{
  private readonly CommandOption<string> _playwrightCommand;
  private readonly CommandOption<string> _playwrightCommandOption;

  public PlaywrightCommand()
  {
    Name = "playwright";
    Description = "Offers Playwright specific commands.";

    _playwrightCommand = Option<string>(
      "-c|--command",
      "The command (e.g. 'install', 'uninstall', ...) to execute (defaults to '--help').",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = "--help",
      true
    );

    _playwrightCommandOption = Option<string>(
      "-o|--option",
      "The option for the command (e.g. 'chromium', 'firefox', ...) to execute.",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = string.Empty,
      true
    );

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

    var command = Environment.OSVersion.Platform == PlatformID.Unix
      ? "pwsh"
      : "powershell.exe";

    await CliWrap.Cli.Wrap(command) // cmd
      .WithArguments(args => args
        .Add(path)
        .Add(_playwrightCommand.ParsedValue)
        .Add(_playwrightCommandOption.ParsedValue)
      )
      .WithStandardOutputPipe(PipeTarget.ToDelegate(WriteLine))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(WriteLineError))
      .WithValidation(CommandResultValidation.None)
      .ExecuteAsync(cancellationToken);

    return await Task.FromResult(0);
  }
}