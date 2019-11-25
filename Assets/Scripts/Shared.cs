using Purchased;
using Reader;
using System.Collections.Generic;


public class Shared
{
    public static string _URL_BASE = "http://localhost:1337/";
    public static string _AUTHORIATION = null;
    public static string _USERNAME = null;
    public static string _PASSWORD = null;
    public static string _FIRSTNAME = null;
    public static string _LASTNAME = null;
    public static string _EMAIL = null;
    public static OwnedEntryArray _OWNED = null;
    public static Dictionary<string, Pages> _BOOK_INFO = new Dictionary<string, Pages>();
    public static string _CURRENT_BOOK_ID = null;
    public static int _CURRENT_BOOK_ISSUE = 0;
    public static Issues _FAVORITES = new Issues() { issues = new List<Issue>() };
    public static Issues _CART = new Issues() { issues = new List<Issue>() };
}
