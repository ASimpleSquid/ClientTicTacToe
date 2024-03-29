using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class NetworkedClient : MonoBehaviour
{

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;
    GameObject gameSystemManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allobjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allobjects)
        {
            if (go.GetComponent<GameSystemManager>() != null)
            {
                gameSystemManager = go;
            }
        }
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    if (ourClientID == default(int)) ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    if (recConnectionID != ourClientID) return;
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    if (recConnectionID != ourClientID) return;
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }

    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.0.31", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }

    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');
        if (csv.Length > 0)
        {
            int signifier = int.Parse(csv[0]);
            if (signifier == ServerToClientSignifiers.LoginComplete)
            {
                Debug.Log("Login successful");
                if (csv.Length > 1)
                {
                    gameSystemManager.GetComponent<GameSystemManager>().updateUserName(csv[1]);
                }
                gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.MainMenu);
            }
            else if (signifier == ServerToClientSignifiers.LoginFailed)
                Debug.Log("Login Failed");
            else if (signifier == ServerToClientSignifiers.AccountCreationComplete)
            {
                Debug.Log("account creation successful");
                if (csv.Length > 1)
                {
                    gameSystemManager.GetComponent<GameSystemManager>().updateUserName(csv[1]);
                }
                gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.MainMenu);
            }
            else if (signifier == ServerToClientSignifiers.AccountCreationFailed)
                Debug.Log("Account creation failed");
            else if (signifier == ServerToClientSignifiers.JoinedPlay)
            {
                if (csv.Length > 2)
                    gameSystemManager.GetComponent<GameSystemManager>().updateChat("join player " + csv[2]);
                if (gameSystemManager.GetComponent<GameSystemManager>().getIsPlayer())
                    gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.MainMenu);
            }
            else if (signifier == ServerToClientSignifiers.RecievedMessage)
            {
                gameSystemManager.GetComponent<GameSystemManager>().updateChat($"{csv[2]} : { csv[1]}");
            }
            else if (signifier == ServerToClientSignifiers.JoinSuccess)
            {
                gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.GameRoom);
            }
        }
    }
    public bool IsConnected()
    {
        return isConnected;
    }
}
public static class ClientToServerSignifiers
{
    public const int CreateAccount = 1;
    public const int Login = 2;
    public const int SendMessage = 3;
    public const int InputField = 4;
    public const int PlayerAttemptJoin = 5;
    public const int ObserverJoin = 6;
    public const int LeaveGame = 7;
    public const int GameMove = 8;
}
public static class ServerToClientSignifiers
{
    public const int LoginComplete = 1;
    public const int LoginFailed = 2;
    public const int AccountCreationComplete = 3;
    public const int AccountCreationFailed = 4;
    public const int Chatbox = 5;
    public const int JoinedPlay = 6;
    public const int RecievedMessage = 7;
    public const int JoinSuccess = 8;
    public const int JoinFail = 9;
    public const int GameUpdate = 10;
}
