using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cart {
    public class SummaryController : MonoBehaviour {
        public Text Total;
        public Button CheckoutButton;
        
        void Start() {
            CheckoutButton.GetComponent<Button>().onClick.AddListener(Checkout); 
        }
        private void Update() {
            UpdatePrice();
        }

        public void UpdatePrice() {
            Total.text = "$" + Shared._CART.GetTotalPrice();
        }
        public void Checkout() {
            
        }
    }
}