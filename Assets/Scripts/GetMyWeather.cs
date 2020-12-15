using System;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using SimpleJSON;
using UnityEngine.Networking;

public class GetMyWeather : MonoBehaviour
{
    public string currentIP;
    public string currentCountry;
    public string currentCity;

    //retrieved from weather API
    public string retrievedCountry;
    public string retrievedCity;
    public int conditionID;
    public string conditionName;
    public string conditionImage;

    void Start()
    {
        StartCoroutine(GetIP());
    }


    private IEnumerator GetIP()
    {
        var www = new UnityWebRequest("http://bot.whatismyipaddress.com/")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //error
            yield break;
        }

        currentIP = www.downloadHandler.text;

        StartCoroutine(GetLocationInfo());
    }

    private IEnumerator GetLocationInfo()
    {

//        currentIP = GetLocalIPAddress();
        WWW cityRequest = new WWW("http://www.geoplugin.net/json.gp?ip=" + currentIP); //get our location info
        yield return cityRequest;

        if (cityRequest.error == null || cityRequest.error == "")
        {
            var N = JSON.Parse(cityRequest.text);
            currentCity = N["geoplugin_city"].Value;
            currentCountry = N["geoplugin_countryName"].Value;

            StartCoroutine(GetWeatherInfo());
        }
        else
        {
            Debug.Log("WWW error: " + cityRequest.error);
        }
    }

    private IEnumerator GetWeatherInfo()
    {

        //get the current weather
        string ApiKey = "bd3f28ce174158546e71731f7a21dceb";
        //        WWW request = new WWW("http://api.openweathermap.org/data/2.5/weather?q=" + currentCity); //get our weather
        WWW request = new WWW("http://api.openweathermap.org/data/2.5/weather?q=" + currentCity + "&appid=" + ApiKey); //get our weather
        yield return request;

        if (request.error == null || request.error == "")
        {
            var N = JSON.Parse(request.text);

            retrievedCountry = N["sys"]["country"].Value; //get the country
            retrievedCity = N["name"].Value; //get the city

            string temp = N["main"]["temp"].Value; //get the temperature
            float tempTemp; //variable to hold the parsed temperature
            float.TryParse(temp, out tempTemp); //parse the temperature
            float finalTemp = Mathf.Round((tempTemp - 273.0f) * 10) / 10; //holds the actual converted temperature

            int.TryParse(N["weather"][0]["id"].Value, out conditionID); //get the current condition ID
            conditionName = N["weather"][0]["main"].Value; //get the current condition Name
//            conditionName = N["weather"][0]["description"].Value; //get the current condition Description
            conditionImage = N["weather"][0]["icon"].Value; //get the current condition Image

            //put all the retrieved stuff in the label
            Debug.Log(
                "Country: " + retrievedCountry
                + "\nCity: " + retrievedCity
                + "\nTemperature: " + finalTemp + " C"
                + "\nCurrent Condition: " + conditionName
                + "\nCondition Code: " + conditionID);

            WeatherManager.Instance.InitWeatherEffects(conditionName);
        }
        else
        {
            Debug.Log("WWW error: " + request.error);
        }
    }

    
}
