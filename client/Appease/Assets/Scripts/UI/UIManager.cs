using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{

    using QNetwork;

    public class UIManager : MonoBehaviour
    {

        public static UIManager Singleton { get; private set; }

        [SerializeField]
        private GameObject startMenu;
        [SerializeField]
        private InputField usernameField;

        #region Singleton Architecture

        private void Start()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }
            Singleton = this;
        }

        private void OnDestroy()
        {
            if (Singleton == this)
                Singleton = null;
        }


        #endregion

        public void ConnectToServer()
        {
            startMenu.SetActive(false);
            usernameField.interactable = false;
            Client.Singleton.ConnectToServer();
        }

    }

}

