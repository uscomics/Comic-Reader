using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StoreFront {
    public class StoreFrontListController : MonoBehaviour {
        public GameObject ContentPanel;
        public GameObject ListItemPrefab;
        private static string _URL_BASE = Shared._URL_BASE + "comics/";

        void Start() {
            Shared.CleanupCart();
            GetFromServer(_URL_BASE + Shared._USERNAME);
        }
    
        public void GetFromServer(string url) {
            // 1. Get information for each book.
            IssueList books = IssueList.GetAllBooksFromServer(_URL_BASE);
            
            // 2. Get info about each book
            Shared._BOOK_INFO = Book.GetBooksFromServer(_URL_BASE, books.Issues);
            
            // 3. Remove old display items.
            while(0 < ContentPanel.transform.childCount) {
                Transform child = ContentPanel.transform.GetChild(0);
                child.transform.parent= null;
                Destroy(child.gameObject);
            }

            // 4. Show books.
            foreach (KeyValuePair<string, Book> book in Shared._BOOK_INFO) {
                ImageHelper imageHelper = new ImageHelper();

                GameObject newCover = Instantiate(ListItemPrefab) as GameObject;
                StoreFrontController controller = newCover.GetComponent<StoreFrontController>();
                controller.Title.text = book.Value.title + " #" + book.Value.issue;
                controller.Id = book.Value.id;
                controller.Issue = book.Value.issue;
                newCover.transform.SetParent(ContentPanel.transform, false);
                newCover.transform.localScale = Vector3.one;
            
                string coverURL = _URL_BASE + book.Value.id + "/" + book.Value.issue + "/" + book.Value.pages[0];
                RawImage image = controller.Cover.GetComponent<RawImage>();
                StartCoroutine(imageHelper.SetImageFromURL(image, coverURL));
            }
        }
    }
}
