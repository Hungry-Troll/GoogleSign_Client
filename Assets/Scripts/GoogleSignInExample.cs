using UnityEngine;
using Google;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GoogleSignInExample : MonoBehaviour
{
    // GoogleSignInResult ?????? ?????? ????
    private GoogleSignInUser googleUser;
    // Google ?????? ???? ?????? UI ??????
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI emailText;
    //
    public GoogleUserIdToken googleUserIdToken;
    //
    private string webClientId = "107667134898-spf0o52a36cucclglo4dinnp0p4190nb.apps.googleusercontent.com"; //??
    //
    public Button loginButton;
    public Button logoutButton;
    void Start()
    {
        // GoogleSignIn SDK ??????
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestEmail = true
        };
        
        loginButton.onClick.AddListener(SignInWithGoogle);
        logoutButton.onClick.AddListener(SignOutFromGoogle);
    }

    // ???? ?????? ????
    public void SignInWithGoogle()
    {
        // Google ??????
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Google ?????? ??????.");
                statusText.text = "?????? ??????.";
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Google ?????? ????: " + task.Exception);
                statusText.text = task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            // ?????? ???? ??
            googleUser = task.Result;
            statusText.text = "?????? ????: " + googleUser.DisplayName;
            tokenText.text = "ID ???? " + googleUser.IdToken.ToString();
            emailText.text = "??????: " + googleUser.Email.ToString();
            Debug.Log("Google ?????? ????: " + googleUser.DisplayName);
            Debug.Log("??????: " + googleUser.Email);
            Debug.Log("ID ???? " + googleUser.IdToken);

            // ???? ???? ????
            GoogleUser user = new GoogleUser();
            user.token = googleUser.IdToken.ToString();
            user.name = googleUser.IdToken.ToString();
            googleUserIdToken.user = user; // ???? ???? ????
        });
    }

    // ???????? ????
    public void SignOutFromGoogle()
    {
        try
        {
            GoogleSignIn.DefaultInstance.SignOut();
            Debug.Log("Google ???????? ????.");
            statusText.text = "???????? ????.";
            tokenText.text = "";
            emailText.text = "";
        }
        catch (System.Exception e)
        {
            Debug.LogError("Google ???????? ????: " + e.Message);
            statusText.text = "???????? ????.";
        }
    }
}
