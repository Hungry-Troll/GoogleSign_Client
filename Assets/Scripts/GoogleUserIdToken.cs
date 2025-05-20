using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using System;

public class GoogleUserIdToken : MonoBehaviour
{
    public Button serverButton; // 로컬 서버 연결 확인 버튼
    public Button authButton;   // 권한 확인 버튼
    public TextMeshProUGUI responseText; // 서버 응답 텍스트
    public GoogleUser user;

    private string serverUrl = "http://172.30.1.29:38080/"; // 로컬 서버 주소
    
    void Start()
    {
        authButton.onClick.AddListener(()=> StartCoroutine(CoCheckAuth()));
        serverButton.onClick.AddListener(() => StartCoroutine(CoCheckServer()));
    }

    IEnumerator CoCheckAuth() // 유저 정보 전송
    {
        string auth = "auth";

        using (UnityWebRequest request = new UnityWebRequest(serverUrl + auth, "POST"))
        {
            string token = user.token.Trim();
            byte[] bodyRaw = Encoding.UTF8.GetBytes(token);
            request.SetRequestHeader("Content-Type", "text/plain");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                responseText.text = request.downloadHandler.text;
            }
            else
            {
                responseText.text = request.error;
            }
        }
    }

    IEnumerator CoCheckServer() // 테스트
    {
        string hello = "hello";
        UnityWebRequest request = UnityWebRequest.Get (serverUrl + hello);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseText.text = request.downloadHandler.text;
        }
        else 
        {
            responseText.text = request.error;
        }
    }
}

[Serializable]
public class GoogleUser
{
    public string token;
    public string name;
}

public struct ServerResponse
{
    public string message;
}
