using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    GameObject LogIn, Username, Password, NewUser, Title, Chatbox, InputField, Send;
    GameObject LogInPage, Chatroom;
    public GameObject networkedClient;
    string currentPlayerName = "";
    public int playerNumber = 0;
    bool isPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allobjects = FindObjectsOfType<GameObject>();
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
                    Debug.Log("Password found chucklenuts");
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
                    Title = go;
                    break;
                case "InputField":
                    NewUser = go;
                    break;
                case "Send":
                    LogInPage = go;
                    break;
                case "Chatroom":
                    Title = go;
                    break;
                default:

                    break;
            }
        }
        LogIn.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        NewUser.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);
        Send.GetComponent<Button>().onClick.AddListener(SendButtonPressed);

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
        Chatbox.GetComponent<TMP_Text>().text += msg + "\n";
    }
    public void SendButtonPressed()
    {
        string msg = ClientToServerSignifiers.SendMessage + "," + InputField.GetComponent<InputField>().text + "," + currentPlayerName;
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
        }
        else if (newState == GameStates.MainMenu)
        {
            LogInPage.SetActive(false);
            Chatroom.SetActive(true);
        }
    }

}

static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueue = 3;
    public const int Chatting = 4;
}