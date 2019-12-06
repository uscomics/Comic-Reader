using System;
using UnityEngine;
using UnityEngine.UI;

namespace Favorites {
    public class FavoritesListController : MonoBehaviour {
        public GameObject ContentPanel;
        public GameObject FavoritePrefab;
        public Dropdown SortOrder;
        public static string _URL_BASE = Shared._URL_BASE + "comics/";

        void Start() {
            Shared.CleanupCart();
            SortOrder.onValueChanged.AddListener(delegate { GetFromServer(_URL_BASE + Shared._USERNAME); });
            GetFromServer(_URL_BASE + Shared._USERNAME);
        }
    
        public void GetFromServer(string url) {
            // 1. Get information for each book.
            Shared._BOOK_INFO = Book.GetBooksFromServer(_URL_BASE, Shared._FAVORITES.Favorites);
            
            // 2. Remove old display items.
            while(0 < ContentPanel.transform.childCount) {
                Transform child = ContentPanel.transform.GetChild(0);
                child.transform.parent= null;
                Destroy(child.gameObject);
            }
            
            // 3. Sort books.
            Dropdown sortPreference = SortOrder.GetComponent<Dropdown>();
            Shared._FAVORITES.SortByKey();
            if (1 == sortPreference.value) { Shared._FAVORITES.SortByDate(); }

            // 4. Show books.
            foreach (Favorite fav in Shared._FAVORITES.Favorites) {
                string bookKey = fav.MakeKey();
                if (!Shared._BOOK_INFO.ContainsKey(bookKey)) { Debug.Log("Not found: " + bookKey); continue;}
                Book book = Shared._BOOK_INFO[bookKey];
                ImageHelper imageHelper = new ImageHelper();

                GameObject newCover = Instantiate(FavoritePrefab) as GameObject;
                FavoriteController controller = newCover.GetComponent<FavoriteController>();
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
