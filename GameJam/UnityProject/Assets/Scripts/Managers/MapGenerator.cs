using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    [SerializeField] private float offset;
    [SerializeField] private Vector2 gridOrigin;
    private float randomRotation;
    private GameObject[] presets;

    private void Awake()
    {
        presets = Resources.LoadAll<GameObject>("Presets");
    }

    private void Start()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                randomRotation = Random.Range(0f, 360f);
                int random = Random.Range(0, presets.Length);
                Vector2 spawnPosition = new Vector2(x * offset, y * offset) + gridOrigin;
                GameObject clone = Instantiate(presets[random], spawnPosition, Quaternion.Euler(0f, 0f, randomRotation));
                if (y == gridY * 0.5 && x == gridX * 0.5)
                {
                    Instantiate(Resources.Load<GameObject>("Prefabs/Player"), clone.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
