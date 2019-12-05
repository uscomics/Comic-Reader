using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StoreFront {
    public class SlideshowController : MonoBehaviour, IPointerClickHandler {
        public RawImage MainSlideRawImage;
        public Text MessageText;
        private int _currentSlide = 0;
        private Slideshow _slideshow;
        private float _lastTransitionTime;
        
        void Start() {
            _slideshow = Slideshow.GetFromServer(Shared._URL_BASE + "slideshow/", "store-front");
            LoadSlide();
        }
    
        void FixedUpdate() {
            if (_lastTransitionTime + (_slideshow.defaultDuration / 1000) < Time.fixedTime ) {
                _currentSlide = _slideshow.GetNextSlideNumber(_currentSlide);
                LoadSlide();
            }
        }
        void LoadSlide() {
            _slideshow.slides[_currentSlide].GetFromServer(Shared._URL_BASE + "slideshow/", "store-front", MainSlideRawImage);
            MessageText.text = _slideshow.slides[_currentSlide].title;
            _lastTransitionTime = Time.fixedTime;
        }
        public void OnPointerClick(PointerEventData eventData) {
            Shared._CURRENT_BOOK_ID = _slideshow.slides[_currentSlide].id;
            Shared._CURRENT_BOOK_ISSUE =  _slideshow.slides[_currentSlide].issue;
            SceneManager.LoadScene("StoreItem", LoadSceneMode.Single);
        }
    }
}
