using UnityEngine;
using UnityEngine.UI;

namespace Cart {
    public class CartController : MonoBehaviour {
        public RawImage Cover;
        public Text Title;
        public Text PriceText;
        public Text PriceTitleText;
        public Button RemoveFromCartButton;
        public Button ReturnToCartButton;
        public string Id;
        public int Issue;
        public string Price;

        void Start() {
            RemoveFromCartButton.GetComponent<Button>().onClick.AddListener(ToggleInCart);
            ReturnToCartButton.GetComponent<Button>().onClick.AddListener(ToggleInCart);
            SetCartButtons();
            SetTextColor();
        }
        public void ToggleInCart() {
            Cart cart = Shared._CART.GetCart(Id, Issue);
            cart.MarkedForRemoval = !cart.MarkedForRemoval;
            SetCartButtons();
            SetTextColor();
        }
        public void SetCartButtons() {
            Cart cart = Shared._CART.GetCart(Id, Issue);
            RemoveFromCartButton.gameObject.SetActive(!cart.MarkedForRemoval);
            ReturnToCartButton.gameObject.SetActive(cart.MarkedForRemoval);
        }
        public void SetTextColor() {
            Cart cart = Shared._CART.GetCart(Id, Issue);
            Color c = (cart.MarkedForRemoval)? UnityEngine.Color.gray : UnityEngine.Color.white;
            Title.color = PriceText.color = PriceTitleText.color = c;
        }
    }
}