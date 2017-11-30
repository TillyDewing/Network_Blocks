using UnityEngine;
using System.Collections;

public class MasterServerConsole : MonoBehaviour
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    Windows.ConsoleWindow console = new Windows.ConsoleWindow();
    Windows.ConsoleInput input = new Windows.ConsoleInput();

    string strInput;

    public NetworkBlocksServer server;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        console.Initialize();
        console.SetTitle("Celtic Survival Master Server");

        input.OnInputText += OnInputText;

        Application.logMessageReceived += HandleLog;

        Debug.Log("**************************************************");
        Debug.Log("*        Network Blocks Dedicated Server         *");
        Debug.Log("*              2017 William Dewing               *");
        Debug.Log("**************************************************");
        Debug.Log("Type 'help' for a list of commands");
        server.InitializeServer();
    }

    void OnInputText(string obj)
    {
        obj = obj.ToLower();
        //Debug.Log("On Input: " + obj);
        
        if(obj.Equals("stop"))
        {
            Debug.Log("Stopping Server");
            server.ShutDownServer();
        }
        else if (obj.Equals("start"))
        {
            Debug.Log("Starting Server");
            server.InitializeServer();
        }
        if (obj.Equals("quit"))
        {
            Debug.Log("Stopping Server");
            server.ShutDownServer();
            Debug.Log("Quiting");
            Application.Quit();
        }
        else if (obj.Equals("help"))
        {
            Debug.Log("start    - Starts the server");
            Debug.Log("stop     - Stops the Server");
            Debug.Log("quit     - quits the Server");
        }
    }

    void HandleLog(string message, string stackTrace, LogType type)
    {
        if (type == LogType.Warning)
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
        else if (type == LogType.Error)
            System.Console.ForegroundColor = System.ConsoleColor.Red;
        else
            System.Console.ForegroundColor = System.ConsoleColor.White;

        // We're half way through typing something, so clear this line ..
        if (System.Console.CursorLeft != 0)
        {
            input.ClearLine();
        }

        System.Console.WriteLine(message);

        // If we were typing something re-add it.
        input.RedrawInputLine();
    }

    //
    // Update the input every frame
    // This gets new key input and calls the OnInputText callback
    //
    void Update()
    {
        input.Update();
    }

    //
    // It's important to call console.ShutDown in OnDestroy
    // because compiling will error out in the editor if you don't
    // because we redirected output. This sets it back to normal.
    //
    void OnDestroy()
    {
        console.Shutdown();
    }

#endif
}
