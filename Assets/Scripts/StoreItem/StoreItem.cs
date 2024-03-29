﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public Button AddToCartButton;

        private static string _URL_BASE = Shared._URL_BASE + "comics/";
        private Book _book;
        private Book _seeAlso1Pages;
        private Book _seeAlso2Pages;
        private Book _seeAlso3Pages;
        private Book _seeAlso4Pages;

        void Start() {
            FavoriteButton.GetComponent<Button>().onClick.AddListener(ToggleFavorite); 
            AddToCartButton.GetComponent<Button>().onClick.AddListener(ToggleAddToCart); 
            Shared.CleanupCart();
            LoadPage();
        }
        public void LoadPage() {
            _book = GetManifest(Shared._CURRENT_BOOK_ID, Shared._CURRENT_BOOK_ISSUE);
            _seeAlso1Pages = GetManifest(_book.seeAlso[0].id, _book.seeAlso[0].issue);
            _seeAlso2Pages = GetManifest(_book.seeAlso[1].id, _book.seeAlso[1].issue);
            _seeAlso3Pages = GetManifest(_book.seeAlso[2].id, _book.seeAlso[2].issue);
            _seeAlso4Pages = GetManifest(_book.seeAlso[3].id, _book.seeAlso[3].issue);
            LoadImages();
            LoadText();
            SetButtons();
            CheckPurchase();
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
            LoadSeeAlsoImage(imageHelper, SeeAlso1Image, _book.seeAlso[0].id, _book.seeAlso[0].issue, _seeAlso1Pages.cover);
            LoadSeeAlsoImage(imageHelper, SeeAlso2Image, _book.seeAlso[1].id, _book.seeAlso[1].issue, _seeAlso2Pages.cover);
            LoadSeeAlsoImage(imageHelper, SeeAlso3Image, _book.seeAlso[2].id, _book.seeAlso[2].issue, _seeAlso3Pages.cover);
            LoadSeeAlsoImage(imageHelper, SeeAlso4Image, _book.seeAlso[3].id, _book.seeAlso[3].issue, _seeAlso4Pages.cover);
         }
        public void LoadSeeAlsoImage(ImageHelper imageHelper, RawImage image, string id, int issue, string cover) {
            StartCoroutine(imageHelper.SetImageFromURL(image, String.Format(_URL_BASE + "{0}/{1}/{2}", id, issue, cover)));
            image.GetComponent<SeeAlso>().Id = id;
            image.GetComponent<SeeAlso>().Issue = issue;
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
            SetButton(FavoriteButton, (null != Shared._FAVORITES && Shared._FAVORITES.HasFavorite( _book.id, _book.issue )));
            SetButton(AddToCartButton, (null != Shared._CART && Shared._CART.HasCart( _book.id, _book.issue )));
        }
        public void SetButton(Button button, bool check) {
            ColorBlock colors = button.colors;
            Color color = ((check)? Color.red : Color.white );
            colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = color;
            button.colors = colors;
        }
        public void ToggleFavorite() {
            Shared._FAVORITES?.ToggleFavorite(_book.id,  _book.issue, Shared._URL_BASE + "user/favorites/add/data", Shared._URL_BASE + "user/favorites/delete/data", SetButtons, SetButtons);
        }
        public void ToggleAddToCart() {
            Shared._CART?.ToggleCart(_book.id,  _book.issue, Shared._URL_BASE + "user/cart/add/data", Shared._URL_BASE + "user/cart/delete/data", SetButtons, SetButtons);
        }
        public void CheckPurchase() {
            if (Shared._PURCHASED.HasPurchase(Shared._CURRENT_BOOK_ID, Shared._CURRENT_BOOK_ISSUE)) {
                string message = String.Format(Messages.GetMessage(Messages.Language.en_US, Messages.MESSAGE_ALREADY_PURCHASED), Shared._PURCHASED.GetPurchase(Shared._CURRENT_BOOK_ID, Shared._CURRENT_BOOK_ISSUE).transactionDate);
                MessageManager.INSTANCE.ShowImageMessage(message, MessageManager.Sound.NoSound);
            }
        }
    }
}
