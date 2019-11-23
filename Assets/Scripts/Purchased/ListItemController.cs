using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Purchased {
    public class ListItemController : MonoBehaviour, IPointerClickHandler {
        public RawImage Cover;
        public Text Title;
        public string Id;
        public int Issue;
        
        public void OnPointerClick(PointerEventData eventData) // 3
        {
            Shared._CURRENT_BOOK_ID = Id;
            Shared._CURRENT_BOOK_ISSUE = Issue;
            SceneManager.LoadScene("Reader", LoadSceneMode.Single);
        }
    }
}