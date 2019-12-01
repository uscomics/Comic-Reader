using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CartAndFavToolbarController : MonoBehaviour {
    public Button CartButton;
    public Button FavoritesButton;

    void Start() {
        CartButton.GetComponent<Button>().onClick.AddListener(DoCart); 
        FavoritesButton.GetComponent<Button>().onClick.AddListener(DoFavorites); 
        Shared.CleanupCart();
    }
    void Update() {
        SetButtons();
    }
    public void SetButtons() {
        SetButton(FavoritesButton, (null != Shared._FAVORITES && 0 < Shared._FAVORITES.Favorites.Count));
        SetButton(CartButton, (null != Shared._CART && 0 < Shared._CART.Cart.Count));
    }
    public void SetButton(Button button, bool check) {
        ColorBlock colors = button.colors;
        Color color = ((check)? Color.red : Color.white );
        colors.normalColor = colors.highlightedColor = colors.pressedColor = colors.selectedColor = color;
        button.colors = colors;
    }
    public void DoCart() {
        if (0 == Shared._CART.Cart.Count) MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NO_CART, MessageManager.Sound.NoSound);
        else SceneManager.LoadScene("Cart", LoadSceneMode.Single);
    }
    public void DoFavorites() {
        if (0 == Shared._FAVORITES.Favorites.Count) MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NO_FAVORITES, MessageManager.Sound.NoSound);
        else SceneManager.LoadScene("Favorites", LoadSceneMode.Single);
    }
}
