	using System;
using System.Collections;
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
		public Slider Progress;
		private int _currentPage;
		private Book _book;
		private static string _URL_BASE = Shared._URL_BASE + "comics/";
		private IEnumerator _coroutine;
		private float _timeOfLastMouseMovement;
		private static float _DISPLAY_CONTROL_DURATION = 5.0f;

		void Start() {
			Shared.CleanupCart();
			GetManifest(Shared._CURRENT_BOOK_ID, Shared._CURRENT_BOOK_ISSUE);
			LoadPage();
			FirstButton.GetComponent<Button>().onClick.AddListener(() => { FirstClick(); });
			PrevButton.GetComponent<Button>().onClick.AddListener(() => { PrevClick(); });
			HomeButton.GetComponent<Button>().onClick.AddListener(() => { HomeClick(); });
			NextButton.GetComponent<Button>().onClick.AddListener(() => { NextClick(); });
			LastButton.GetComponent<Button>().onClick.AddListener(() => { LastClick(); });
			_timeOfLastMouseMovement = Time.fixedTime;
			Progress.value = 0;
		}

		void Update() {
			if (0 == _currentPage) {
				FirstButton.interactable = false;
				PrevButton.interactable = false;
			} else {
				FirstButton.interactable = true;
				PrevButton.interactable = true;
			}

			if (_book.pages.Length - 1 == _currentPage) {
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

			if (null ==_book) return;
			int pages = _book.pages.Length;
			float progress = ((float) _currentPage + 1) / ((float) pages);
			Progress.value = progress;
		}

		public void GetManifest(string id, int issue) {
			string url = String.Format(_URL_BASE + "{0}/{1}", id, issue);
			_book = Book.GetFromServer(url);
			_currentPage = 0;
		}

		public void LoadPage() {
			if (null == _book) return;
			string url = String.Format(_URL_BASE + "{0}/{1}/{2}", _book.id, _book.issue, _book.pages[_currentPage]);
			ImageHelper imageHelper = new ImageHelper();
			_coroutine = imageHelper.SetImageFromURL(RawImage.GetComponent<RawImage>(), url);
			StartCoroutine(_coroutine);
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
			if (_book.pages.Length - 1 == _currentPage) return;
			_currentPage++;
			LoadPage();
			_timeOfLastMouseMovement = Time.fixedTime;
		}

		void LastClick() {
			if (!LastButton.interactable) return;
			if (_book.pages.Length - 1 == _currentPage) return;
			_currentPage = _book.pages.Length - 1;
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
}