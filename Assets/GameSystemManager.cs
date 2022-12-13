using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    GameObject LogIn, Username, Password, NewUser, Title;
    GameObject LogInPage;
    public GameObject networkedClient;
    string currentPlayerName = "";
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
                default:

                    break;
            }
        }
        LogIn.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        NewUser.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);

        ChangeState(GameStates.LoginMenu);
    }

    // Update is called once per frame
    void Update()
    {

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
            LogIn.SetActive(true);
            NewUser.SetActive(true);
            Password.SetActive(true);
            Username.SetActive(true);
            Title.SetActive(true);
        }
        else if (newState == GameStates.MainMenu)
        {
            LogIn.SetActive(false);
            NewUser.SetActive(false);
            Password.SetActive(false);
            Username.SetActive(false);
            Title.SetActive(false);
        }
    }

}

static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
}