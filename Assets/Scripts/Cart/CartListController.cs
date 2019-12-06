using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cart {
    public class CartListController : MonoBehaviour {
        public GameObject ContentPanel;
        public GameObject CartItemPrefab;
        private static string _URL_BASE = Shared._URL_BASE + "comics/";

        void Start() {
            LoadCartFromURL(_URL_BASE + Shared._USERNAME);
        }
    
        public void LoadCartFromURL(string url) {            
            // 1. Get information for each book.
            Shared._BOOK_INFO = Book.GetBooksFromServer(_URL_BASE, Shared._CART.Cart);
            
            // 2. Remove old display items.
            while(0 < ContentPanel.transform.childCount) {
                Transform child = ContentPanel.transform.GetChild(0);
                child.transform.parent= null;
                Destroy(child.gameObject);
            }
            
            // 3. Sort books.
            Shared._CART.SortByKey();

            // 4. Show books.
            foreach (Cart cart in Shared._CART.Cart) {
                string bookKey = cart.MakeKey();
                if (!Shared._BOOK_INFO.ContainsKey(bookKey)) { Debug.Log("Not found: " + bookKey); continue;}
                Book book = Shared._BOOK_INFO[bookKey];
                ImageHelper imageHelper = new ImageHelper();

                GameObject newCover = Instantiate(CartItemPrefab) as GameObject;
                CartController controller = newCover.GetComponent<CartController>();
                controller.Title.text = book.title+ " #" + book.issue;
                controller.Id = book.id;
                controller.Issue = book.issue;
                controller.Price = book.price;
                newCover.transform.SetParent(ContentPanel.transform, false);
                newCover.transform.localScale = Vector3.one;
            
                string coverURL = _URL_BASE + book.id + "/" + book.issue + "/" + book.pages[0];
                RawImage image = controller.Cover.GetComponent<RawImage>();
                StartCoroutine(imageHelper.SetImageFromURL(image, coverURL));
            }
        }
    }
}
