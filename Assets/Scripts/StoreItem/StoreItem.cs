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
        private Reader.Pages _pages;
        private Reader.Pages _seeAlso1Pages;
        private Reader.Pages _seeAlso2Pages;
        private Reader.Pages _seeAlso3Pages;
        private Reader.Pages _seeAlso4Pages;

        // Start is called before the first frame update
        void Start() {
            FavoriteButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(ToggleFavorite()); }); 
            AddToCartButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(ToggleAddToCart()); }); 
            LoadPage();
        }

        public void LoadPage() {
            if (null == Shared._USERNAME) Shared._USERNAME = "dave";
            if (null == Shared._CURRENT_BOOK_ID) Shared._CURRENT_BOOK_ID = "gold_venus";
            if (0 == Shared._CURRENT_BOOK_ISSUE) Shared._CURRENT_BOOK_ISSUE = 1;
            _pages = GetManifest(Shared._CURRENT_BOOK_ID, Shared._CURRENT_BOOK_ISSUE);
            LoadImages();
            LoadText();
            SetButtons();
        }

        public Reader.Pages GetManifest(string id, int issue) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(_URL_BASE + "{0}/{1}", id, issue));
                request.Headers["Authorization"] = Shared._AUTHORIATION;

                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string jsonResponse = reader.ReadToEnd();
                return JsonUtility.FromJson<Reader.Pages>(jsonResponse);
            } catch (Exception e) {
                MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
                return null;
            }
        }

        public void LoadImages() {
            if (null == _pages) return;
            string url = _URL_BASE;
            url += _pages.id + "/";
            url += _pages.issue + "/";
            StartCoroutine(LoadImagesFromURL(url, MainImage, CoverImage, Preview1Image, Preview2Image, Preview3Image, SeeAlso1Image, SeeAlso2Image, SeeAlso3Image, SeeAlso4Image));
         }

        public IEnumerator LoadImagesFromURL(string url, RawImage mainImage, RawImage coverImage, RawImage preview1Image, RawImage preview2Image, RawImage preview13Image, RawImage seeAlso1Image, RawImage seeAlso2Image, RawImage seeAlso3Image, RawImage seeAlso4Image) {
            string finalURL = url + _pages.cover;
            WWW www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            Destroy(mainImage.texture);
            Destroy(coverImage.texture);
            mainImage.texture = www.texture;
            coverImage.texture = www.texture;
            
            finalURL = url + _pages.preview[0];
            www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            Destroy(preview1Image.texture);
            preview1Image.texture = www.texture;
            
            finalURL = url + _pages.preview[1];
            www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            Destroy(preview2Image.texture);
            preview2Image.texture = www.texture;
            
            finalURL = url + _pages.preview[2];
            www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            preview13Image.texture = www.texture;

            _seeAlso1Pages = GetManifest(_pages.seeAlso[0].id, _pages.seeAlso[0].issue);
            finalURL = String.Format(_URL_BASE + "{0}/{1}/{2}", _pages.seeAlso[0].id, _pages.seeAlso[0].issue, _seeAlso1Pages.cover);
            www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            Destroy(seeAlso1Image.texture);
            seeAlso1Image.texture = www.texture;
            seeAlso1Image.GetComponent<SeeAlso>().Id = _pages.seeAlso[0].id;
            seeAlso1Image.GetComponent<SeeAlso>().Issue = _pages.seeAlso[0].issue;

            _seeAlso2Pages = GetManifest(_pages.seeAlso[1].id, _pages.seeAlso[1].issue);
            finalURL = String.Format(_URL_BASE + "{0}/{1}/{2}", _pages.seeAlso[1].id, _pages.seeAlso[1].issue, _seeAlso2Pages.cover);
            www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            Destroy(seeAlso2Image.texture);
            seeAlso2Image.texture = www.texture;
            seeAlso2Image.GetComponent<SeeAlso>().Id = _pages.seeAlso[1].id;
            seeAlso2Image.GetComponent<SeeAlso>().Issue = _pages.seeAlso[1].issue;

            _seeAlso3Pages = GetManifest(_pages.seeAlso[2].id, _pages.seeAlso[2].issue);
            finalURL = String.Format(_URL_BASE + "{0}/{1}/{2}", _pages.seeAlso[2].id, _pages.seeAlso[2].issue, _seeAlso3Pages.cover);
            www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            Destroy(seeAlso3Image.texture);
            seeAlso3Image.texture = www.texture;
            seeAlso3Image.GetComponent<SeeAlso>().Id = _pages.seeAlso[2].id;
            seeAlso3Image.GetComponent<SeeAlso>().Issue = _pages.seeAlso[2].issue;
         
            _seeAlso4Pages = GetManifest(_pages.seeAlso[3].id, _pages.seeAlso[3].issue);
            finalURL = String.Format(_URL_BASE + "{0}/{1}/{2}", _pages.seeAlso[3].id, _pages.seeAlso[3].issue, _seeAlso4Pages.cover);
            www = new WWW(finalURL);
            while (!www.isDone) { yield return null; }
            Destroy(seeAlso4Image.texture);
            seeAlso4Image.texture = www.texture;
            seeAlso4Image.GetComponent<SeeAlso>().Id = _pages.seeAlso[3].id;
            seeAlso4Image.GetComponent<SeeAlso>().Issue = _pages.seeAlso[3].issue;
        }

        public void LoadText() {
            if (null == _pages) return;
            BookTitle.text = _pages.title + " #" + _pages.issue;
            Description1Title.text = _pages.descriptionTitle1;
            Description1.text = _pages.description1;
            Description2Title.text = (( 0 < _pages.descriptionTitle2.Length)? _pages.descriptionTitle2 : " " );
            Description2.text = (( 0 < _pages.description2.Length)? _pages.description2 : " " );
            Description3Title.text = (( 0 < _pages.descriptionTitle3.Length )? _pages.descriptionTitle3 : " " );
            Description3.text = (( 0 < _pages.description3.Length )? _pages.description3 : " " );    
            Price.text = "$" + _pages.price;
        }

        public void SetButtons() {
            ColorBlock colors = FavoriteButton.colors;
            colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.white;
            FavoriteButton.colors = colors;

            if (null != Shared._FAVORITES && Shared._FAVORITES.HasIssue( _pages.id, _pages.issue )) {
                colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.red;
                FavoriteButton.colors = colors;
            }
            
            colors = FavoriteToolbarButton.colors;
            colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.white;
            FavoriteToolbarButton.colors = colors;
            if (null != Shared._FAVORITES && 0 < Shared._FAVORITES.issues.Count) {
                colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.red;
                FavoriteToolbarButton.colors = colors;
            }
            
            colors = AddToCartButton.colors;
            colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.white;
            AddToCartButton.colors = colors;

            if (null != Shared._CART && Shared._CART.HasIssue( _pages.id, _pages.issue )) {
                colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.red;
                AddToCartButton.colors = colors;
            }
            
            colors = AddToCartToolbarButton.colors;
            colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.white;
            AddToCartToolbarButton.colors = colors;
            if (null != Shared._CART && 0 < Shared._CART.issues.Count) {
                colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = Color.red;
                AddToCartToolbarButton.colors = colors;
            }
        }

        public IEnumerator ToggleFavorite() {
            WWWForm form = new WWWForm();
            UnityWebRequest www;
            form.AddField("username", Shared._USERNAME);
            form.AddField("id", _pages.id);
            form.AddField("issue", _pages.issue);
            if (Shared._FAVORITES.HasIssue(_pages.id, _pages.issue)) {
                Shared._FAVORITES.RemoveIssue(_pages.id, _pages.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/favorites/delete/data", form);
            } else {
                Shared._FAVORITES.AddIssue(_pages.id, _pages.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/favorites/add/data", form);
            }
            SetButtons();
            yield return www.SendWebRequest();
        }

        public IEnumerator ToggleAddToCart() {
            WWWForm form = new WWWForm();
            UnityWebRequest www;
            form.AddField("username", Shared._USERNAME);
            form.AddField("id", _pages.id);
            form.AddField("issue", _pages.issue);
            if (Shared._CART.HasIssue(_pages.id, _pages.issue)) {
                Shared._CART.RemoveIssue(_pages.id, _pages.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/cart/delete/data", form);
            } else {
                Shared._CART.AddIssue(_pages.id, _pages.issue);
                www = UnityWebRequest.Post(Shared._URL_BASE + "user/cart/add/data", form);
            }
            SetButtons();
            yield return www.SendWebRequest();
        }
    }
}
