using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace Purchased {
    public class PurchasedList {
        public List<Purchased> Purchased = new List<Purchased>();
        public bool HasPurchase(string id, int issue, string purchased) {
            foreach (Purchased book in Purchased) {
                if (book.id != id || book.issue != issue || book.transactionDate != purchased) continue;
                return true;
            }
            return false;
        }
        public void AddPurchase(string id, int issue, string purchased) {
            if (HasPurchase(id, issue, purchased)) return;
            Purchased newIssue = new Purchased();
            newIssue.id = id;
            newIssue.issue = issue;
            newIssue.transactionDate = purchased;
            Purchased.Add(newIssue);
        }
        public void RemovePurchase(string id, int issue) {
            foreach (Purchased i in Purchased) {
                if (i.id != id || i.issue != issue) continue;
                Purchased.Remove(i);
                break;
            }
        }
        public string ToJSON() { return  JsonUtility.ToJson(Purchased.ToArray()); }
        public void FromJSON(string json) {
            Purchased[] objects = JsonHelper.getJsonArray<Purchased> (json);
            Purchased = new List<Purchased>(objects);
        }
        public void SortByKey() {
            IssueCompare purchasedCompare = new IssueCompare();
            Purchased.Sort(purchasedCompare); 
        }
        public void SortByDatePurchased() {
            // This is a stable sort.
            // http://www.csharp411.com/c-stable-sort/
            int count = Shared._PURCHASED.Purchased.Count;
            for (int j = 1; j < count; j++) {
                Purchased key = Purchased[j];

                int i = j - 1;
                for (; i >= 0 && Purchased[i].transactionDate.CompareTo( key.transactionDate ) > 0; i--) {
                    Purchased[i + 1] = Purchased[i];
                }
                Purchased[i + 1] = key;
            }
            Purchased.Reverse( );
        }
        public static PurchasedList GetFromServer(string url) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Headers["Authorization"] = Shared._AUTHORIATION;

                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                if ( HttpStatusCode.OK != response.StatusCode ) {
                    MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_UNABLE_TO_OBTAIN_BOOK_LIST);
                } else {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string jsonResponse = reader.ReadToEnd();

                    PurchasedList newList = new PurchasedList();
                    newList.FromJSON(jsonResponse);
                    return newList;
                }
            } catch (Exception e) {
                MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
            }
            return null;
        }
    }
}
