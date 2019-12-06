using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeToolbarController : MonoBehaviour {
    public Button HomeButton;

    void Start() {
        HomeButton.GetComponent<Button>().onClick.AddListener(DoHome); 
        Shared.CleanupCart();
    }
    public void DoHome() {
        SceneManager.LoadScene("StoreFront", LoadSceneMode.Single);
    }
}
