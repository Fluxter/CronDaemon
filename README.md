# Fluxter CronManager
A small library for cron like execution

## Installation
Nuget coming soon.

## Example
### Basic Usage
```csharp
var daemon = new CronDaemon();
daemon.AddJob("* * * * * *", () =>
{
    // Do Work
    // This gets executed every minute.
});
daemon.Start();
```

### In a small console application

```csharp
namespace Application
{
    using Fluxter.CronDaemon;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var daemon = new CronDaemon();
            daemon.AddJob("* * * * * *", () =>
            {
                // Do Work
                // This gets executed every minute.
            });
            daemon.Start();
        }
    }
}
```
