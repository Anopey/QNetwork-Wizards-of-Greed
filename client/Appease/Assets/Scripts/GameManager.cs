using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Singleton Architecture

    public static GameManager Singleton { get; private set; }

    private void Awake()
    {
        if(Singleton != null)
        {
            Debug.LogError("");
            Destroy(this);
            return;
        }
        Singleton = this;
    }

    private void OnDestroy()
    {
        if(Singleton == this)
        {
            Singleton = null;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
