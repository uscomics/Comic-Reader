using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Account {
    public class Account {
        public static readonly string _DEFAULT_DESTINATION = "./src/authentication/authentication.json";
        public string username;
        public string password;
        public string email;
        public string firstName;
        public string lastName;
        public string group;

        public delegate void Callback();

        public Account(string inUsername, string inPassword, string inEmail, string inFirstName, string inLastName, string inGroup) {
            username = inUsername;
            password = inPassword;
            email = inEmail;
            firstName = inFirstName;
            lastName = inLastName;
            group = inGroup;
        }

        public int Validate() {
            if (0 == username.Length) return Messages.ERROR_USERNAME_REQUIRED;
            if (5 > username.Length) return Messages.ERROR_INVALID_USERNAME_LENGTH;
            if (!Regex.IsMatch(username, "[a-zA-Z0-9-_]")) return Messages.ERROR_INVALID_USERNAME;
            if (0 == password.Length) return Messages.ERROR_PASSWORD_REQUIRED;
            if (5 > password.Length) return Messages.ERROR_PASSWORD_LENGTH;
            if (0 < email.Length && 4 > email.Length) return Messages.ERROR_INVALID_EMAIL;
            if (0 < email.Length && 0 == email.IndexOf('@')) return Messages.ERROR_INVALID_EMAIL;
            return 0;
        }

        public IEnumerator PostToServer(string url, string inDestiantion, Callback errorCallback = null, Callback successCallback = null) {
            int errorCode = Validate();
            if (0 != errorCode) {
                MessageManager.INSTANCE.ShowImageMessage(errorCode);
                errorCallback?.Invoke();
                yield break; // exit
            }

            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            form.AddField("email", email);
            form.AddField("firstName", firstName);
            form.AddField("lastName", lastName);
            form.AddField("group1", group);
            form.AddField("destination", inDestiantion);
            UnityWebRequest www = UnityWebRequest.Post(Shared._URL_BASE + "user/add/data", form);
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

            MessageManager.INSTANCE.ShowImageMessage(Messages.SUCCESS_ACCOUNT_ADDED, MessageManager.Sound.UseSuccessSound);
            yield return new WaitForSeconds(5);
            successCallback?.Invoke();
        }
    }
}
