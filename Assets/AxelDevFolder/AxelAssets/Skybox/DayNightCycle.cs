using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

// inspired by immersive insiders on YouTube
 
public class DayNightCycle : MonoBehaviour
{
    // variable to store a light source
    [SerializeField] private Light sun;

    // variable to store the time of the day
    [Range(0, 24)] public float timeOfDay;
    public int day = 1;

    // variable to store the speed of rotation
    [SerializeField] private float sunRotationSpeed;
    public float dayHour;
    public float nightHour;

    // variables to store the lighting presets
    [Header("Lighting Presets")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;
    public bool isDay = false;
    private bool isDisplayed = false;

    [SerializeField] private GameObject dayHourPanel;

    private void FixedUpdate()
    {
        timeOfDay += Time.fixedDeltaTime * sunRotationSpeed;
        if (timeOfDay > 24)
        {
            timeOfDay = 0;
            day++;
        }

        if(timeOfDay >= 5 && !isDisplayed)
        {
            dayHourPanel.SetActive(true);
            isDisplayed = true;
        }
        if(timeOfDay >= 5.1f && isDisplayed)
        {
            dayHourPanel.SetActive(false);
            isDisplayed = false;
        }

        UpdateSunRotation();
        UpdateLighting();
        isDay = timeOfDay > dayHour && timeOfDay < nightHour;
        dayHourPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Jour " + day.ToString();
        string hourDisplay = Mathf.Floor(timeOfDay) < 10 ? "0"+Mathf.Floor(timeOfDay).ToString() : Mathf.Floor(timeOfDay).ToString();
        string minuteDisplay = Mathf.Floor((timeOfDay - Mathf.Floor(timeOfDay)) * 60) < 10 ? "0" + Mathf.Floor((timeOfDay - Mathf.Floor(timeOfDay)) * 60).ToString() : Mathf.Floor((timeOfDay - Mathf.Floor(timeOfDay)) * 60).ToString();
        dayHourPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hourDisplay + "h" + minuteDisplay;
    }

    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLighting();
    }

    // function to update Sun's rotation
    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-50, 210, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z);
    }

    // function to update the lighting
    private void UpdateLighting()
    {
        float timeFraction = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
    }
}
