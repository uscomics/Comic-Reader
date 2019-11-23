using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AddAccount {
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

        // Start is called before the first frame update
        void Start() {
            CreateAccountButton.GetComponent<Button>().onClick.AddListener(() => { SendAccount(); }); 
            LoginButton.GetComponent<Button>().onClick.AddListener(() => { Login(); }); 
        }

        // Update is called once per frame
        void Update () {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                        if (selectable != null)
                            selectable.Select();
                    }
                }
                else
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                        if (selectable != null)
                            selectable.Select();
                    }
                }
            }	
        }
 
        public void SendAccount() {
            string url = Shared._URL_BASE + "account/add";	
            _coroutine = UserLogin(url);
            StartCoroutine(_coroutine);
        }

        public int ValidateInput() {
            if (0 == UserName.text.Length) return Messages.ERROR_USERNAME_REQUIRED;
            if (5 > UserName.text.Length) return Messages.ERROR_INVALID_USERNAME_LENGTH;
            if( !Regex.IsMatch(UserName.text, "[a-zA-Z0-9-_]") ) return Messages.ERROR_INVALID_USERNAME;
            if (0 == Password.text.Length) return Messages.ERROR_PASSWORD_REQUIRED;
            if (5 > Password.text.Length) return Messages.ERROR_PASSWORD_LENGTH;
            if (Password.text != ReenterPassword.text) return Messages.ERROR_PASSWORDS_DO_NOT_MATCH;
            if (0 < Email.text.Length && 4 > Email.text.Length) return Messages.ERROR_INVALID_EMAIL;
            if (0 < Email.text.Length && 0 == Email.text.IndexOf('@')) return Messages.ERROR_INVALID_EMAIL;
            return 0;
        }
        
        public IEnumerator UserLogin(string url) {
            int errorCode = ValidateInput();
            if (0 != errorCode) {
                Debug.Log("Error :" + errorCode);
                MessageManager.INSTANCE.ShowImageMessage(errorCode);
            } else {
                Debug.Log("url :" + url);
                WWWForm form = new WWWForm();
                form.AddField("username", UserName.text);
                form.AddField("password", Password.text);
                form.AddField("email", Email.text);
                form.AddField("firstName", FirstName.text);
                form.AddField("lastName", LastName.text);
                form.AddField("group1", "user");
                form.AddField("destination", "./src/authentication/authentication.json");
                UnityWebRequest www = UnityWebRequest.Post(Shared._URL_BASE + "account/add", form);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError) {
                    MessageManager.INSTANCE.ShowImageMessage(Messages.ERROR_NETWORK);
                } else {
                    MessageManager.INSTANCE.ShowImageMessage(Messages.SUCCESS_ACCOUNT_ADDED, MessageManager.Sound.UseSuccessSound);
                    yield return new WaitForSeconds(5);
                    SceneManager.LoadScene("Login", LoadSceneMode.Single);
                }
            }
        }
 
        public void Login() {
            SceneManager.LoadScene("Login", LoadSceneMode.Single);
        }
    }
}
