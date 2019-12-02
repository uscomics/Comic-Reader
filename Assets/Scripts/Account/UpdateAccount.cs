using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Account {
    public class UpdateAccount : MonoBehaviour {
        public InputField UserName;
        public InputField Password;
        public InputField ReenterPassword;
        public InputField Email;
        public InputField FirstName;
        public InputField LastName;
        public Button UpdateAccountButton;
        private IEnumerator _coroutine;

        void Start() {
            UpdateAccountButton.GetComponent<Button>().onClick.AddListener(() => { SendAccount(Shared._URL_BASE + "account/update"); }); 
            Shared.CleanupCart();
            UserName.text = Shared._USERNAME;
            Password.text = Shared._PASSWORD;
            Email.text = Shared._EMAIL;
            FirstName.text = Shared._FIRSTNAME;
            LastName.text = Shared._LASTNAME;
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
            StartCoroutine(account.PostToServer(url, Account._DEFAULT_DESTINATION, () => { Debug.Log( "Error updating account" ); }, () => { SceneManager.LoadScene("StoreFront", LoadSceneMode.Single); }));
        }
    }
}
