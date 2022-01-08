using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProcessKiller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ProcessKiller processKiller;
            using var loggerFactory = LoggerFactory.Create(builder => 
            { 
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole(); 
            }); 
            ILogger logger = loggerFactory.CreateLogger<ProcessKiller>();
            
            try
            {
                processKiller = new ProcessKiller(args[0], args[1], args[2], logger);
            }
            catch (Exception e) when (e is ProcessNotFoundException or ArgumentException)
            {
                logger.LogError(e.Message);
                return;
            }
            catch (IndexOutOfRangeException)
            {
                logger.LogError("Usage: ProcessKiller.exe [process name] [check time] [time to live]");
                return;
            }

            await processKiller.CancelableKiller();
        }
    }
}
