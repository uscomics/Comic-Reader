using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messages {
    public enum Language {en_US};
    public static readonly int ERROR_USERNAME_REQUIRED = 1;
    public static readonly int ERROR_INVALID_USERNAME = 2;
    public static readonly int ERROR_INVALID_USERNAME_LENGTH = 3;
    public static readonly int ERROR_PASSWORD_REQUIRED = 4;
    public static readonly int ERROR_PASSWORD_LENGTH = 5;
    public static readonly int ERROR_PASSWORDS_DO_NOT_MATCH = 6;
    public static readonly int ERROR_INVALID_EMAIL = 7;
    public static readonly int SUCCESS_ACCOUNT_ADDED = 8;
    public static readonly int ERROR_NETWORK = 9;
    public static readonly int ERROR_INVALID_CREDENTIALS = 10;
    public static readonly int SUCCESS_LOGIN = 11;
    public static readonly int ERROR_UNABLE_TO_OBTAIN_BOOK_LIST = 12;
    public static readonly int ERROR_NO_FAVORITES = 13;
    public static readonly int SUCCESS_PURCHASE = 14;

    private static readonly string[] messages_en_US = {
        "A user name is required.",
        "Invalid user name. User name can contain only letters, numbers, hyphens, or underscores.",
        "Invalid user name. Must be at least 5 characters long.",
        "A password is required.",
        "Invalid password. Must be at least 5 characters long.",
        "Passwords do not match.",
        "Invalid email.",
        "Account created successfully. Welcome to U.S. Comics.",
        "A network error occured.",
        "Invalid credentials.",
        "Login successful.",
        "Unable to obtain the list of your books from the server.",
        "You don't have any favorites.",
        "Purchase successful."
    };

    public static string GetMessage(Language inLanguage, int inMessageId) {
        if (Language.en_US == inLanguage) {
            if (0 >= inMessageId || messages_en_US.Length < inMessageId) return null;
            return messages_en_US[inMessageId - 1];
        }
        return null;
    }
}

public class MessageManager : MonoBehaviour {
    public enum Sound {UseErrorSound, UseSuccessSound,NoSound};
    public static MessageManager INSTANCE;
    
    public TextMeshProUGUI Message;
    public CanvasGroup MessageCanvasGroup;
    public ImageHelper ImageImage;
    public TextMeshProUGUI ImageMessage;
    public CanvasGroup ImageMessageCanvasGroup;
    public int Lifetime = 5; // seconds
    public int DefaultLifetime = 5; // seconds
    public AudioClip ErrorSound;
    public AudioClip SuccessSound;

    private readonly Queue<QueuedMessage> _messageQueue = new Queue<QueuedMessage>();
    private readonly Queue<QueuedMessage> _imageMessageQueue = new Queue<QueuedMessage>();
    private bool Visible = false;
    private float StartTime;
    private AudioSource AudioSource;

    // Use this for initialization
    void Start() {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
        if (null != mainCamera) AudioSource = mainCamera.GetComponent<AudioSource>();
        if (null == AudioSource) { Debug.LogError("MessageManager.Start: AudioSource is null."); }
        if (null == AudioSource) { return; }

        MessageManager.INSTANCE = this;
    }

    // Update is called once per frame
    void Update() {
        if ((true == Visible)
        && (Time.time >= StartTime + Lifetime))
        {
            HideMessage();
            HideImageMessage();
        } // if
    }

    public void ShowMessage(int inMessageId, Sound inUseSound = Sound.UseErrorSound, int inLifetime = -1) {
        string message = Messages.GetMessage( Messages.Language.en_US, inMessageId );
        if (null == message) {
            Debug.LogError("MessageManager::ShowMessage: Invalide message.");
            return;
        }
        ShowMessage(message, inUseSound, inLifetime);
    }

    public void ShowMessage(string inMessage, Sound inUseSound = Sound.UseErrorSound, int inLifetime = -1) {
        if (true == Visible) {
            _messageQueue.Enqueue(new QueuedMessage(inMessage, inUseSound, inLifetime));
            return;
        }
        if (-1 != inLifetime) Lifetime = inLifetime;
        else Lifetime = DefaultLifetime;
        Visible = true;
        StartTime = Time.time;
        Message.text = inMessage;
        MessageCanvasGroup.alpha = 1;
        MessageCanvasGroup.interactable = true;
        MessageCanvasGroup.blocksRaycasts = true;
        if (Sound.UseErrorSound == inUseSound && null != ErrorSound) { AudioSource.PlayOneShot(ErrorSound); }
        else if (Sound.UseSuccessSound == inUseSound && null != SuccessSound) { AudioSource.PlayOneShot(SuccessSound); }
    }

    public void PlaySuccessSound() {
        if (null != SuccessSound) { AudioSource.PlayOneShot(SuccessSound); }
    }

    public void PlayErrorSound() {
        if (null != ErrorSound) { AudioSource.PlayOneShot(ErrorSound); }
    }

    void HideMessage() {
        MessageCanvasGroup.alpha = 0;
        MessageCanvasGroup.interactable = false;
        MessageCanvasGroup.blocksRaycasts = false;
        Visible = false;
        StartTime = 0;
        if (0 == _messageQueue.Count) { return; }
        QueuedMessage msg = _messageQueue.Dequeue();
        ShowMessage(msg.Message, msg.Sound, msg.Lifetime);
    }

    public void ShowImageMessage(int inMessageId, Sound inUseSound = Sound.UseErrorSound, int inLifetime = -1) {
        string message = Messages.GetMessage( Messages.Language.en_US, inMessageId );
        if (null == message) {
            Debug.LogError("MessageManager::ShowMessage: Invalide message.");
            return;
        }
        ShowImageMessage(message, inUseSound, inLifetime);
    }

    public void ShowImageMessage(string inMessage, Sound inUseSound = Sound.UseErrorSound, int inLifetime = -1) {
        if (Visible) {
            _imageMessageQueue.Enqueue(new QueuedMessage(inMessage, inUseSound, inLifetime));
            return;
        }

        if (-1 != inLifetime) Lifetime = inLifetime;
        else Lifetime = DefaultLifetime;
        Visible = true;
        StartTime = Time.time;
        ImageMessage.text = inMessage;
        ImageMessageCanvasGroup.alpha = 1;
        ImageMessageCanvasGroup.interactable = true;
        ImageMessageCanvasGroup.blocksRaycasts = true;
        if (Sound.UseErrorSound == inUseSound && null != ErrorSound) { AudioSource.PlayOneShot(ErrorSound); }
        else if (Sound.UseSuccessSound == inUseSound && null != SuccessSound) { AudioSource.PlayOneShot(SuccessSound); }
    }

    void HideImageMessage() {
        ImageMessageCanvasGroup.alpha = 0;
        ImageMessageCanvasGroup.interactable = false;
        ImageMessageCanvasGroup.blocksRaycasts = false;
        Visible = false;
        StartTime = 0;
        if (0 == _imageMessageQueue.Count) { return; }
        QueuedMessage msg = _imageMessageQueue.Dequeue();
        ShowImageMessage( msg.Message, msg.Sound, msg.Lifetime );
    }
}

public class QueuedMessage {
    public QueuedMessage(string inMessage, MessageManager.Sound inUseSound, int inLifetime) {
        Message = inMessage;
        Sound = inUseSound;
        Lifetime = inLifetime;
    }
    public readonly string Message;
    public readonly MessageManager.Sound Sound;    
    public readonly int Lifetime;
}
