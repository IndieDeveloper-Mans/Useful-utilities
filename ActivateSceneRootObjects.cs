using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSceneRootObjects : MonoBehaviour
{
    [Header("Activation Control")]
    public List<GameObject> rootObjects;
    [Space]
    public float delayToActivate = 0.1f;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // add only inactive objects
            if (!transform.GetChild(i).gameObject.activeInHierarchy)
            {
                rootObjects.Add(transform.GetChild(i).gameObject);
            }
        }

        if (rootObjects.Count > 0)
        {
            StartCoroutine(ActivateChildren());
        }
        else
        {
            Debug.Log("All children is active in " + gameObject.name + " destroing " + this + " script");

            this.enabled = false;
        }
    }

    public IEnumerator ActivateChildren()
    {
        int activeChildCount = 0;

        for (int i = 0; i < rootObjects.Count; i++)
        {
            rootObjects[i].SetActive(true);

            // stop on all activation ends
            activeChildCount++;

            if (activeChildCount == rootObjects.Count)
            {
                Debug.Log("Stop activating children in " + gameObject.name);

                yield break;
            }

            yield return new WaitForSeconds(delayToActivate);
        }
    }
}