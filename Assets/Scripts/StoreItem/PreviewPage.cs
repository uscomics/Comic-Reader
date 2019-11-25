using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StoreItem {
    public class PreviewPage : MonoBehaviour, IPointerClickHandler
    {
        public RawImage MainPreviewImge;
        public RawImage PreviewImge;
        
        public void OnPointerClick(PointerEventData eventData) // 3
        {
            MainPreviewImge.texture = PreviewImge.texture;
        }
    }
}
