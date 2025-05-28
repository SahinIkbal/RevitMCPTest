# RevitMCPTest Revit Add-in

## Overview

`RevitMCPTest` is an Autodesk Revit add-in designed to demonstrate inter-process communication (IPC) using named pipes. It allows an external application to send messages to Revit, which are then displayed using a `TaskDialog` within the Revit user interface. This project utilizes Revit\'s External Event mechanism to safely interact with the Revit API from an external context.

## Features

*   **Named Pipe Server:** Implements a `NamedPipeServerStream` to listen for incoming messages from a client application on the pipe named "MyPipe".
*   **Revit External Event:** Uses `IExternalEventHandler` to process messages and trigger UI updates (TaskDialog) in Revit.
*   **Revit Application Entry Point:** Implements `IExternalApplication` for initialization during Revit startup.
*   **Dynamic Message Display:** Shows messages received from the client application in a Revit `TaskDialog`.

## Technologies Used

*   **Programming Language:** C# 12.0
*   **Framework:** .NET 8
*   **Revit API:** Interacts with Autodesk Revit (requires `RevitAPI.dll` and `RevitAPIUI.dll`).
*   **Inter-Process Communication:** Named Pipes

## Project Structure

*   `RevitAppEvents.cs`: Contains the main add-in logic, including:
    *   `App`: The `IExternalApplication` implementation for startup and shutdown.
    *   `SampleExternalEventHandler`: The `IExternalEventHandler` implementation to execute Revit API calls.
    *   `MCPExternalEvent`: A helper class to manage and raise the external event.
*   `PipeServer.cs`: Contains the `PipeServer` class responsible for:
    *   Creating and managing the named pipe server.
    *   Reading messages from connected clients.
    *   Invoking the `TaskDialog` via the external event.
*   `Builder.cs` (Implicit): A static class (assumed from `Builder.MyMessage`) likely used to pass the message content to the event handler.
*   `RevitMCPTest.addin`: The XML manifest file required by Revit to load the add-in.

## Setup and Usage

1.  **Build the Project:**
    *   Compile the `RevitMCPTest` project. This will produce `RevitMCPTest.dll` (typically in a `bin\Debug\net8.0` or `bin\Release\net8.0` folder).

2.  **Configure the `.addin` Manifest File:**
    *   Locate the existing `RevitMCPTest.addin` file in the project.
    *   Open it and ensure the `<Assembly>` path correctly points to the location of your compiled `RevitMCPTest.dll`. For example:    ```xml
    <RevitAddIns>
      <AddIn Type="Application">
        <Name>RevitMCPTest</Name>
        <!-- Update this path to the actual location of your DLL -->
        <Assembly>C:\\Path\\To\\Your\\Project\\RevitMCPTest\\bin\\Debug\\net8.0\\RevitMCPTest.dll</Assembly>
        <!-- The AddInId should ideally remain consistent. If a new unique ID is absolutely necessary, you can generate one. -->
        <AddInId>3c478521-32c4-46b1-966b-9e3327260a13</AddInId>
        <FullClassName>RevitMCPTest.App</FullClassName>
        <VendorId>USER</VendorId>
        <VendorDescription>Your Company, www.yourcompany.com</VendorDescription>
          </AddIn>
        </RevitAddIns>```    *   Verify that the `<AddInId>` is appropriate. If this add-in has been previously deployed or registered, it's best to keep the existing ID. The current ID in the project is `3c478521-32c4-46b1-966b-9e3327260a13`.

3.  **Deploy the Add-in:**
*   Copy the `RevitMCPTest.addin` file to one of the Revit Add-ins folders:
    *   For the current user: `%APPDATA%\\Autodesk\\Revit\\Addins\\[YourRevitVersion]`
    *   For all users: `%PROGRAMDATA%\\Autodesk\\Revit\\Addins\\[YourRevitVersion]`
    (Replace `[YourRevitVersion]` with your Revit version, e.g., `2023`, `2024`)

4.  **Run Revit:**
*   Start Autodesk Revit. The add-in will load, and the pipe server will start listening.

5.  **Send Messages:**
*   Use a separate client application to connect to the named pipe "MyPipe" and send string messages. Each message sent will be displayed in a `TaskDialog` in Revit.

## How It Works

1.  On Revit startup, the `App.OnStartup` method is called.
2.  It initializes the `MCPExternalEvent` and starts the `PipeServer.NamedPipeServer` method in a background task.
3.  The `PipeServer` creates a `NamedPipeServerStream` named "MyPipe" and waits for a client connection.
4.  When a client connects and sends a message, the `PipeServer` reads the message.
5.  The message is stored (e.g., in `Builder.MyMessage`).
6.  `MCPExternalEvent.Raise()` is called, which triggers the `SampleExternalEventHandler.Execute` method in the Revit UI context.
7.  The `Execute` method retrieves the stored message and displays it using `TaskDialog.Show()`.
8.  The server sends a "Dialog Shown" confirmation back to the client and continues to listen for more messages until the client disconnects.

This project serves as a basic example of how to integrate external processes with Revit for automation and custom workflows.
