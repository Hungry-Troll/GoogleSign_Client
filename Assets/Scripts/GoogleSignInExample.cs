using UnityEngine;
using Google;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GoogleSignInExample : MonoBehaviour
{
    // GoogleSignInResult ��ü�� ������ ����
    private GoogleSignInUser googleUser;
    // Google �α��� ���� ǥ���� UI �ؽ�Ʈ
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI emailText;
    //
    public GoogleUserIdToken googleUserIdToken;
    //
    private string webClientId = "107667134898-spf0o52a36cucclglo4dinnp0p4190nb.apps.googleusercontent.com"; //��
    //
    public Button loginButton;
    public Button logoutButton;
    void Start()
    {
        // GoogleSignIn SDK �ʱ�ȭ
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestEmail = true
        };
        
        loginButton.onClick.AddListener(SignInWithGoogle);
        logoutButton.onClick.AddListener(SignOutFromGoogle);
    }

    // ���� �α��� ����
    public void SignInWithGoogle()
    {
        // Google �α���
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Google �α��� ��ҵ�.");
                statusText.text = "�α��� ��ҵ�.";
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Google �α��� ����: " + task.Exception);
                statusText.text = task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            // �α��� ���� ��
            googleUser = task.Result;
            statusText.text = "�α��� ����: " + googleUser.DisplayName;
            tokenText.text = "ID ��ū " + googleUser.IdToken.ToString();
            emailText.text = "�̸���: " + googleUser.Email.ToString();
            Debug.Log("Google �α��� ����: " + googleUser.DisplayName);
            Debug.Log("�̸���: " + googleUser.Email);
            Debug.Log("ID ��ū " + googleUser.IdToken);

            // ���� ���� ����
            GoogleUser user = new GoogleUser();
            user.token = googleUser.IdToken.ToString();
            user.name = googleUser.IdToken.ToString();
            googleUserIdToken.user = user; // ���� ���� ����
        });
    }

    // �α׾ƿ� ó��
    public void SignOutFromGoogle()
    {
        try
        {
            GoogleSignIn.DefaultInstance.SignOut();
            Debug.Log("Google �α׾ƿ� ����.");
            statusText.text = "�α׾ƿ� ����.";
            tokenText.text = "";
            emailText.text = "";
        }
        catch (System.Exception e)
        {
            Debug.LogError("Google �α׾ƿ� ����: " + e.Message);
            statusText.text = "�α׾ƿ� ����.";
        }
    }
}
