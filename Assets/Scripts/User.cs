using System;
using System.IO;
using System.Net;
using UnityEngine;

[Serializable]
public class User {
    public string username;
    public string email;
    public string firstName;
    public string lastName;
    public static User GetFromServer(string url) {
        try {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Headers["Authorization"] = Shared._AUTHORIATION;
				
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            return JsonUtility.FromJson<User>(jsonResponse);
        } catch (Exception e) {
            MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
            return null;
        }
    }
}
