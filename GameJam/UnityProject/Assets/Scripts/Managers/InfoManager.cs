using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    public int _enemyAmount;                              //used to show how many enemies are nearby
    public int _sataliteAmount;                          //used to show how many satalites are nearby
    public GameObject _closestEnemy;                    //stores the closest enemy
    public GameObject _closestSatalite;                //stores the closest satalite
    public bool _scannerIsActive;                     //used to check if the scanner is active
    public int _disconectedSatalites;                //used to check how many satalites the player still needs to connect
    public int _connectedSatalites;                 //used to updates the progressbar
    public Transform _lastPlayerLocation;          //gets updated to the last player location
    public Vector3 _currentPlayerVelocity;        //gets current velocity for the parallax effect
    public int _hitPoints;                       //player hitpoints
    public float _fixTimer;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
