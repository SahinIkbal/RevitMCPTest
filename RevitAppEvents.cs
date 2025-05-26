using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;

namespace RevitMCPTest
{
    // Sample implementation of an External Event Handler for Revit MCP
    public class SampleExternalEventHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            // Example: Show a simple TaskDialog
            TaskDialog.Show("MCP External Event", "External Event Executed!");
        }

        public string GetName()
        {
            return "Sample MCP External Event Handler";
        }
    }

    public static class MCPExternalEvent
    {
        private static ExternalEvent _externalEvent;
        private static SampleExternalEventHandler _handler;

        public static ExternalEvent GetOrCreateExternalEvent()
        {
            if (_externalEvent == null)
            {
                _handler = new SampleExternalEventHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }
            return _externalEvent;
        }

        public static void Raise()
        {
            GetOrCreateExternalEvent().Raise();
        }
    }

    // Application class to initialize the external event on Revit startup
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Initialize the external event
            MCPExternalEvent.GetOrCreateExternalEvent();
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Clean up resources if necessary
            return Result.Succeeded;
        }
    }
}
