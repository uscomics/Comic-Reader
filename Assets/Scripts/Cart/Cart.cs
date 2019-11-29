using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Cart {
    [Serializable]
    public class Cart : Favorites.Favorite {
        public bool MarkedForRemoval = false;
    }
}
