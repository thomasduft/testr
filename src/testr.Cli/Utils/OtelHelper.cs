using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

internal static class OtelHelper
{
  private const string OtlpOption = "--otlp";

  public static MeterProvider CreateMeterProvider(ref string[] args)
  {
    var meterProviderBuilder = Sdk.CreateMeterProviderBuilder()
      .SetResourceBuilder(ResourceBuilder
        .CreateDefault()
        .AddService("testR.Cli"))
      .AddMeter("tomware.TestR.Cli.Metrics");
    // .AddConsoleExporter();
    if (args.Contains(OtlpOption))
    {
      var otlpIndex = Array.IndexOf(args, OtlpOption);
      if (otlpIndex + 1 < args.Length)
      {
        var uriString = args[otlpIndex + 1];
        meterProviderBuilder
          .AddOtlpExporter((exporterOptions, metricReaderOptions) =>
          {
            exporterOptions.Endpoint = new Uri(uriString);
            exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
            metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 100;
          });

        // Remove the --otlp and the URI from the args
        args = args
          .Where((_, index) => index != otlpIndex && index != otlpIndex + 1)
          .ToArray();
      }
    }

    return meterProviderBuilder.Build();
  }
}
