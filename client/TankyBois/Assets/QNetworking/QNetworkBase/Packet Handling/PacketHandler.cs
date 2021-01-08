using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QNetwork
{
    [Serializable]
    public abstract class PacketHandler : ScriptableObject
    {

        public void Initialize() //TODO Initialize only once. Remember that this is a scriptable object.
        {
            OnGameStart();
        }

        /// <summary>
        /// This will only be called once at around Start() in play mode.
        /// </summary>
        protected abstract void OnGameStart();

    }

}
