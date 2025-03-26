using Microsoft.Playwright;

using tomware.TestR;

internal class TestCaseExecutor
{
  private readonly ExecutorConfig _config;
  private string _requestUri = string.Empty;

  public TestCaseExecutor(ExecutorConfig config)
  {
    _config = config;
  }

  public async Task<IEnumerable<TestStepResult>> ExecuteAsync(
    string domain,
    TestCaseExecutionParam executionParam,
    TestCaseExecutionParam? preconditionExecutionParam,
    CancellationToken cancellationToken
  )
  {
    using var playwright = await Playwright.CreateAsync();
    await using var browser = await GetBrowserType(playwright)
      .LaunchAsync(new BrowserTypeLaunchOptions
      {
        Headless = _config.Headless,
        SlowMo = _config.Slow // by N milliseconds per operation,
      });

    var context = await GetBrowserContext(browser);
    var page = await context.NewPageAsync();

    // execute precondition instructions if available
    if (preconditionExecutionParam != null)
    {
      await NavigateToRouteAsync(page, domain, preconditionExecutionParam.Route);

      foreach (var instruction in preconditionExecutionParam.Instructions)
      {
        await TestAsync(
          page,
          instruction,
          instruction => ConsoleHelper.WriteLine($"- Executing step: {instruction.TestStep.Id} - {instruction.TestStep.Description}")
        );
      }
    }

    await NavigateToRouteAsync(page, domain, executionParam.Route);

    var results = new List<TestStepResult>();
    foreach (var instruction in executionParam.Instructions)
    {
      var result = await TestAsync(
        page,
        instruction,
        instruction => ConsoleHelper.WriteLineYellow($"- Executing step: {instruction.TestStep.Id} - {instruction.TestStep.Description}")
      );
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

  private async Task NavigateToRouteAsync(
    IPage page,
    string domain,
    string route
  )
  {
    var requestUri = $"{domain}/{route}";
    if (requestUri != _requestUri)
    {
      await page.GotoAsync(requestUri);
      _requestUri = requestUri;

      // being safe and try again in certain case :-)
      if (!page.Url.StartsWith(requestUri))
      {
        await Task.Delay(1000);

        await page.GotoAsync(requestUri);
      }
    }
  }

  private static async Task<TestStepResult> TestAsync(
    IPage page,
    TestStepInstruction instruction,
    Action<TestStepInstruction> consoleAction
  )
  {
    try
    {
      await ProcessStepAsync(page, instruction, consoleAction);
    }
    catch (Exception ex)
    {
      return TestStepResult.Failed(instruction.TestStep, ex.Message);
    }

    return TestStepResult.Success(instruction.TestStep);
  }

  private static Task<bool> ProcessStepAsync(
    IPage page,
    TestStepInstruction instruction,
    Action<TestStepInstruction> consoleAction
  )
  {
    consoleAction(instruction);

    ILocator? locator = EvaluateLocator(page, instruction);

    return ExecuteAction(instruction, locator);
  }

  private static ILocator EvaluateLocator(
    IPage page,
    TestStepInstruction instruction
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

  private static Task<bool> ExecuteAction(
    TestStepInstruction instruction,
    ILocator locator
  )
  {
    if (instruction.Action == ActionType.Click)
    {
      locator.ClickAsync();
    }
    else if (instruction.Action == ActionType.Fill)
    {
      locator.FillAsync(instruction.Value);
    }
    else if (instruction.Action == ActionType.PickFile)
    {
      locator.SetInputFilesAsync(instruction.Value);
    }
    else if (instruction.Action == ActionType.IsVisible)
    {
      return locator.IsVisibleAsync();
    }

    return Task.FromResult(true);
  }
}
