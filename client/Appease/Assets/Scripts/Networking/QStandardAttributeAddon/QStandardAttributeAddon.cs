using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.QNetwork.QStandardAttributeInfo
{

    using QNetwork;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Standard Attribute Addon", menuName = "QNetwork/Addons/StandardAttributeAddon", order = 1)]
    public class QStandardAttributeAddon : QAddon
    {
        protected override void OnNetworkManagerInitialized()
        {
            Debug.Log("QNetworking standard attribute addon is now being initialized...");
        }
    }
}
