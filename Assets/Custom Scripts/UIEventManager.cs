using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventManager : MonoBehaviour
{
    public void ActivateAfterDelay(GameObject target)
    {
        StartCoroutine(DelayedActivation(target));
    }

    private IEnumerator DelayedActivation(GameObject target)
    {
        yield return new WaitForSeconds(1f);

        if (target != null)
        {
            target.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Target GameObject is null.");
        }
    }
}
