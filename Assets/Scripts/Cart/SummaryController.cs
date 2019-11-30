using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cart {
    public class SummaryController : MonoBehaviour {
        public Text Subtotal;
        public Text Taxes;
        public Text Total;
        public Button PurchaseButton;
        
        void Start() {
            PurchaseButton.GetComponent<Button>().onClick.AddListener(Purchase); 
        }
        private void Update() {
            UpdatePrice();
            UpdateTaxes();
            UpdateTotal();
        }
        public void UpdatePrice() {
            Subtotal.text = "$" + Shared._CART.GetTotalPrice();
        }
        public void UpdateTaxes() {
        }
        public void UpdateTotal() {
            Total.text = Subtotal.text;
        }
        public void Purchase() {
            StartCoroutine(Shared._CART.Purchase(Shared._URL_BASE + "user/checkout/data", null, LoadPurchaseScreen));
        }
        public void LoadPurchaseScreen() {
            SceneManager.LoadScene("Purchased", LoadSceneMode.Single);
        }
    }
}