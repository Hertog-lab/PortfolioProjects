using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    Transform threeHealth;
    Transform twoHealth;
    Transform oneHealth;

    private void Start()
    {
        InfoManager.instance._hitPoints = 3;
        oneHealth = transform.GetChild(2);
        twoHealth = transform.GetChild(3);
        threeHealth = transform.GetChild(4);
    }


    public void Update()
    {
        switch (InfoManager.instance._hitPoints) {
            case 3:
                threeHealth.gameObject.SetActive(true);
                twoHealth.gameObject.SetActive(false);
                oneHealth.gameObject.SetActive(false);
                break;
            case 2:
                threeHealth.gameObject.SetActive(false);
                twoHealth.gameObject.SetActive(true);
                oneHealth.gameObject.SetActive(false);
                break;
            case 1:
                threeHealth.gameObject.SetActive(false);
                twoHealth.gameObject.SetActive(false);
                oneHealth.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

}
