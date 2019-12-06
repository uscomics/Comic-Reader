using System;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace StoreFront {
	[Serializable]
	public class Slide : Issue {
		public string image;
		public string title;
		public void GetFromServer(string baseURL, string slideshowName, RawImage rawImage) {
			string url = baseURL + slideshowName + "/" + image;
			ImageHelper imageHelper = new ImageHelper();
			Shared.GetEmptyScript().StartCoroutine(imageHelper.SetImageFromURL(rawImage, url));
		}
	}
}

