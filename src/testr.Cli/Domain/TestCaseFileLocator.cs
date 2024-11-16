namespace tomware.TestR;

internal static class TestCaseFileLocator
{
  public static string FindFile(string directory, string testCaseId)
  {
    foreach (var file in Directory.GetFiles(directory, "*.md", SearchOption.AllDirectories))
    {
      var splittedItems = File
        .ReadAllLines(file!)
        .FirstOrDefault()!
        .Split(":");
      if (splittedItems[0].Trim().ToLower().Contains(testCaseId.ToLower()))
      {
        return file;
      }
    }

    throw new FileNotFoundException($"TestCase definition for '{testCaseId}' not found!");
  }
}