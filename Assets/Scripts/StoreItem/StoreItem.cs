using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace StoreItem {
    public class StoreItem : MonoBehaviour {
        public RawImage MainImage;
        public RawImage CoverImage;
        public RawImage Preview1Image;
        public RawImage Preview2Image;
        public RawImage Preview3Image;
        public RawImage SeeAlso1Image;
        public RawImage SeeAlso2Image;
        public RawImage SeeAlso3Image;
        public RawImage SeeAlso4Image;
        public Text BookTitle;
        public Text Description1Title;
        public Text Description1;
        public Text Description2Title;
        public Text Description2;
        public Text Description3Title;
        public Text Description3;
        public Text Price;
        public Button FavoriteButton;
        public Button FavoriteToolbarButton;
        public Button AddToCartButton;
        public Button AddToCartToolbarButton;

        private static string _URL_BASE = Shared._URL_BASE + "comics/";
        private Book _book;
        private Book _seeAlso1Pages;
        private Book _seeAlso2Pages;
        private Book _seeAlso3Pages;
        private Book _seeAlso4Pages;

        // Start is called before the first frame update
        void Start() {
            FavoriteButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(ToggleFavorite()); }); 
            AddToCartButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(ToggleAddToCart()); }); 
            LoadPage();
        }

        public void LoadPage() {
            if (0 == Shared._USERNAME.Length) Shared._USERNAME = "dave";
            if (0 == Shared._CURRENT_BOOK_ID.Length) Shared._CURRENT_BOOK_ID = "gold_venus";
            if (0 == Shared._CURRENT_BOOK_ISSUE) Shared._CURRENT_BOOK_ISSUE = 1;
            _book = GetManifest(Shared._CURRENT_BOOK_ID, Shared._CURRENT_BOOK_ISSUE);
            _seeAlso1Pages = GetManifest(_book.seeAlso[0].id, _book.seeAlso[0].issue);
            _seeAlso2Pages = GetManifest(_book.seeAlso[1].id, _book.seeAlso[1].issue);
            _seeAlso3Pages = GetManifest(_book.seeAlso[2].id, _book.seeAlso[2].issue);
            _seeAlso4Pages = GetManifest(_book.seeAlso[3].id, _book.seeAlso[3].issue);
            LoadImages();
            LoadText();
            SetButtons();
        }

        public Book GetManifest(string id, int issue) {
            return Book.GetFromServer(String.Format(_URL_BASE + "{0}/{1}", id, issue));
        }

        public void LoadImages() {
            if (null == _book) return;
            string url = _URL_BASE + _book.id + "/" + _book.issue + "/";
            ImageHelper imageHelper = new ImageHelper();
            StartCoroutine(imageHelper.SetImageFromURL(MainImage, url + _book.cover));
            StartCoroutine(imageHelper.SetImageFromURL(CoverImage, url + _book.cover));            
            StartCoroutine(imageHelper.SetImageFromURL(Preview1Image, url + _book.preview[0]));            
            StartCoroutine(imageHelper.SetImageFromURL(Preview2Image, url + _book.preview[1]));            
            StartCoroutine(imageHelper.SetImageFromURL(Preview3Image, url + _book.preview[2]));            
 
            StartCoroutine(imageHelper.SetImageFromURL(SeeAlso1Image, String.Format(_URL_BASE + "{0}/{1}/{2}", _book.seeAlso[0].id, _book.seeAlso[0].issue, _seeAlso1Pages.cover)));
            SeeAlso1Image.GetComponent<SeeAlso>().Id = _book.seeAlso[0].id;
            SeeAlso1Image.GetComponent<SeeAlso>().Issue = _book.seeAlso[0].issue;

            StartCoroutine(imageHelper.SetImageFromURL(SeeAlso2Image, String.Format(_URL_BASE + "{0}/{1}/{2}", _book.seeAlso[1].id, _book.seeAlso[1].issue, _seeAlso2Pages.cover)));
            SeeAlso2Image.GetComponent<SeeAlso>().Id = _book.seeAlso[1].id;
            SeeAlso2Image.GetComponent<SeeAlso>().Issue = _book.seeAlso[1].issue;

            StartCoroutine(imageHelper.SetImageFromURL(SeeAlso3Image, String.Format(_URL_BASE + "{0}/{1}/{2}", _book.seeAlso[2].id, _book.seeAlso[2].issue, _seeAlso3Pages.cover)));
            SeeAlso3Image.GetComponent<SeeAlso>().Id = _book.seeAlso[2].id;
            SeeAlso3Image.GetComponent<SeeAlso>().Issue = _book.seeAlso[2].issue;
         
            StartCoroutine(imageHelper.SetImageFromURL(SeeAlso4Image, String.Format(_URL_BASE + "{0}/{1}/{2}", _book.seeAlso[3].id, _book.seeAlso[3].issue, _seeAlso4Pages.cover)));
            SeeAlso4Image.GetComponent<SeeAlso>().Id = _book.seeAlso[3].id;
            SeeAlso4Image.GetComponent<SeeAlso>().Issue = _book.seeAlso[3].issue;
         }

        public void LoadText() {
            if (null == _book) return;
            BookTitle.text = _book.title + " #" + _book.issue;
            Description1Title.text = _book.descriptionTitle1;
            Description1.text = _book.description1;
            Description2Title.text = (( 0 < _book.descriptionTitle2.Length)? _book.descriptionTitle2 : " " );
            Description2.text = (( 0 < _book.description2.Length)? _book.description2 : " " );
            Description3Title.text = (( 0 < _book.descriptionTitle3.Length )? _book.descriptionTitle3 : " " );
            Description3.text = (( 0 < _book.description3.Length )? _book.description3 : " " );    
            Price.text = "$" + _book.price;
        }

        public void SetButtons() {
            SetButton(FavoriteButton, (null != Shared._FAVORITES && Shared._FAVORITES.HasIssue( _book.id, _book.issue )));
            SetButton(FavoriteToolbarButton, (null != Shared._FAVORITES && 0 < Shared._FAVORITES.Issues.Count));
            SetButton(AddToCartButton, (null != Shared._CART && Shared._CART.HasIssue( _book.id, _book.issue )));
            SetButton(AddToCartToolbarButton, (null != Shared._CART && 0 < Shared._CART.Issues.Count));
        }

        public void SetButton(Button button, bool check) {
            ColorBlock colors = button.colors;
            Color color = ((check)? Color.red : Color.white );
            colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = color;
            button.colors = colors;
        }

        public IEnumerator ToggleFavorite() {
            WWWForm form = new WWWForm();
            UnityWebRequest www;
            form.AddField("username", Shared._USERNAME);
            form.AddField("id", _book.id);
            form.AddField("issue", _book.issue);
            if (Shared._FAVORITES.HasIssue(_book.id, _book.issue)) {
                Shared._FAVORITES.RemoveIssue(_book.id, _book.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/favorites/delete/data", form);
            } else {
                Shared._FAVORITES.AddIssue(_book.id, _book.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/favorites/add/data", form);
            }
            SetButtons();
            yield return www.SendWebRequest();
        }

        public IEnumerator ToggleAddToCart() {
            WWWForm form = new WWWForm();
            UnityWebRequest www;
            form.AddField("username", Shared._USERNAME);
            form.AddField("id", _book.id);
            form.AddField("issue", _book.issue);
            if (Shared._CART.HasIssue(_book.id, _book.issue)) {
                Shared._CART.RemoveIssue(_book.id, _book.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/cart/delete/data", form);
            } else {
                Shared._CART.AddIssue(_book.id, _book.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/cart/add/data", form);
            }
            SetButtons();
            yield return www.SendWebRequest();
        }
    }
}
