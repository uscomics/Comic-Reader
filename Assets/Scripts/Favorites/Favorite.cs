using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Favorites {
    [Serializable]
    public class Favorite : Issue {
        public string transactionDate = "";
        public delegate void Callback();

        public IEnumerator UpdateServer(string url, Callback errorCallback = null, Callback successCallback = null) {    
            WWWForm form = new WWWForm();
            form.AddField("username", Shared._USERNAME);
            form.AddField("id", id);
            form.AddField("issue", issue);
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
    
            if (www.isNetworkError) {
                MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
                errorCallback?.Invoke();
                yield break; // exit
            } else if (www.isHttpError) {
                if (null != www.downloadHandler.text) MessageManager.INSTANCE.ShowImageMessage(www.downloadHandler.text);
                else MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
                errorCallback?.Invoke();
                yield break; // exit
            }
            successCallback?.Invoke();
        }
    }
}
