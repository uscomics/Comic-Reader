using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StoreItem {
    public class SeeAlso : MonoBehaviour, IPointerClickHandler
    {
        public string Id;
        public int Issue;
        public GameObject StoreItemScript;
            
        public void OnPointerClick(PointerEventData eventData) // 3
        {
            Shared._CURRENT_BOOK_ID = Id;
            Shared._CURRENT_BOOK_ISSUE = Issue;
            StoreItemScript.SendMessage("LoadPage");
        }
    }
}
