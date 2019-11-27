using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

[Serializable]
public class Book : Issue {
	public string title = "";
	public string[] pages = { };
	public string cover = "";
	public string[] preview = { };
	public string descriptionTitle1 = "";
	public string description1 = "";
	public string descriptionTitle2 = "";
	public string description2 = "";
	public string descriptionTitle3 = "";
	public string description3 = "";
	public string price = "";
	public Issue[] seeAlso = { };
	public static Book GetFromServer(string url) {
		try {
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
			request.Headers["Authorization"] = Shared._AUTHORIATION;
				
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			string jsonResponse = reader.ReadToEnd();
			return JsonUtility.FromJson<Book>(jsonResponse);
		} catch (Exception e) {
			MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
			return null;
		}
	}
	public static Dictionary<string, Book> GetPurchasedBooksFromServer(string urlBase, Purchased.PurchasedList purchased) {
		Dictionary<string, Book> books = new Dictionary<string, Book>();
		foreach (Purchased.Purchased p in purchased.Purchased) {
			try {
				string bookKey = p.MakeKey();
				HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(urlBase + "{0}/{1}", p.id, p.issue));
				request.Headers["Authorization"] = Shared._AUTHORIATION;

				HttpWebResponse response = (HttpWebResponse) request.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream());
				string jsonResponse = reader.ReadToEnd();
				Book book = JsonUtility.FromJson<Book>(jsonResponse);
				books.Add(bookKey, book);
			} catch (Exception e) {
				MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
			}
		}
		return books;
	}
}
	