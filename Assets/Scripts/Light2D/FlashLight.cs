using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    Light2D light2D;

    [SerializeField]private float maxIntensity;
    [SerializeField]private float minIntensity;
    private float currentintensity;
    private float currentWaitTime;
    [SerializeField]private float maxTime;
    [SerializeField]private float minTime;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        currentintensity = maxIntensity;
        light2D.intensity = currentintensity;
        currentWaitTime = - maxTime;

        StartCoroutine(UpdateLightIntensity());
    } 

    IEnumerator UpdateLightIntensity()
    {
        while(true)
        {
            currentWaitTime = Random.Range(minTime, maxTime);
            currentintensity = Random.Range(minIntensity, maxIntensity);
            light2D.intensity = currentintensity;

            yield return new WaitForSeconds(currentWaitTime);
        }
    }
}
