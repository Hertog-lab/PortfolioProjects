using System.Collections.Generic;
using UnityEngine;

public class GetEnemies : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();    //list of enemies in range
    private List<GameObject> satalites = new List<GameObject>(); //list of satalites in range
    private List<GameObject> contacts = new List<GameObject>(); //list of all radar contacts

    [SerializeField] private GameObject nothing;
    [SerializeField] private float scanRadius;



    private void Update()
    {
        

        RaycastHit2D[] scannerTargets = Physics2D.CircleCastAll(transform.position, scanRadius, Vector2.zero);
        if (InfoManager.instance._scannerIsActive) {
            if (scannerTargets != null) {
                for (int i = 0; i < scannerTargets.Length; i++) {
                    float targetAngle = GetAngleFromPlayer(scannerTargets[i].collider.gameObject);
                    if (targetAngle < 90) {
                        scannerTargets[i].collider.GetComponent<SignalEmitter>().SetVolume(GetAngleFromPlayer(scannerTargets[i].collider.gameObject));

                        if (scannerTargets[i].collider.GetComponent<SignalEmitter>() == true) {
                            enemies.Add(scannerTargets[i].collider.gameObject);                      //adds enemies to the list
                            InfoManager.instance._enemyAmount = enemies.Count;                  //updates the enemy count
                            InfoManager.instance._closestEnemy = enemies[0];                   //sets the closest enemy variable in the InfoManager
                        }

                        //checks if the collision object is a satalite
                        if (scannerTargets[i].collider.GetComponent<Satellite>() == true) {
                            satalites.Add(scannerTargets[i].collider.gameObject);            //adds satalites to the list
                            InfoManager.instance._sataliteAmount = satalites.Count;    //updates the satalite count
                            InfoManager.instance._closestSatalite = satalites[0];     //sets the closest satalite variable in the InfoManager
                        }

                    }
                }
            }
        }
        else
            return;

    }

    private int AngleSortFunc(GameObject a, GameObject b)
    {
        //checks the difference in angle from player between game objects and returns the right values to change their position on the list

        Vector3 aTargetDir = a.transform.position - transform.position;
        Vector3 bTargetDir = b.transform.position - transform.position;

        float aAngle = Vector3.Angle(aTargetDir, transform.up);
        float bAngle = Vector3.Angle(bTargetDir, transform.up);

        if (aAngle < bAngle) {
            return -1;
        }
        else if (aAngle > bAngle) {
            return 1;
        }
        else {
            return 0;
        }
    }

    public float GetAngleFromPlayer(GameObject target)
    {
        Vector3 TargetDir = target.transform.position - transform.position;

        float angle = Vector3.Angle(TargetDir, transform.up);
        return angle;
    }


    /*
     * 
     * //enemies.Sort(AngleSortFunc);
        //satalites.Sort(AngleSortFunc);

        //checks if objects enter range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the collision object is an enemy
        if (collision.GetComponent<Enemy>() == true) {
            enemies.Add(collision.gameObject);                      //adds enemies to the list
            InfoManager.instance._enemyAmount = enemies.Count;     //updates the enemy count
            InfoManager.instance._closestEnemy = enemies[0];     //sets the closest enemy variable in the InfoManager
        }

        //checks if the collision object is a satalite
        if (collision.GetComponent<Satalite>() == true) {
            satalites.Add(collision.gameObject);                        //adds satalites to the list
            InfoManager.instance._sataliteAmount = satalites.Count;    //updates the satalite count
            InfoManager.instance._closestSatalite = satalites[0];    //sets the closest satalite variable in the InfoManager
        }
    }

    //checks if enemies exit range
    private void OnTriggerExit2D(Collider2D collision)
    {
        //checks if the collision object is an enemy
        if (collision.GetComponent<Enemy>() == true) {
            enemies.Remove(collision.gameObject);                    //removes the enemy form the list
            InfoManager.instance._enemyAmount = enemies.Count;      //updates the enemy count
            if (enemies.Count >= 1)
                InfoManager.instance._closestEnemy = enemies[0];
            else
                InfoManager.instance._closestEnemy = nothing;
        }

        //checks if the collision object is a satalite
        if (collision.GetComponent<Satalite>() == true) {
            satalites.Remove(collision.gameObject);                    //removes the satalite from the list
            InfoManager.instance._sataliteAmount = satalites.Count;   //updates the satalite count
        }
    }

    //as named this function is used to sort the enemies from far away to close
    private int DistanceSortFunc(GameObject a, GameObject b)
    {
        //checks the distances between enemies and player and returns the right values to change their position on the list
        if (Vector2.Distance(a.transform.position, transform.position) < Vector2.Distance(b.transform.position, transform.position)) {
            return -1;
        }
        else if (Vector2.Distance(a.transform.position, transform.position) > Vector2.Distance(b.transform.position, transform.position)) {
            return 1;
        }
        else {
            return 0;
        }
    }
    */
}
