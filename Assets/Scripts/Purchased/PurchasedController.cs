﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Purchased {
    public class PurchasedController : MonoBehaviour, IPointerClickHandler {
        public RawImage Cover;
        public Text Title;
        public string Id;
        public int Issue;
        
        public void OnPointerClick(PointerEventData eventData) {
            Shared._CURRENT_BOOK_ID = Id;
            Shared._CURRENT_BOOK_ISSUE = Issue;
            SceneManager.LoadScene("Reader", LoadSceneMode.Single);
        }
    }
}