using System;
using UnityEngine;
using UnityEngine.UI;

namespace Purchased {
    public class PurchasedListController : MonoBehaviour {
        public GameObject ContentPanel;
        public GameObject SortOrder;
        public GameObject ListItemPrefab;
        private static string _URL_BASE = Shared._URL_BASE + "comics/";

        void Start() {
            if (String.IsNullOrEmpty(Shared._USERNAME)) Shared._USERNAME = "dave";
            SortOrder.GetComponent<Dropdown>().onValueChanged.AddListener(delegate { LoadOwnedFromURL(_URL_BASE + Shared._USERNAME); });
            LoadOwnedFromURL(_URL_BASE + Shared._USERNAME);
        }
    
        public void LoadOwnedFromURL(string url) {
            Shared._PURCHASED = PurchasedList.GetFromServer(url);
            if (null == Shared._PURCHASED || 0 == Shared._PURCHASED.Purchased.Count) return;

            // 2. Get information for each book.
            Shared._BOOK_INFO = Book.GetPurchasedBooksFromServer(_URL_BASE, Shared._PURCHASED);
            
            // 3. Remove old display items.
            while(0 < ContentPanel.transform.childCount) {
                Transform child = ContentPanel.transform.GetChild(0);
                child.transform.parent= null;
                Destroy(child.gameObject);
            }
            
            // 4. Sort books.
            Dropdown sortPreference = SortOrder.GetComponent<Dropdown>();
            Debug.Log(sortPreference.value);
            Shared._PURCHASED.SortByKey();
            if (1 == sortPreference.value) { Shared._PURCHASED.SortByDatePurchased(); }

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
    }
}
