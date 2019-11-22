using Reader;
using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Purchased {
    public class ListController : MonoBehaviour {
        public GameObject ContentPanel;
        public GameObject SortOrder;
        public GameObject ListItemPrefab;
        private static string _URL_BASE = Shared._URL_BASE + "comics/";
        private IEnumerator _coroutine;

        void Start() {
            SortOrder.GetComponent<Dropdown>().onValueChanged.AddListener(delegate { LoadOwned(); });
            LoadOwned();
        }

        public void LoadOwned() {
            _coroutine = LoadOwnedFromURL(_URL_BASE + Shared._USERNAME);
            StartCoroutine(_coroutine);
        }
    
        public IEnumerator LoadOwnedFromURL(string url) {
            // 1. Determine all the books the user owns.
            try {
                if (null == Shared._OWNED) {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string jsonResponse = reader.ReadToEnd();
                    
                    Shared._OWNED = JsonUtility.FromJson<OwnedEntryArray>("{ \"OwnedEntries\": " + jsonResponse + "}");
                }
                if (null == Shared._OWNED) {
                    // TODO: Error
                }
            } catch (Exception e) {
                // TODO: Error
            }

            // 2. Get information for each book.
            try {
                foreach (OwnedEntry ownedEntry in Shared._OWNED.OwnedEntries) {
                    string bookKey = ownedEntry.MakeKey();
                    if (Shared._BOOK_INFO.ContainsKey(bookKey)) { continue; }
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(_URL_BASE + "{0}/{1}", ownedEntry.id, ownedEntry.issue));
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string jsonResponse = reader.ReadToEnd();
                    Pages pages = JsonUtility.FromJson<Pages>(jsonResponse);
                    Shared._BOOK_INFO.Add(bookKey, pages);
                }
            } catch (Exception e) {
                // TODO: Error
            }

            // 3. Remove old display items.
            while(0 < ContentPanel.transform.childCount) {
                Transform child = ContentPanel.transform.GetChild(0);
                child.transform.parent= null;
                Destroy(child.gameObject);
                Debug.Log(ContentPanel.transform.childCount);
            }
            
            // 4. Sort books.
            Shared._OWNED.SortByKey();
            Dropdown sortPreference = SortOrder.GetComponent<Dropdown>();
            if (1 == sortPreference.value) { Shared._OWNED.SortByDatePurchased(); }

            // 5. Show books.
            foreach (OwnedEntry ownedEntry in Shared._OWNED.OwnedEntries) {
                string bookKey = ownedEntry.MakeKey();
                Pages pages = Shared._BOOK_INFO[bookKey];

                GameObject newCover = Instantiate(ListItemPrefab) as GameObject;
                ListItemController controller = newCover.GetComponent<ListItemController>();
                controller.Title.text = pages.title+ " #" + pages.issue;
                
                string coverURL = _URL_BASE;
                coverURL += pages.id + "/";
                coverURL += pages.issue + "/";
                coverURL += pages.pages[0];
                WWW www = new WWW(coverURL);
                while (!www.isDone)
                    yield return null;
                controller.Cover.GetComponent<RawImage>().texture = www.texture;
                newCover.transform.SetParent(ContentPanel.transform, false);
                newCover.transform.localScale = Vector3.one;
            }
        }
    }
    
    [Serializable]
    public class OwnedEntry
    {
        public string id;
        public int issue;
        public string purchased;
        public string MakeKey() {
            return  id + "_" + issue;
        }
    }
    
    [Serializable]
    public class OwnedEntryArray
    {
        public OwnedEntry[] OwnedEntries;
        public void SortByKey() {
            Array.Sort(OwnedEntries, delegate(OwnedEntry owned1, OwnedEntry owned2) { return owned1.MakeKey().CompareTo(owned2.MakeKey()); });
        }
        public void SortByDatePurchased() {
            // This is a stable sort.
            // http://www.csharp411.com/c-stable-sort/
            int count = Shared._OWNED.OwnedEntries.Length;
            for (int j = 1; j < count; j++) {
                OwnedEntry key = OwnedEntries[j];

                int i = j - 1;
                for (; i >= 0 && OwnedEntries[i].purchased.CompareTo( key.purchased ) > 0; i--) {
                    OwnedEntries[i + 1] = OwnedEntries[i];
                }
                OwnedEntries[i + 1] = key;
            }
            Array.Reverse( Shared._OWNED.OwnedEntries, 0, Shared._OWNED.OwnedEntries.Length );
        }
    }
}
