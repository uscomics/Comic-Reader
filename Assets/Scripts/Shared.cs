using Purchased;
using System.Collections.Generic;
using UnityEngine;

public class Shared
{
    public static string _URL_BASE = "http://localhost:1337/";
    public static string _AUTHORIATION = "";
    public static string _USERNAME = "";
    public static string _PASSWORD = "";
    public static string _FIRSTNAME = "";
    public static string _LASTNAME = "";
    public static string _EMAIL = "";
    public static PurchasedList _PURCHASED = new PurchasedList();
    public static Dictionary<string, Book> _BOOK_INFO = new Dictionary<string, Book>();
    public static string _CURRENT_BOOK_ID = "";
    public static int _CURRENT_BOOK_ISSUE = 0;
    public static Favorites.FavoritesList _FAVORITES = new Favorites.FavoritesList();
    public static IssueList _CART = new IssueList();

    public static EmptyScript GetEmptyScript() {
        GameObject g = GameObject.Find("EmptyGameObject");
        if (null == g) return null;
        EmptyScript s = g.GetComponent<EmptyScript>();
        return s;
    }            

}
