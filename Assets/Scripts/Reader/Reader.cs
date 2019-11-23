using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Reader {
	public class Reader : MonoBehaviour {
		public GameObject ImageCanvas;
		public GameObject RawImage;
		public GameObject ButtonPanel;
		public Button FirstButton;
		public Button PrevButton;
		public Button HomeButton;
		public Button NextButton;
		public Button LastButton;
		private int _currentPage;
		private Pages _pages;
		private static string _URL_BASE = Shared._URL_BASE + "comics/";
		private IEnumerator _coroutine;
		private float _timeOfLastMouseMovement;
		private static float _DISPLAY_CONTROL_DURATION = 5.0f;

		void Start() {
			GetManifest(Shared._CURRENT_BOOK_ID, Shared._CURRENT_BOOK_ISSUE);
			LoadPage();
			FirstButton.GetComponent<Button>().onClick.AddListener(() => { FirstClick(); });
			PrevButton.GetComponent<Button>().onClick.AddListener(() => { PrevClick(); });
			HomeButton.GetComponent<Button>().onClick.AddListener(() => { HomeClick(); });
			NextButton.GetComponent<Button>().onClick.AddListener(() => { NextClick(); });
			LastButton.GetComponent<Button>().onClick.AddListener(() => { LastClick(); });
			_timeOfLastMouseMovement = Time.fixedTime;
		}

		void Update() {
			if (0 == _currentPage) {
				FirstButton.interactable = false;
				PrevButton.interactable = false;
			} else {
				FirstButton.interactable = true;
				PrevButton.interactable = true;
			}

			if (_pages.pages.Length - 1 == _currentPage) {
				NextButton.interactable = false;
				LastButton.interactable = false;
			} else {
				NextButton.interactable = true;
				LastButton.interactable = true;
			}

			if ((0 != Input.GetAxis("Mouse X")) || (0 != Input.GetAxis("Mouse Y"))) {
				_timeOfLastMouseMovement = Time.fixedTime;
			}

			if (_DISPLAY_CONTROL_DURATION < Time.fixedTime - _timeOfLastMouseMovement) HideControls();
			else ShowControls();

		}

		void OnGUI() {
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				if (Event.current.shift) LastClick();
				else NextClick();
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (Event.current.shift) FirstClick();
				else PrevClick();
			}
		}

		public float GetImageWidth() {
			return RawImage.GetComponent<RawImage>().texture.width;
		}

		public float GetImageHeight() {
			return RawImage.GetComponent<RawImage>().texture.height;
		}

		public float GetCanvasWidth() {
			RectTransform objectRectTransform = ImageCanvas.GetComponent<RectTransform>();
			return objectRectTransform.rect.width;
		}

		public float GetCanvasHeight() {
			RectTransform objectRectTransform = ImageCanvas.GetComponent<RectTransform>();
			return objectRectTransform.rect.height;
		}

		public void GetManifest(string id, int issue) {
			try {
				HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(_URL_BASE + "{0}/{1}", id, issue));
				request.Headers["Authorization"] = Shared._AUTHORIATION;
				
				HttpWebResponse response = (HttpWebResponse) request.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream());
				string jsonResponse = reader.ReadToEnd();
				_pages = JsonUtility.FromJson<Pages>(jsonResponse);
				_currentPage = 0;
			} catch (Exception e) {
				MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
			}
		}

		public void LoadPage() {
			if (null == _pages) return;
			string url = _URL_BASE;
			url += _pages.id + "/";
			url += _pages.issue + "/";
			url += _pages.pages[_currentPage];
			_coroutine = SetImageFromURL(url);
			StartCoroutine(_coroutine);
		}

		public IEnumerator SetImageFromURL(string url) {
			WWW www = new WWW(url);
			while (!www.isDone)
				yield return null;
			Destroy(RawImage.GetComponent<RawImage>().texture);
			RawImage.GetComponent<RawImage>().texture = www.texture;
			ScaleImage();
		}

		public void SetImageFromResource(string path) {
			RawImage.GetComponent<RawImage>().texture = Resources.Load(path) as Texture2D;
			ScaleImage();
		}

		void ScaleImage() {
			float imageRatio = GetImageWidth() / GetImageHeight();
			RawImage.GetComponent<AspectRatioFitter>().aspectRatio = imageRatio;
		}

		void FirstClick() {
			if (!FirstButton.interactable) return;
			if (0 == _currentPage) return;
			_currentPage = 0;
			LoadPage();
			_timeOfLastMouseMovement = Time.fixedTime;
		}

		void PrevClick() {
			if (!PrevButton.interactable) return;
			if (0 == _currentPage) return;
			_currentPage--;
			LoadPage();
			_timeOfLastMouseMovement = Time.fixedTime;
		}

		void HomeClick() {
			Shared._CURRENT_BOOK_ID = null;
			Shared._CURRENT_BOOK_ISSUE = 0;
			SceneManager.LoadScene("Purchased", LoadSceneMode.Single);
		}

		void NextClick() {
			if (!NextButton.interactable) return;
			if (_pages.pages.Length - 1 == _currentPage) return;
			_currentPage++;
			LoadPage();
			_timeOfLastMouseMovement = Time.fixedTime;
		}

		void LastClick() {
			if (!LastButton.interactable) return;
			if (_pages.pages.Length - 1 == _currentPage) return;
			_currentPage = _pages.pages.Length - 1;
			LoadPage();
			_timeOfLastMouseMovement = Time.fixedTime;
		}

		void HideControls() {
			ButtonPanel.gameObject.SetActive(false);
			FirstButton.gameObject.SetActive(false);
			PrevButton.gameObject.SetActive(false);
			HomeButton.gameObject.SetActive(false);
			NextButton.gameObject.SetActive(false);
			LastButton.gameObject.SetActive(false);
		}

		void ShowControls() {
			ButtonPanel.gameObject.SetActive(true);
			FirstButton.gameObject.SetActive(true);
			PrevButton.gameObject.SetActive(true);
			HomeButton.gameObject.SetActive(true);
			NextButton.gameObject.SetActive(true);
			LastButton.gameObject.SetActive(true);
		}
	}
	
	[Serializable]
	public class Pages {
		public string id;
		public string title;
		public int issue;
		public string[] pages;
		public string cover;
		public string[] preview;
	}

}
