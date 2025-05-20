using UnityEngine;
using Google;
using UnityEngine.UI;
using TMPro;

public class GoogleSignInExample : MonoBehaviour
{
    // GoogleSignInResult 객체를 저장할 변수
    private GoogleSignInUser googleUser;
    // Google 로그인 상태 표시할 UI 텍스트
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI emailText;
    //
    public GoogleUserIdToken googleUserIdToken;
    //
    private string webClientId = "107667134898-spf0o52a36cucclglo4dinnp0p4190nb.apps.googleusercontent.com"; //웹클라이언트 ID(Android)
    private string iosClientId = "107667134898-uvbihhun7u4tompu2jlltmiiid1iknoe.apps.googleusercontent.com"; // IOS용 클라이언트 ID
    //
    public Button loginButton;
    public Button logoutButton;
    void Start()
    {
        // GoogleSignIn SDK 초기화
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            WebClientId = GetPlatformClientId(),
            RequestIdToken = true,
            RequestEmail = true
        };
        
        loginButton.onClick.AddListener(SignInWithGoogle);
        logoutButton.onClick.AddListener(SignOutFromGoogle);
    }

    private string GetPlatformClientId()
    {
#if UNITY_ANDROID
        return webClientId;
#elif UNITY_IOS
    return iosClientId;
#else
    return null;
#endif
    }

    // 구글 로그인 실행
    public void SignInWithGoogle()
    {
        // Google 로그인
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Google 로그인 취소됨.");
                statusText.text = "로그인 취소됨.";
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Google 로그인 실패: " + task.Exception);
                statusText.text = task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }
            // 로그인 성공 시
            googleUser = task.Result;
            statusText.text = "로그인 성공: " + googleUser.DisplayName;
            tokenText.text = "ID 토큰 " + googleUser.IdToken.ToString();
            emailText.text = "이메일: " + googleUser.Email.ToString();
            Debug.Log("Google 로그인 성공: " + googleUser.DisplayName);
            Debug.Log("이메일: " + googleUser.Email);
            Debug.Log("ID 토큰 " + googleUser.IdToken);

            // 유저 정보 생성
            GoogleUser user = new GoogleUser();
            user.token = googleUser.IdToken.ToString();
            user.name = googleUser.IdToken.ToString();
            googleUserIdToken.user = user; // 유저 정보 전송
        });
    }

    // 로그아웃 처리
    public void SignOutFromGoogle()
    {
        try
        {
            GoogleSignIn.DefaultInstance.SignOut();
            Debug.Log("Google 로그아웃 성공.");
            statusText.text = "로그아웃 성공.";
            tokenText.text = "";
            emailText.text = "";
        }
        catch (System.Exception e)
        {
            Debug.LogError("Google 로그아웃 실패: " + e.Message);
            statusText.text = "로그아웃 실패.";
        }
    }
}
