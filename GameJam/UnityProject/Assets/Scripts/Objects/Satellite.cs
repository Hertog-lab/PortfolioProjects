using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    private SpriteRenderer sr;
    private AudioSource audioSource;
    [SerializeField] private AudioSource fixSoundSource;
    public bool isFixed;
    void Start()
    {
        InfoManager.instance._disconectedSatalites += 1;
        audioSource = GetComponent<AudioSource>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    private void Update()
    {
        if (InfoManager.instance._scannerIsActive && !isFixed) {
            sr.enabled = true;
        }
        else {
            sr.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isFixed != true) {
            if (collision.GetComponent<Movement>() == true) {
                if (InfoManager.instance._scannerIsActive)
                    InfoManager.instance._fixTimer += Time.deltaTime;
                if (InfoManager.instance._fixTimer > 3)
                    FixSatellite();
            }
        }
    }
    private void FixSatellite()
    {
        InfoManager.instance._fixTimer = 0;
        InfoManager.instance._connectedSatalites += 1;
        isFixed = true;
        fixSoundSource.Play();
        audioSource.enabled = false;
        if (InfoManager.instance._hitPoints < 3) {
            InfoManager.instance._hitPoints++;
            //TODO Add happy fixing sound!
        }
    }
}