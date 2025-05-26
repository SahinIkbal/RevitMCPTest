using System.IO.Pipes;

namespace RevitMCPTest
{
    internal class PipeServer
    {
        public static string MyMessage { get; set; } 
        public static void NamedPipeServer()
        {

            

            string? message;
            while (true)
            {
                using var server = new NamedPipeServerStream("MyPipe", PipeDirection.InOut);
                server.WaitForConnection();

                using var reader = new StreamReader(server);
                using var writer = new StreamWriter(server) { AutoFlush = true };
                message = reader.ReadLine();
                string returnMessage = TaskDialog(message);
                writer.WriteLine(returnMessage); // Send a response back to the client
            }
        }

        public static string TaskDialog(string message)
        {
            PipeServer.MyMessage = message;
            string state = MCPExternalEvent.Raise().ToString();
            return state;
        }
    }
}
