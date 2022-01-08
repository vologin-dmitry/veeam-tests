using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProcessKiller
{
    public class ProcessKiller
    {
        private readonly int _timeToLive;
        private readonly int _timeToCheck;
        private readonly Process[] _processes;
        private readonly DateTime[] _processStartTime;
        private readonly ILogger _logger;
        
        public ProcessKiller(string processName, string timeToLive, string timeToCheck, ILogger logger)
        {
            _logger = logger;
            if (!(int.TryParse(timeToLive, out _timeToLive) && int.TryParse(timeToCheck, out _timeToCheck)))
            {
                throw new ArgumentException("Please input integer values as the second and the third parameter");
            }
            
            _processes = Process.GetProcessesByName(processName);
            if (_processes.Length == 0)
            {
                throw new ProcessNotFoundException("Sorry! Process not found");
            }

            _processStartTime = _processes.Select(proc => proc.StartTime).ToArray();
        }
        
        public async Task CancelableKiller()
        {
            var tokenSource = new CancellationTokenSource();
            var cancelTask = Task.Run(() => Cancel(tokenSource), tokenSource.Token);
            await Task.WhenAny(CheckAndKill(tokenSource.Token), cancelTask);
            tokenSource.Cancel();
        }
        
        private async Task CheckAndKill(CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                try
                {
                    if (!TryToKill())
                    {
                        for (var i = 0; i < _timeToLive; ++i)
                        {
                            if (!token.IsCancellationRequested)
                            {
                                await Task.Delay(TimeSpan.FromMinutes(_timeToCheck), token);
                                if (TryToKill())
                                    return;
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                { 
                    _logger.LogInformation("Application cancelled!");
                }
            }
            _logger.LogInformation("Finished");
        }
        
        private bool TryToKill()
        {
            var timeNow = DateTime.Now.TimeOfDay;
            for (var j = 0; j < _processes.Length; ++j)
            {
                _logger.LogInformation($"Process lifetime: " + (timeNow - _processes[j].StartTime.TimeOfDay));
                if ((DateTime.Now - _processStartTime[j]) <= TimeSpan.FromMinutes(_timeToLive)) continue;
                _processes[j].Kill();
                _logger.LogInformation($"Process {_processes[j].ProcessName} killed");
                return true;
            }

            return false;
        }

        private Task Cancel(CancellationTokenSource tokenSource)
        {
            _logger.LogInformation("Press 'q' to exit");
            while (char.ToUpper(Console.ReadKey(true).KeyChar) != 'Q')
            {
            }

            _logger.LogInformation("\n q key pressed: cancelling task.\n");
            tokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
