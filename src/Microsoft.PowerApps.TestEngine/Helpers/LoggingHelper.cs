﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.PowerApps.TestEngine.Config;
using Microsoft.PowerApps.TestEngine.PowerApps;
using Microsoft.PowerApps.TestEngine.System;
using Microsoft.PowerApps.TestEngine.TestInfra;

namespace Microsoft.PowerApps.TestEngine.Helpers
{
    public class LoggingHelper
    {
        private readonly IPowerAppFunctions _powerAppFunctions;
        private readonly ISingleTestInstanceState _singleTestInstanceState;
        private ILogger Logger { get { return _singleTestInstanceState.GetLogger(); } }

        public LoggingHelper(IPowerAppFunctions powerAppFunctions,
                             ISingleTestInstanceState singleTestInstanceState)
        {
            _powerAppFunctions = powerAppFunctions;
            _singleTestInstanceState = singleTestInstanceState;
        }

        public async void DebugInfo(ITestEngineConsoleEvents consoleEventHandler)
        {
            try
            {
                ExpandoObject debugInfo = (ExpandoObject)await _powerAppFunctions.GetDebugInfo();
                if (debugInfo != null && debugInfo.ToString() != "undefined")
                {
                    Logger.LogTrace($"------------------------------\n Debug Info \n------------------------------");
                    foreach (var info in debugInfo)
                    {
                        Logger.LogTrace($"{info.Key}:\t{info.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogDebug("Issue getting DebugInfo. This can be a result of not being properly logged in.");
                Logger.LogDebug(ex.ToString());
                consoleEventHandler?.EncounteredCriticalIssue("Could not access PowerApps. Confirm you are logged in properly.");
            }
        }
    }
}
