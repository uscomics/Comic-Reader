using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Account {
    public class AddAccount : MonoBehaviour {
        public InputField UserName;
        public InputField Password;
        public InputField ReenterPassword;
        public InputField Email;
        public InputField FirstName;
        public InputField LastName;
        public Button CreateAccountButton;
        public Button LoginButton;
        private IEnumerator _coroutine;

        void Start() {
            CreateAccountButton.GetComponent<Button>().onClick.AddListener(() => { SendAccount(Shared._URL_BASE + "account/add"); }); 
            LoginButton.GetComponent<Button>().onClick.AddListener(() => { GoToLogin(); }); 
            Shared.CleanupCart();
        }

        void Update () {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    if (EventSystem.current.currentSelectedGameObject != null) {
                        Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                        if (selectable != null) selectable.Select();
                    }
                } else {
                    if (EventSystem.current.currentSelectedGameObject != null) {
                        Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                        if (selectable != null) selectable.Select();
                    }
                }
            }	
        }
        
        public void SendAccount(string url) {
            if (Password.text != ReenterPassword.text) {
                MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_PASSWORDS_DO_NOT_MATCH);
                return;
            }
            Account account = new Account(UserName.text, Password.text, Email.text, FirstName.text, LastName.text, "user");
            StartCoroutine(account.PostToServer(url, Account._DEFAULT_DESTINATION, () => { Debug.Log( "Error adding account" ); }, () => { SceneManager.LoadScene("Login", LoadSceneMode.Single); }));
        }
 
        public void GoToLogin() {
            SceneManager.LoadScene("Login", LoadSceneMode.Single);
        }
    }
}
