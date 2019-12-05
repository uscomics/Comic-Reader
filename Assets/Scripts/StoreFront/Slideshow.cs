using System;
using System.IO;
using System.Net;
using UnityEngine;

namespace StoreFront {
    public class Slideshow {
        public string id;
        public float defaultDuration;
        public Slide[] slides = { };

        public int GetNextSlideNumber(int currentSlideNumber) {
            if (null == slides) return -1;
            if (0 == slides.Length) return 0;
            currentSlideNumber++;
            if (slides.Length <= currentSlideNumber) return 0;
            return currentSlideNumber;
        }
        public int GetPrevSlideNumber(int currentSlideNumber) {
            if (null == slides) return -1;
            if (0 == slides.Length) return 0;
            currentSlideNumber--;
            if (-1 == currentSlideNumber) return slides.Length - 1;
            return currentSlideNumber;
        }
        public static Slideshow GetFromServer(string baseURL, string slideshowName) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(baseURL + slideshowName);
                request.Headers["Authorization"] = Shared._AUTHORIATION;
				
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string jsonResponse = reader.ReadToEnd();
                return JsonUtility.FromJson<Slideshow>(jsonResponse);
            } catch (Exception e) {
                MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
                return null;
            }
        }
    }    
}
