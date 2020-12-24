﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Networking
{

    public class NetworkManager : MonoBehaviour
    {

        public static NetworkManager Singleton { get; private set; }

        [SerializeField]
        private PacketManagerArgs packetManagerArgs;

        public PacketManager PacketManager { get; private set; }

        #region Singleton Architecture

        private void Awake()
        {
            if(Singleton != null)
            {
                Destroy(gameObject);
                return;
            }
            Singleton = this;
            Initialize();
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

        #region Thread Management

        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        private void Update()
        {
            UpdateMain();
        }

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
        private static void UpdateMain()
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