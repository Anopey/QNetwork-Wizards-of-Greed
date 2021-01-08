using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QNetwork;
using QNetwork.Infrastructure;
using QUnity.UI;
using UnityEngine.UI;

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

    [SerializeField]
    private Button submissionButton;

    [SerializeField]
    private List<GameObject> crystals;

    [SerializeField]
    private float upDownMotionAmplitude;

    [SerializeField]
    private float upDownMotionSpeed;

    private Coroutine upDownAnimationCoroutine;

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
        upDownAnimationCoroutine = StartCoroutine(CrystalUpDown(upDownMotionAmplitude, upDownMotionSpeed));
        if (PlayerPrefs.HasKey("Username") && reuseName)
        {
            usernameEntryParent.SetActive(false);
            InitializeConnectionToServerWithUsername(PlayerPrefs.GetString("Username"));
        }
    }

    #region Username Submission

    public void OnUsernameSubmit()
    {
        Debug.Log("Submitting Username...");

        string username = usernameInputField.text;

        InitializeConnectionToServerWithUsername(username);

        PlayerPrefs.SetString("Username", username);
    }



    #endregion

    #region Crystal Animations

    private Dictionary<GameObject, float> crystalToSinInput = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, Vector3> crystalToInitial = new Dictionary<GameObject, Vector3>();
    private IEnumerator CrystalUpDown(float amplitude, float speed)
    {
        crystalToSinInput.Clear();
        crystalToInitial.Clear();

        foreach (var crystal in crystals)
        {
            crystalToSinInput.Add(crystal, Random.Range(0, 2 * Mathf.PI));
            crystalToInitial.Add(crystal, crystal.transform.position);
        }


        while (true)
        {
            foreach (var crystal in crystals)
            {
                crystalToSinInput[crystal] += Time.deltaTime * speed;
                crystal.transform.position = crystalToInitial[crystal] + Vector3.up * amplitude * Mathf.Sin(crystalToSinInput[crystal]);
            }
            yield return new WaitForEndOfFrame();
        }

    }

    #endregion

    string username;

    private void InitializeConnectionToServerWithUsername(string username)
    {
        Client.Singleton.ConnectToServer(OnConnectionEstablished);

        submissionButton.interactable = false;

        this.username = username;
    }

    private void OnConnectionEstablished()
    {

        AcknowledgementHandler.Singleton.RegisterHandle(17, OnUsernameRecievedSuccessful, OnUsernameRecievedFailed);

        //set username
        Packet p = new Packet(17, new string[] { username });
        Client.Singleton.WriteToServer(17, p);

        usernameEntryParent.SetActive(false);
    }

    private void OnUsernameRecievedSuccessful()
    {
        Debug.Log("The server has accepted the username!");
    }
    
    private void OnUsernameRecievedFailed(string err)
    {
        Debug.LogError("The server has not accepted the username, returning:\n" + err);
    }

}
