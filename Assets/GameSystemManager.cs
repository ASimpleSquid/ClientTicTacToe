using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    GameObject LogIn, Username, Password, NewUser, Title, Chatbox, Input, Send, History, JoinObserve, JoinPlayer;
    GameObject LogInPage, Chatroom, JoinButtons, GameBoard;
    GameObject TR, T, TL, CR, C, CL, BR, B, BL, Leave;
    public GameObject networkedClient;
    string currentPlayerName = "";
    public int playerNumber = 0;
    bool isPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allobjects = FindObjectsOfType<GameObject>(true);
        foreach (GameObject go in allobjects)
        {
            switch(go.name)
            {
                case "LogIn":
                    LogIn = go;
                    break;
                case "Username":
                    Username = go;
                    break;
                case "Password":
                    Password = go;
                    break;
                case "NewUser":
                    NewUser = go;
                    break;
                case "LogInPage":
                    LogInPage = go;
                    break;
                case "Title":
                    Title = go;
                    break;
                case "Chatbox":
                    Chatbox = go;
                    break;
                case "Input":
                    Input = go;
                    break;
                case "Send":
                    Send = go;
                    break;
                case "Chatroom":
                    Chatroom = go;
                    break;
                case "History":
                    History = go;
                    break;
                case "JoinObserve":
                    JoinObserve = go;
                    break;
                case "JoinPlayer":
                    JoinPlayer = go;
                    break;
                case "JoinButtons":
                    JoinButtons = go;
                    break;
                case "GameBoard":
                    GameBoard = go;
                    break;
                case "TR":
                    TR = go;
                    break;
                case "T":
                    T = go;
                    break;
                case "TL":
                    TL = go;
                    break;
                case "CR":
                    CR = go;
                    break;
                case "C":
                    C = go;
                    break;
                case "CL":
                    CL = go;
                    break;
                case "BR":
                    BR = go;
                    break;
                case "B":
                    B = go;
                    break;
                case "BL":
                    BL = go;
                    break;
                case "Leave":
                    Leave = go;
                    break;
                default:

                    break;
            }
        }
        LogIn.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        NewUser.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);
        Send.GetComponent<Button>().onClick.AddListener(SendButtonPressed);
        JoinPlayer.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        JoinObserve.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);
        Leave.GetComponent<Button>().onClick.AddListener(SendButtonPressed);

        ChangeState(GameStates.LoginMenu);
    }
    public void addToChat(string msg)
    {
        Chatbox.GetComponent<TMP_Text>().text += msg + "\n";
    }
    // Update is called once per frame
    void Update()
    {

    }
    public bool getIsPlayer()
    {
        return isPlayer;
    }
    public void updateChat(string msg)
    {
        Debug.Log($"Message: {msg}");
        History.GetComponent<TMP_Text>().text += msg + "\n";
    }
    public void SendButtonPressed()
    {
        string msg = ClientToServerSignifiers.SendMessage + "," + Input.GetComponent<TMP_InputField>().text + "," + currentPlayerName;
        Debug.Log("msg:" + msg);
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        Debug.Log("send " + msg);
    }
    public void SubmitButtonPressed()
    {
        Debug.Log("button");
        string p = Password.GetComponent<TMP_InputField>().text;
        string n = Username.GetComponent<TMP_InputField>().text;
        string msg;
        if (NewUser.GetComponent<Toggle>().isOn)
            msg = ClientToServerSignifiers.CreateAccount + "," + n + "," + p;
        else
            msg = ClientToServerSignifiers.Login + "," + n + "," + p;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void CreateToggleChanged(bool newValue)
    {

    }
    public void JoinButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.PlayerAttemptJoin}");
    }
    public void LeaveButtonPressed()
    {

    }
    public void TRButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{2}");
    }
    public void TButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{1}");
    }
    public void TLButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{0}");
    }
    public void CRButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{5}");
    }
    public void CButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{4}");
    }
    public void CLButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{3}");
    }
    public void BRButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{8}");
    }
    public void BButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{7}");
    }
    public void BLButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost($"{ClientToServerSignifiers.GameMove},{6}");
    }
    public void updateUserName(string name)
    {
        currentPlayerName = name;
    }
    public void ChangeState(int newState)
    {

        if (newState == GameStates.LoginMenu)
        {
            LogInPage.SetActive(true);
            Chatroom.SetActive(false);
            GameBoard.SetActive(false);
            JoinButtons.SetActive(false);
        }
        else if (newState == GameStates.MainMenu)
        {
            LogInPage.SetActive(false);
            Chatroom.SetActive(true);
            GameBoard.SetActive(false);
            JoinButtons.SetActive(true);
        }
        else if(newState == GameStates.GameRoom)
        {
            LogInPage.SetActive(false);
            Chatroom.SetActive(true);
            GameBoard.SetActive(true);
            JoinButtons.SetActive(false);
        }
    }

}

static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueue = 3;
    public const int Chatting = 4;
    public const int GameRoom = 5;
}