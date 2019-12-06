using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountToolbarController : MonoBehaviour {
    public Button LoginButton;
    public Button SignupButton;
    public Button LogoutButton;
    public Button AccountButton;
    public Button BooksButton;

    void Start() {
        LoginButton.GetComponent<Button>().onClick.AddListener(DoLogin); 
        SignupButton.GetComponent<Button>().onClick.AddListener(DoSignup); 
        LogoutButton.GetComponent<Button>().onClick.AddListener(DoLogout); 
        AccountButton.GetComponent<Button>().onClick.AddListener(DoAccount); 
        BooksButton.GetComponent<Button>().onClick.AddListener(DoBooks); 
        Shared.CleanupCart();
    }
    void Update() {
        SetButtons();
    }
    public void SetButtons() {
        if (String.IsNullOrEmpty(Shared._USERNAME)) {
            LoginButton.gameObject.SetActive(true);
            SignupButton.gameObject.SetActive(true);
            LogoutButton.gameObject.SetActive(false);
            AccountButton.gameObject.SetActive(false);
            BooksButton.gameObject.SetActive(false);
        } else {
            LoginButton.gameObject.SetActive(false);
            SignupButton.gameObject.SetActive(false);
            LogoutButton.gameObject.SetActive(true);
            AccountButton.gameObject.SetActive(true);
            BooksButton.gameObject.SetActive(true);
        }
    }
    public void DoLogin() {
        SceneManager.LoadScene("Login", LoadSceneMode.Single);
    }
    public void DoLogout() {
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
        SceneManager.LoadScene("UpdateAccount", LoadSceneMode.Single);
    }
    public void DoBooks() {
        SceneManager.LoadScene("Purchased", LoadSceneMode.Single);
    }
    public void DoSignup() {
        SceneManager.LoadScene("AddAccount", LoadSceneMode.Single);
    }
}
