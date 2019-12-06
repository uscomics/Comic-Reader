using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Purchased {
    public class PurchasedListController : MonoBehaviour {    
        public GameObject ContentPanel;
        public Dropdown SortOrder;
        public GameObject ListItemPrefab;
        private static string _URL_BASE = Shared._URL_BASE + "comics/";

        void Start() {
            SortOrder.onValueChanged.AddListener(delegate { GetFromServer(_URL_BASE + Shared._USERNAME); });
            Shared.CleanupCart();
            GetFromServer(_URL_BASE + Shared._USERNAME);
        }
    
        public void GetFromServer(string url) {
            // 1. Get purchased books.
            Shared._PURCHASED = PurchasedList.GetFromServer(url);
            if (null == Shared._PURCHASED || 0 == Shared._PURCHASED.Purchased.Count) return;

            // 2. Get information for each book.
            Shared._BOOK_INFO = Book.GetBooksFromServer(_URL_BASE, Shared._PURCHASED.Purchased);
            
            // 3. Remove old display items.
            while(0 < ContentPanel.transform.childCount) {
                Transform child = ContentPanel.transform.GetChild(0);
                child.transform.parent= null;
                Destroy(child.gameObject);
            }
            
            // 4. Sort books.
            Dropdown sortPreference = SortOrder.GetComponent<Dropdown>();
            Shared._PURCHASED.SortByKey();
            if (1 == sortPreference.value) { Shared._PURCHASED.SortByDate(); }

            // 5. Show books.
            foreach (Purchased purchased in Shared._PURCHASED.Purchased) {
                string bookKey = purchased.MakeKey();
                if (!Shared._BOOK_INFO.ContainsKey(bookKey)) { Debug.Log("Not found: " + bookKey); continue;}
                Book book = Shared._BOOK_INFO[bookKey];
                ImageHelper imageHelper = new ImageHelper();

                GameObject newCover = Instantiate(ListItemPrefab) as GameObject;
                PurchasedController controller = newCover.GetComponent<PurchasedController>();
                controller.Title.text = book.title+ " #" + book.issue;
                controller.Id = book.id;
                controller.Issue = book.issue;
                newCover.transform.SetParent(ContentPanel.transform, false);
                newCover.transform.localScale = Vector3.one;
            
                string coverURL = _URL_BASE + book.id + "/" + book.issue + "/" + book.pages[0];
                RawImage image = controller.Cover.GetComponent<RawImage>();
                StartCoroutine(imageHelper.SetImageFromURL(image, coverURL));
            }
        }
        public void Home() {
            SceneManager.LoadScene("StoreFront", LoadSceneMode.Single);
        }
    }
}
