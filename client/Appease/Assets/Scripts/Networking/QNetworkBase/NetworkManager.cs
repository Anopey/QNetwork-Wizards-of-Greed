using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using QUnity.Utility;
using System.Reflection;

namespace Game.QNetwork
{

    public class NetworkManager : MonoBehaviour
    {

        public static NetworkManager Singleton { get; private set; }

        [SerializeField]
        private PacketManagerArgs packetManagerArgs;

        public PacketManager PacketManager { get; private set; }

        public static event Action AddonInitializationEvent; //dont want addons to be monobehaviours too to not clutter up scene so doing it like this.

        #region Singleton Architecture

        private void Start()
        {
            Debug.Log("Initializing NetworkManager");
            if(Singleton != null)
            {
                Debug.LogError("Two instances of NetworkManager cannot exist!");
                return;
            }
            Debug.Log("NetworkManager Initialized.");
            Singleton = this;
            DontDestroyOnLoad(this);
            Initialize();
            InitializeAddons();
        }

        private void OnDestroy()
        {
            if (Singleton == this)
                Singleton = null;
        }

        #endregion

        private void Initialize()
        {
            PacketManager = new PacketManager(packetManagerArgs);
        }

        private void InitializeAddons()
        {
            AddonInitializationEvent?.Invoke();
        }

        #region Thread Management

        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="_action">The action to be executed on the main thread.</param>
        public static void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Debug.Log("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(_action);
                actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        private void Update()
        {
            if (actionToExecuteOnMainThread)
            {
                executeCopiedOnMainThread.Clear();
                lock (executeOnMainThread)
                {
                    executeCopiedOnMainThread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
                {
                    executeCopiedOnMainThread[i]();
                }
            }
        }

        #endregion



    }

}