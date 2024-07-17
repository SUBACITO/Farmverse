using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using UnityEngine.UI;

public class BlynkDataFetcher : MonoBehaviour
{
    private const string BLYNK_AUTH_TOKEN = "J0Ut9HGqlZi_oREbMizYfk9gKXG_dnAe";
    private const string TEMP_VPIN = "V1";
    private const string HUMI_VPIN = "V0";

    private float temperature;
    private float humidity;

    [Header("IOT SYSTEM")]
    public GameObject IOT_Table;
    public GameObject iot_log_item;

    private const int MaxLogs = 15;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to fetch data periodically
        StartCoroutine(FetchSensorData());
    }

    // Coroutine to fetch data from Blynk server
    private IEnumerator FetchSensorData()
    {
        while (true)
        {
            yield return StartCoroutine(FetchTemperature());
            yield return StartCoroutine(FetchHumidity());

            Debug.Log($"Temp: {temperature} °C\tHum: {humidity} %");
            DateTime currentTime = DateTime.Now;
            string timestamp = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
            AddLogItem($"[{timestamp}] Temp: {temperature} °C\tHum: {humidity} %");
            RemoveExcessLogs();

            // Wait for a specified duration before the next fetch
            yield return new WaitForSeconds(2.0f); // Example: Wait 5 seconds before fetching again
        }
    }

    // Adds a log item to the IOT_Table
    private void AddLogItem(string logText)
    {
        GameObject item = Instantiate(iot_log_item, iot_log_item.transform.position, Quaternion.identity, IOT_Table.transform);
        item.GetComponentInChildren<Text>().text = logText;
        item.transform.localScale = Vector3.one; // Ensure the instantiated item scales correctly
    }

    private void RemoveExcessLogs()
    {
        int childCount = IOT_Table.transform.childCount;

        if (childCount > MaxLogs)
        {
            for (int i = 0; i < childCount - MaxLogs; i++)
            {
                Destroy(IOT_Table.transform.GetChild(i).gameObject);
            }
        }
    }

    private IEnumerator FetchTemperature()
    {
        string url = $"https://blynk.cloud/external/api/get?token={BLYNK_AUTH_TOKEN}&pin={TEMP_VPIN}";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching temperature: {webRequest.error}");
            }
            else
            {
                string result = webRequest.downloadHandler.text;
                // Blynk returns data in a JSON-like array format, e.g., ["25.0"]
                temperature = float.Parse(result.Trim(new char[] { '[', ']', '"' }));
            }
        }
    }

    private IEnumerator FetchHumidity()
    {
        string url = $"https://blynk.cloud/external/api/get?token={BLYNK_AUTH_TOKEN}&pin={HUMI_VPIN}";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching humidity: {webRequest.error}");
            }
            else
            {
                string result = webRequest.downloadHandler.text;
                // Blynk returns data in a JSON-like array format, e.g., ["60.0"]
                humidity = float.Parse(result.Trim(new char[] { '[', ']', '"' }));
            }
        }
    }
}
