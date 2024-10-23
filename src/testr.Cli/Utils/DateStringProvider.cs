using System;
using System.Diagnostics;

namespace tomware.TestR;

public static class DateStringProvider
{
  public static string GetDateString()
  {
    return DateTime.Now.ToString("yyyy-MM-dd");
  }
}