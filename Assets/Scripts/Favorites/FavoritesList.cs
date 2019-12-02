using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace Favorites {
    public class FavoritesList {
        public List<Favorite> Favorites = new List<Favorite>();

        public bool HasFavorite(string id, int issue) {
            foreach (Favorite fav in Favorites) {
                if (fav.id != id || fav.issue != issue) continue;
                return true;
            }
            return false;
        }
        public Favorite GetFavorite(string id, int issue) {
            foreach (Favorite fav in Favorites) {
                if (fav.id != id || fav.issue != issue) continue;
                return fav;
            }
            return null;
        }
        public void AddFavorite(string id, int issue, string addURL, Favorite.Callback errorCallback = null, Favorite.Callback successCallback = null) {
            if (HasFavorite(id, issue)) return;
            Favorite newFavorite = new Favorite();
            newFavorite.id = id;
            newFavorite.issue = issue;
            Favorites.Add(newFavorite);
            Shared.GetEmptyScript()?.StartCoroutine(newFavorite.UpdateServer(addURL, errorCallback, successCallback));
        }
        public void RemoveFavorite(string id, int issue, string removeURL, Favorite.Callback errorCallback = null, Favorite.Callback successCallback = null) {
            foreach (Favorite f in Favorites) {
                if (f.id != id || f.issue != issue) continue;
                Favorites.Remove(f);
                Shared.GetEmptyScript()?.StartCoroutine(f.UpdateServer(removeURL, errorCallback, successCallback));
                break;
            }
        }
        public void ToggleFavorite(string id, int issue, string addURL, string removeURL, Favorite.Callback errorCallback = null, Favorite.Callback successCallback = null) {
            if (HasFavorite(id, issue)) RemoveFavorite(id, issue, removeURL, errorCallback, successCallback);
            else AddFavorite(id, issue, addURL, errorCallback, successCallback);
        }
        public string ToJSON() {
            string json = "[ ";
            for (int i = 0; i < Favorites.Count; i++) {
                if (0 < i) json += ", ";
                json += JsonUtility.ToJson(Favorites[i]);
            }
            json += " ]";
            return json;
        }
        public void FromJSON(string json) {
            Favorite[] objects = JsonHelper.getJsonArray<Favorite>(json);
            Favorites = new List<Favorite>(objects);
        }
        public void SortByKey() {
            IssueCompare issueCompare = new IssueCompare();
            Favorites.Sort(issueCompare);
        }
        public void SortByDate() {
            // This is a stable sort.
            // http://www.csharp411.com/c-stable-sort/
            int count = Shared._FAVORITES.Favorites.Count;
            for (int j = 1; j < count; j++) {
                Favorite key = Favorites[j];

                int i = j - 1;
                for (; i >= 0 && Favorites[i].transactionDate.CompareTo( key.transactionDate ) > 0; i--) {
                    Favorites[i + 1] = Favorites[i];
                }
                Favorites[i + 1] = key;
            }
            Favorites.Reverse( );
        }
        public static FavoritesList GetFromServer(string url) {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Headers["Authorization"] = Shared._AUTHORIATION;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            if (HttpStatusCode.OK == response.StatusCode || HttpStatusCode.NotFound == response.StatusCode) {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string jsonResponse = reader.ReadToEnd();
                FavoritesList newList = new FavoritesList();
                newList.FromJSON(jsonResponse);
                return newList;
            } else {
                MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
                return null;
            }
        }

    }
}
