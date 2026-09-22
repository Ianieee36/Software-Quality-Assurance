namespace AppointmentBooking.Metrics;

public sealed record CliOptions(
    string EventsPath,
    string TargetsPath,
    string OutputPath)
{
    public static CliOptions Parse(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        string eventsPath = "LabData/booking-events.csv";
        string targetsPath = "LabData/quality-targets.json";
        string outputPath = "artifacts/quality-summary.md";
        for (int index = 0; index < args.Length; index++)
        {
            switch (args[index])
            {
                case "--events":
                    eventsPath = ReadValue(args, ref index, "--events");
                    break;
                case "--targets":
                    targetsPath = ReadValue(args, ref index, "--targets");
                    break;
                case "--output":
                    outputPath = ReadValue(args, ref index, "--output");
                    break;
                default:
                    throw new ArgumentException($"Unknown option '{args[index]}'.");
            }
        }

        return new CliOptions(eventsPath, targetsPath, outputPath);
    }

    private static string ReadValue(string[] args, ref int index, string option)
    {
        index++;
        if (index >= args.Length || args[index].StartsWith("--", StringComparison.Ordinal))
        {
            throw new ArgumentException($"Option '{option}' requires a value.");
        }

        return args[index];
    }
}
