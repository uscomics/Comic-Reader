using System;
using UnityEngine;
using UnityEngine.UI;

namespace Favorites {
    public class FavoritesListController : MonoBehaviour {
        public GameObject ContentPanel;
        public GameObject FavoritePrefab;
        private static string _URL_BASE = Shared._URL_BASE + "comics/";

        void Start() {
            if (String.IsNullOrEmpty(Shared._USERNAME)) {
                Shared._USERNAME = "dave";
                Shared._FAVORITES = FavoritesList.GetFromServer(String.Format(Shared._URL_BASE + "favorites/{0}", Shared._USERNAME));
            }
            LoadFavoritesFromURL(_URL_BASE + Shared._USERNAME);
        }
    
        public void LoadFavoritesFromURL(string url) {
            // 2. Get information for each book.
            Shared._BOOK_INFO = Book.GetFavoriteBooksFromServer(_URL_BASE, Shared._FAVORITES);
            
            // 3. Remove old display items.
            while(0 < ContentPanel.transform.childCount) {
                Transform child = ContentPanel.transform.GetChild(0);
                child.transform.parent= null;
                Destroy(child.gameObject);
            }
            
            // 4. Sort books.
            Shared._FAVORITES.SortByKey();

            // 5. Show books.
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
