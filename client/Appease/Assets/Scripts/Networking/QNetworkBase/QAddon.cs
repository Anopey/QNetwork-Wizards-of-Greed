using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Game.QNetwork
{
    public abstract class QAddon: ScriptableObject
    {
        private void OnEnable()
        {
            NetworkManager.AddonInitializationEvent += OnNetworkManagerInitialized;
        }

        protected abstract void OnNetworkManagerInitialized();
    }
}
