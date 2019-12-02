using System;
using System.Collections;
using System.Collections.Generic;
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
 
	void Start () {
		Screen.fullScreen = false;
		LoginButton.GetComponent<Button>().onClick.AddListener(() => { SendCredentials(); }); 
		CreateAccountButton.GetComponent<Button>().onClick.AddListener(() => { CreateAccount(); }); 
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				if (EventSystem.current.currentSelectedGameObject != null) {
					Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
					if (selectable != null)
						selectable.Select();
				}
			} else {
				if (EventSystem.current.currentSelectedGameObject != null) {
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
			Debug.Log(www.downloadHandler.text);
			Shared._AUTHORIATION = headers["Authorization"];
			Shared._USERNAME = UserName.text;
			Shared._PASSWORD = Password.text;

			User user = User.GetFromServer(Shared._URL_BASE + "user/" + Shared._USERNAME + "/info");

			if (null == user) {
				MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
				yield break; // exit
			} 
			Shared._EMAIL = user.email;
			Shared._FIRSTNAME = user.firstName;
			Shared._LASTNAME = user.lastName;
			Shared._FAVORITES = Favorites.FavoritesList.GetFromServer(String.Format(Shared._URL_BASE + "favorites/{0}", Shared._USERNAME));
			if (null == Shared._FAVORITES) Shared._FAVORITES = new Favorites.FavoritesList();
			Shared._CART = Cart.CartList.GetFromServer(String.Format(Shared._URL_BASE + "cart/{0}", Shared._USERNAME));
			if (null != Shared._CART) {
				MessageManager.INSTANCE.PlaySuccessSound();
				yield return new WaitForSeconds(2);
				SceneManager.LoadScene("Purchased", LoadSceneMode.Single);
			} else {
				Shared._CART = new Cart.CartList();
			}
		}
	}
	public void CreateAccount() {
		SceneManager.LoadScene("AddAccount", LoadSceneMode.Single);
	}
	
}
