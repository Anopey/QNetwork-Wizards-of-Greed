using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QNetwork;
using QUnity.UI;


public class MainMenuManager : MonoBehaviour
{

    [SerializeField]
    private float fadeDuration = 1f;

    [SerializeField]
    private bool reuseName = false;

    [SerializeField]
    private GameObject usernameEntryParent;

    [SerializeField]
    private TMP_InputField usernameInputField;

    #region Singleton Architecture
    public static MainMenuManager Singleton { get; private set; }

    private void Start()
    {
        if(Singleton != null)
        {
            Debug.LogError("Two instances of the main menu manager cannot exist at once!");
            return;
        }
        Singleton = this;
        Initialize();
    }
    #endregion

    private void Initialize()
    {
        if (PlayerPrefs.HasKey("Username") && !reuseName)
        {
            usernameEntryParent.SetActive(false);
            InitializeConnectionToServerWithUsername(PlayerPrefs.GetString("Username"));
        }
        else
        {
            usernameEntryParent.SetActive(true);
        }
    }

    public void OnUsernameSubmit()
    {
        Debug.Log("Submitting Username...");

        string username = usernameInputField.text;

        InitializeConnectionToServerWithUsername(username);

        //QUIUtility.Fade(usernameEntryParent, fadeDuration);

        PlayerPrefs.SetString("Username", username);

        usernameEntryParent.SetActive(false);
    }

    private void InitializeConnectionToServerWithUsername(string username)
    {
        Client.Singleton.ConnectToServer();

        //set username
        Packet p = new Packet(17, new string[] { username});
        Client.Singleton.WriteToServer(17, p);
    }

}
