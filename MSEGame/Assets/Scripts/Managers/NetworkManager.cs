using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json.Linq;

enum RequestType
{
    POST,
    GET,
    PUT,
    DELETE
}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public string url = "localhost";
    public string port = "8080";

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("Send Request")]
    private async void testRequest()
    {

        string apiKey = "0d8dc82ca22ed494ecc0955e0a6187cc";
        float lat = 37.532600f;
        float lon = 127.024612f; 
        string path = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";      

        UnityWebRequest req = CreateRequest(path, RequestType.GET);
        var operation = req.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        var jsonResponse = req.downloadHandler.text;

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log($"Success: {req.downloadHandler.text}");
        else
            Debug.Log($"Failed: {req.error}");

        try
        {
            JObject o = JObject.Parse(jsonResponse);
            Debug.Log($"Response Code: {o.SelectToken("weather[0].main")}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse {jsonResponse}. {ex.Message}");
        }
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    async Task<object> SendRequestWithResponse(UnityWebRequest req)
    {
        var operation = req.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        var jsonResponse = req.downloadHandler.text;

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log($"Success: {req.downloadHandler.text}");
        else
            Debug.Log($"Failed: {req.error}");

        try
        {
            object obj = JsonUtility.FromJson<object>(jsonResponse);
            return obj;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse {jsonResponse}. {ex.Message}");
        }

        return null;
    }

    public async Task<Card> GetCard(CardType type)
    {
        UnityWebRequest req = CreateRequest($"{url}:{port}/card?type={type}", RequestType.GET);

        object obj = await SendRequestWithResponse(req);

        // Assign different type here
        Card card = obj as Card;
        Debug.Log($"GET CARD RESPONSE: {obj}");

        return card;
        //return new EquipmentCard("Helmet of Coolness", EquipmentType.Helmet, Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.zero), 10, 5);
    }

    public async void PostPlayer(Player player)
    {
        UnityWebRequest req = CreateRequest($"{url}:{port}/player", RequestType.POST, player);

        var response = await SendRequestWithResponse(req);
        Debug.Log($"POST PLAYER RESPONSE: {response}");
    }


}
