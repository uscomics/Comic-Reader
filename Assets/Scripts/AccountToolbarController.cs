using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountToolbarController : MonoBehaviour {
    public Button LoginButton;
    public Button LogoutButton;
    public Button AccountButton;

    void Start() {
        Debug.Log("AccountToolbarController.Start");
        LoginButton.GetComponent<Button>().onClick.AddListener(DoLogin); 
        LogoutButton.GetComponent<Button>().onClick.AddListener(DoLogout); 
        AccountButton.GetComponent<Button>().onClick.AddListener(DoAccount); 
        Shared.CleanupCart();
    }
    void Update() {
        SetButtons();
    }
    public void SetButtons() {
        if (null == Shared._USERNAME) {
            LoginButton.gameObject.SetActive(true);
            LogoutButton.gameObject.SetActive(false);
            AccountButton.gameObject.SetActive(false);
        } else {
            LoginButton.gameObject.SetActive(false);
            LogoutButton.gameObject.SetActive(true);
            AccountButton.gameObject.SetActive(true);
        }
    }
    public void DoLogin() {
        Debug.Log("Login");
        SceneManager.LoadScene("Login", LoadSceneMode.Single);
    }
    public void DoLogout() {
        Debug.Log("Logout");
        Shared._USERNAME = null;
        Shared._AUTHORIATION = null;
        Shared._PASSWORD = null;
        Shared._FIRSTNAME = null;
        Shared._LASTNAME = null;
        Shared._EMAIL = null;
        Shared._PURCHASED = new Purchased.PurchasedList();
        Shared._FAVORITES = new Favorites.FavoritesList();
        Shared._CART = new Cart.CartList();
        SceneManager.LoadScene("Storefront", LoadSceneMode.Single);
    }
    public void DoAccount() {
    }
}
