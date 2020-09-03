using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Api : MonoBehaviour
{
    public GameObject wonPanelPrefab;
    public GameObject wonPanel;

    private static string _sID;
    private static Api _instance;

    private void Awake()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public static Api Instance()
    {
        return !Exists() ? null : _instance;

        bool Exists()
        {
            return _instance != null;
        }
    }

    private void Start()
    {
        _sID = Application.absoluteURL.Split('=').Last().Replace("/", "");
        StartCoroutine(StartGame());
        // var r = "{\"Achievement\":{\"Id\":62,\"SessionId\":\"5314bc4a-649b-ea11-8236-fcfe9e584587\",\"Level\":1,\"Score\":200,\"AwardedOn\":null,\"UpdatedOn\": \"2020-05-21T13:09:45.9680348Z\",\"Status\": 2,\"ErrorText\": null,\"TransactionId\": null,\"IsReachedRewardLimit\": false,\"Rewards\":[{\"Reward\":{\"Id\":157,\"RewardKey\":\"GameCoins\",\"RewardIconUrl\":\"\",\"Color\": \"f1c040\",\"Priority\":60},\"Amount\":1000}]},\"Status\": true}";
        // debugText.text = _sID;
    }

    private static IEnumerator StartGame()
    {
        const string startGameUrl = "https://dev.arcadespro.com/ArcadesOnline.WebApi/StartGame";
        var request = new Dictionary<string, string> {["SessionId"] = _sID};
        var json = JsonConvert.SerializeObject(request);
        var uhText = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        var www = new UnityWebRequest {method = UnityWebRequest.kHttpVerbPOST, url = startGameUrl};
        var dhJson = new DownloadHandlerBuffer();
        www.uploadHandler = uhText;
        www.downloadHandler = dhJson;
        www.SetRequestHeader("Authorization", "token 5D13BB80-6E2F-4B1B-B2F0-CC2501B71622");
        www.SetRequestHeader("Content-Type", "application/json");
        var result = www.SendWebRequest();
        while (!result.isDone)
        {
            print("Waiting for responce from API");
            yield return null;
            print(result.webRequest.error);
        }
        
        print("Starting game: " + dhJson.text);
    }

    public IEnumerator ReportGame(string score)
    {
        if (!int.TryParse(score, out var intScore))
        {
            print("Score is not a string of digits");
            yield break;
        }
        
        print("Reporting game");

        if (intScore == 0) score = "1";
        const string reportUrl = "https://dev.arcadespro.com/ArcadesOnline.WebApi/ReportAchievement";
        var request = new Dictionary<string, string> {["SessionId"] = _sID, ["Score"] = score, ["Level"] = "1"};
        var json = JsonConvert.SerializeObject(request);
        var uhText = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        var www = new UnityWebRequest {method = UnityWebRequest.kHttpVerbPOST, url = reportUrl};
        var dhJson = new DownloadHandlerBuffer();
        www.uploadHandler = uhText;
        www.downloadHandler = dhJson;
        www.SetRequestHeader("Authorization", "token 5D13BB80-6E2F-4B1B-B2F0-CC2501B71622");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        while (!www.isDone)
        {
            print("waiting for responce from the api");
            yield return null;
        }
        // var q = JsonConvert.DeserializeObject<Responce>(dhJson.text);
        // var rewards = q.Achievement.Rewards;
        // print(rewards.Length);
        //
        // foreach (var reward in rewards)
        // {
        //     var i = Instantiate(wonPanelPrefab, wonPanel.transform).transform;
        //     i.GetComponentInChildren<TMP_Text>().text = "זכית ב – " + Reverse(reward.Amount.ToString());
        //
        //     // var url = reward.Reward.RewardIconUrl;
        //     // print(url);
        //     // var r = new UnityWebRequest(url);
        //     // var iconDh = new DownloadHandlerTexture();
        //     // r.downloadHandler = iconDh;
        //     // r.method = UnityWebRequest.kHttpVerbGET;
        //     // r.SendWebRequest();
        //     // while (!r.isDone)
        //     // {
        //     //     print("waiting to download the icon");
        //     //     yield return null;
        //     // }
        //     // print("Error: " + r.error + " Bytes length: " + r.downloadedBytes + " " + iconDh.texture);
        //     // i.GetComponentInChildren<RawImage>().texture = iconDh.texture;
        //     // print("Assigned texture " + iconDh.data.Length);
        // }
    }

    private static string Reverse(string s)
    {
        var charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public IEnumerator EndGame(int score)
    {
        if (_sID == null) yield break;
        
        if (score == 0) score = 1;
        print("Ending game");
    
        const string endGameUrl = "https://app.arcadespro.com/ArcadesOnline.WebApi/EndGame";
        var request = new Dictionary<string, string> {["SessionId"] = _sID, ["Score"] = score.ToString(), ["Level"] = "1"};
        var json = JsonConvert.SerializeObject(request);
        var uhText = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        var www = new UnityWebRequest {method = UnityWebRequest.kHttpVerbPOST, url = endGameUrl};
        var dhJson = new DownloadHandlerBuffer();
        www.uploadHandler = uhText;
        www.downloadHandler = dhJson;
        www.SetRequestHeader("Authorization", "token 5D13BB80-6E2F-4B1B-B2F0-CC2501B71622");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        while (!www.isDone) yield return null;
        var q = JsonConvert.DeserializeObject<Responce>(dhJson.text);
        var rewards = q.Achievement.Rewards;
        print(rewards.Length);

        foreach (var reward in rewards)
        {
            var i = Instantiate(wonPanelPrefab, wonPanel.transform).transform;
            i.GetComponentInChildren<TMP_Text>().text = "זכית ב – " + Reverse(score.ToString());

            var r = new UnityWebRequest((string) reward.Reward.RewardIconUrl);
            print(reward.Reward.RewardIconUrl);
            var iconDh = new DownloadHandlerTexture();
            r.downloadHandler = iconDh;
            r.method = UnityWebRequest.kHttpVerbGET;
            r.SendWebRequest();
            while (!r.isDone) yield return null;
            print(r.error);
            i.GetComponentInChildren<RawImage>().texture = iconDh.texture;
            print("Assigned texture " + iconDh.data.Length);
        }
    }
}