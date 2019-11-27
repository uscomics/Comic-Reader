using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StoreItem {
    public class PreviewPage : MonoBehaviour, IPointerClickHandler
    {
        public RawImage MainPreviewImge;
        public RawImage PreviewImge;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            MainPreviewImge.texture = PreviewImge.texture;
        }
    }
}
