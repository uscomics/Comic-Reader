using Purchased;
using Reader;
using System.Collections.Generic;


public class Shared
{
    public static string _URL_BASE = "http://localhost:1337/";
    public static string _AUTHORIATION = null;
    public static string _USERNAME = "dave";
    public static string _PASSWORD = null;
    public static OwnedEntryArray _OWNED = null;
    public static Dictionary<string, Pages> _BOOK_INFO = new Dictionary<string, Pages>();
}
