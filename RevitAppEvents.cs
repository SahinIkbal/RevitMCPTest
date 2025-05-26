using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.IO.Pipes;

namespace RevitMCPTest
{
    // Sample implementation of an External Event Handler for Revit MCP
    public class SampleExternalEventHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            // Example: Show a simple TaskDialog
            TaskDialog.Show("MCP External Event", PipeServer.MyMessage);
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

        public static ExternalEventRequest Raise()
        {
           return GetOrCreateExternalEvent().Raise();
        }
    }

    // Application class to initialize the external event on Revit startup
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            Task.Run(PipeServer.NamedPipeServer);
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
