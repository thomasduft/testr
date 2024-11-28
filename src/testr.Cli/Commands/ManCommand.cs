using Fluid;

using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class ManCommand : CommandLineApplication
{
  public ManCommand()
  {
    Name = "man";
    Description = "Displays a man page for helping with writing the Test-Data syntax within a Test Case.";

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    var content = await GetContent(
      Templates.Manual
    );

    Console.Write(content);

    return await Task.FromResult(0);
  }

  private async ValueTask<string> GetContent(
    string templateName
  )
  {
    var source = ResourceLoader.GetTemplate(templateName);
    var template = new FluidParser().Parse(source);

    return await template.RenderAsync();
  }
}