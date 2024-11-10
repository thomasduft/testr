namespace tomware.TestR;

public record ExecutorConfig(
  bool Headless = false,
  int Slow = 500,
  int Timeout = 30000,
  BrowserType BrowserType = BrowserType.Chrome,
  string? RecordVideoDir = null
);