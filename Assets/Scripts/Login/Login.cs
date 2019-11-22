using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


public class Login : MonoBehaviour {
	public InputField UserName;
	public InputField Password;
	public Button LoginButton;
	public Button CreateAccountButton;
	private IEnumerator _coroutine;
 
	void Start () {
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
		if (www.isNetworkError || www.isHttpError || !headers.ContainsKey("Authorization")) {
			Debug.Log("Login failed");
		}
		else {
			Shared._AUTHORIATION = headers["Authorization"];
			Shared._USERNAME = UserName.text;
			Shared._PASSWORD = Password.text;
			Debug.Log("Login complete! " + www.responseCode + " " + www.downloadedBytes);
			Dictionary<string, string>.KeyCollection keys = headers.Keys;
			foreach (string key in keys) {
				Debug.Log(key + ": " + headers[key]);
			}
		}
	}

	public void CreateAccount() {
	}
	
}
