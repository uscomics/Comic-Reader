using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResetPassword : MonoBehaviour
{
    public InputField EmailText;
    public Button ResetPasswordButton;
    public delegate void Callback();

    void Start() {
        ResetPasswordButton.GetComponent<Button>().onClick.AddListener(Reset); 
    }
    void Update() {
    }
    public void Reset() {
        StartCoroutine(PostToServer(Shared._URL_BASE + "user/password/reset/request/data"));
    }

    public int Validate() {
        if (0 == EmailText.text.Length) return Messages.ERROR_EMAIL_REQUIRED;
        if (0 < EmailText.text.Length && 4 > EmailText.text.Length) return Messages.ERROR_INVALID_EMAIL;
        if (0 < EmailText.text.Length && 0 == EmailText.text.IndexOf('@')) return Messages.ERROR_INVALID_EMAIL;
        return 0;
    }
    public IEnumerator PostToServer(string url, Callback errorCallback = null, Callback successCallback = null) {
        int errorCode = Validate();
        if (0 != errorCode) {
            MessageManager.INSTANCE.ShowImageMessage(errorCode);
            errorCallback?.Invoke();
            yield break; // exit
        }

        WWWForm form = new WWWForm();
        form.AddField("email", EmailText.text);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
            MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
            errorCallback?.Invoke();
            yield break; // exit
        } else if (www.isHttpError) {
            Debug.Log(www.error);
            if (null != www.downloadHandler.text) MessageManager.INSTANCE.ShowImageMessage(www.downloadHandler.text);
            else MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
            errorCallback?.Invoke();
            yield break; // exit
        }

        MessageManager.INSTANCE.ShowImageMessage(Messages.SUCCESS_PASSWORD_RESET_REQUEST, MessageManager.Sound.UseSuccessSound);
        yield return new WaitForSeconds(5);
        successCallback?.Invoke();
    }
}
