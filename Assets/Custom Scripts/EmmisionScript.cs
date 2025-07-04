using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmisionScript : MonoBehaviour
{
    public Material targetMaterial;
    public Color emissionColor = Color.cyan;
    public float emissionIntensity = 5f;
    public float pulseDuration = 2f;
    public int flag = 0;
    private bool isBlinking = true;
    public bool isMgt = false;
    void Start()
    {
        if (targetMaterial != null)
        {
            targetMaterial.EnableKeyword("_EMISSION");
        }
        if(flag == 1)
        {
            StartBlinking();
        }
    }

    void Update()
    {
        if (flag == 1)
        {
            if (targetMaterial == null || !isBlinking) return;

            float emissionFactor = Mathf.PingPong(Time.time / (pulseDuration / 2f), 1f);
            Color finalEmission = emissionColor * (emissionFactor * emissionIntensity);

            targetMaterial.SetColor("_EmissionColor", finalEmission);
        }
    }

    // Call this function to stop blinking and reset emission
    public void ontouch()
    {
        StopBlinking();
    }

    public void StopBlinking()
    {
        flag = 0;
        isBlinking = false;
        if (targetMaterial != null)
        {
            targetMaterial.SetColor("_EmissionColor", Color.black);
        }
    }

    public void StartBlinking()
    {
        flag = 1;
        isBlinking = true;
        if (targetMaterial != null)
        {
            targetMaterial.EnableKeyword("_EMISSION");
        }
       // StartCoroutine(StopBlinkingAfterDelay(10f));
    }

    private IEnumerator StopBlinkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopBlinking();
    }

    private void OnDisable()
    {
       // flag = 0;
        StopBlinking();
    }

    private void OnEnable()
    {
        if (isMgt == true)
        {
            flag = 1;
            StartBlinking();
        }
    }
}
