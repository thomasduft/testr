using Microsoft.Playwright;

using tomware.TestR;

public class TestCaseExecutor
{
  private readonly ExecutorConfig _config;
  private string _requestUri = string.Empty;

  public TestCaseExecutor(ExecutorConfig config)
  {
    _config = config;
  }

  public async Task<IEnumerable<TestCaseResult>> ExecuteAsync(
    string domain,
    string route,
    IEnumerable<TestStepInstructionItem> instructions
  )
  {
    var results = new List<TestCaseResult>();

    using var playwright = await Playwright.CreateAsync();
    await using var browser = await GetBrowserType(playwright)
      .LaunchAsync(new BrowserTypeLaunchOptions
      {
        // Channel = !string.IsNullOrWhiteSpace(_browserType) ? _browserType : null,
        Headless = _config.Headless,
        SlowMo = _config.Slow // by N milliseconds per operation,
      });

    var context = await GetBrowserContext(browser);
    var page = await context.NewPageAsync();

    foreach (var instruction in instructions)
    {
      var result = await TestAsync(page, domain, route, instruction);
      results.Add(result);
    }

    await context.DisposeAsync();

    return results;
  }

  private IBrowserType GetBrowserType(IPlaywright playwright)
  {
    return _config.BrowserType switch
    {
      tomware.TestR.BrowserType.Webkit => playwright.Webkit,
      tomware.TestR.BrowserType.Firefox => playwright.Firefox,
      _ => playwright.Chromium,
    };
  }

  private async Task<IBrowserContext> GetBrowserContext(IBrowser browser)
  {
    var options = new BrowserNewContextOptions
    {
      IgnoreHTTPSErrors = true
    };

    if (!string.IsNullOrWhiteSpace(_config.RecordVideoDir))
    {
      options.RecordVideoDir = _config.RecordVideoDir;
      options.RecordVideoSize = new RecordVideoSize() { Width = 640, Height = 480 };
    }

    IBrowserContext? context = await browser.NewContextAsync(options);
    context.SetDefaultTimeout(_config.Timeout);

    return context;
  }

  private async Task<TestCaseResult> TestAsync(
    IPage page,
    string domain,
    string route,
    TestStepInstructionItem instruction
  )
  {
    try
    {
      var requestUri = $"{domain}/{route}";
      if (requestUri != _requestUri)
      {
        await page.GotoAsync(requestUri);
        _requestUri = requestUri;

        // being safe and try again in certain case :-)
        if (!page.Url.StartsWith(requestUri))
        {
          await page.GotoAsync(requestUri);
        }
      }

      await ProcessStepAsync(page, instruction);

      return TestCaseResult.Success(instruction.TestStep);
    }
    catch (Exception ex)
    {
      return TestCaseResult.Failed(instruction.TestStep, ex.Message);
    }
  }

  private static async Task<bool> ProcessStepAsync(
    IPage page,
    TestStepInstructionItem instruction
  )
  {
    ILocator? locator = EvaluateLocator(page, instruction);

    return await ExecuteAction(instruction, locator);
  }

  private static ILocator EvaluateLocator(
    IPage page,
    TestStepInstructionItem instruction
  )
  {
    ILocator? locator = instruction.Locator switch
    {
      LocatorType.GetByLabel => page.GetByLabel(instruction.Text),
      LocatorType.GetByRole => page.GetByRole(instruction.AriaRole, new() { Name = instruction.Text }),
      LocatorType.GetByTestId => page.GetByTestId(instruction.Text),
      LocatorType.BySelector => page.Locator(instruction.Text),
      _ => page.GetByText(instruction.Text)
    };

    return locator;
  }

  private static async Task<bool> ExecuteAction(
    TestStepInstructionItem instruction,
    ILocator locator
  )
  {
    if (instruction.Action == ActionType.Click)
    {
      await locator.ClickAsync();
    }
    else if (instruction.Action == ActionType.Fill)
    {
      await locator.FillAsync(instruction.Value);
    }
    else
    {
      return await locator.IsVisibleAsync();
    }

    return await Task.FromResult(true);
  }
}
