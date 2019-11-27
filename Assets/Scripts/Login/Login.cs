using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
	public InputField UserName;
	public InputField Password;
	public Button LoginButton;
	public Button CreateAccountButton;
	private IEnumerator _coroutine;
	private static string _URL_BASE = Shared._URL_BASE + "comics/";
 
	void Start () {
		Screen.fullScreen = false;
		LoginButton.GetComponent<Button>().onClick.AddListener(() => { SendCredentials(); }); 
		CreateAccountButton.GetComponent<Button>().onClick.AddListener(() => { CreateAccount(); }); 
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if (EventSystem.current.currentSelectedGameObject != null)
				{
					Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
					if (selectable != null)
						selectable.Select();
				}
			}
			else
			{
				if (EventSystem.current.currentSelectedGameObject != null)
				{
					Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
					if (selectable != null)
						selectable.Select();
				}
			}
		}	
	}

	public void SendCredentials() {
		string url = Shared._URL_BASE + "login";	
		_coroutine = UserLogin(url);
		StartCoroutine(_coroutine);
	}

	public IEnumerator UserLogin(string url)
	{
		WWWForm form = new WWWForm();
		form.AddField("username", UserName.text);
		form.AddField("password", Password.text);

		UnityWebRequest www = UnityWebRequest.Post(Shared._URL_BASE + "user/authenticate", form);
		yield return www.SendWebRequest();

		Dictionary<string, string> headers = www.GetResponseHeaders();
		if (www.isNetworkError || www.isHttpError) {
			MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
		}
		else if (!headers.ContainsKey("Authorization")) {
			MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_INVALID_CREDENTIALS);
		}
		else {
			Shared._AUTHORIATION = headers["Authorization"];
			Shared._USERNAME = UserName.text;
			Shared._PASSWORD = Password.text;
			
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(Shared._URL_BASE + "favorites/{0}", Shared._USERNAME));
			request.Headers["Authorization"] = Shared._AUTHORIATION;
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			if (HttpStatusCode.OK == response.StatusCode || HttpStatusCode.NotFound == response.StatusCode) {
				StreamReader reader = new StreamReader(response.GetResponseStream());
				string jsonResponse = reader.ReadToEnd();
				Shared._FAVORITES = JsonUtility.FromJson<IssueList>("{ \"issues\":" + jsonResponse + "}");
								
				MessageManager.INSTANCE.PlaySuccessSound();
				yield return new WaitForSeconds(2);
				SceneManager.LoadScene("Purchased", LoadSceneMode.Single);
			} else {
				MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
			}
		}
	}

	public void CreateAccount() {
		SceneManager.LoadScene("AddAccount", LoadSceneMode.Single);
	}
	
}
