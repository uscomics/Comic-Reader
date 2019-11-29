using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace Cart {
    [Serializable]
    public class CartList {
        public List<Cart> Cart = new List<Cart>();

        public bool HasCart(string id, int issue) {
            foreach (Cart fav in Cart) {
                if (fav.id != id || fav.issue != issue) continue;
                return true;
            }
            return false;
        }
        public Cart GetCart(string id, int issue) {
            foreach (Cart fav in Cart) {
                if (fav.id != id || fav.issue != issue) continue;
                return fav;
            }
            return null;
        }
        public void AddCart(string id, int issue, string addURL, Cart.Callback errorCallback = null, Cart.Callback successCallback = null) {
            if (HasCart(id, issue)) return;
            Cart newCart = new Cart();
            newCart.id = id;
            newCart.issue = issue;
            Cart.Add(newCart);
            Shared.GetEmptyScript()?.StartCoroutine(newCart.UpdateServer(addURL, errorCallback, successCallback));
        }
        public void RemoveCart(string id, int issue, string removeURL, Cart.Callback errorCallback = null, Cart.Callback successCallback = null) {
            foreach (Cart f in Cart) {
                if (f.id != id || f.issue != issue) continue;
                Cart.Remove(f);
                Shared.GetEmptyScript()?.StartCoroutine(f.UpdateServer(removeURL, errorCallback, successCallback));
                break;
            }
        }
        public void ToggleCart(string id, int issue, string addURL, string removeURL, Cart.Callback errorCallback = null, Cart.Callback successCallback = null) {
            if (HasCart(id, issue)) RemoveCart(id, issue, removeURL, errorCallback, successCallback);
            else AddCart(id, issue, addURL, errorCallback, successCallback);
        }
        public string GetTotalPrice() {
            float total = 0.0f;
            foreach (Cart f in Cart) {
                Book book = Shared._BOOK_INFO[f.MakeKey()];
                if (null == book) continue;
                if (!f.MarkedForRemoval) total += float.Parse(book.price);
            }
            return total.ToString("0.00");
        }
        public string ToJSON() {
            return JsonUtility.ToJson(Cart.ToArray());
        }
        public void FromJSON(string json) {
            Cart[] objects = JsonHelper.getJsonArray<Cart>(json);
            Cart = new List<Cart>(objects);
        }
        public void SortByKey() {
            IssueCompare issueCompare = new IssueCompare();
            Cart.Sort(issueCompare);
        }
        public static CartList GetFromServer(string url) {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Headers["Authorization"] = Shared._AUTHORIATION;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            if (HttpStatusCode.OK == response.StatusCode || HttpStatusCode.NotFound == response.StatusCode) {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string jsonResponse = reader.ReadToEnd();
                CartList newList = new CartList();
                newList.FromJSON(jsonResponse);
                return newList;
            } else {
                MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
                return null;
            }
        }

    }
}
