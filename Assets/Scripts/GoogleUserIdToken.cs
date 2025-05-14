using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using System;

public class GoogleUserIdToken : MonoBehaviour
{
    public Button serverButton; // ���� ���� ���� Ȯ�� ��ư
    public Button authButton;   // ���� Ȯ�� ��ư
    public TextMeshProUGUI responseText; // ���� ���� �ؽ�Ʈ
    public GoogleUser user;

    private string serverUrl = "http://172.30.1.29:38080/"; // ���� ���� �ּ�
    
    void Start()
    {
        authButton.onClick.AddListener(()=> StartCoroutine(CoCheckAuth()));
        serverButton.onClick.AddListener(() => StartCoroutine(CoCheckServer()));
    }

    IEnumerator CoCheckAuth() // ���� ���� ����
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

    IEnumerator CoCheckServer() // �׽�Ʈ
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
