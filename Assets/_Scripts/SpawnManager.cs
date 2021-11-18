using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    [SerializeField]
    private GameObject[] figures;
    private GameObject newFigure;
    [SerializeField]
    private float timeToSpawn;
    private float timeToSpawnCounter;
    [SerializeField]
    private Color[] colors;

    #region CameraBounds
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private float offset;
    #endregion

    void Start()
    {
        offset = this.transform.localScale.x / 2;
        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
    }
    // Update is called once per frame
    void Update()
    {
        if(timeToSpawnCounter >= timeToSpawn)
        {
            SpawnNewFigure();
            timeToSpawnCounter = 0;
            TranslateSpawnPoint();
        }else
        {
            timeToSpawnCounter += Time.deltaTime;
        }
    }

    /// <summary>
    /// Spawn new figure from the array figures. Range -> 0-4 (inclusive)
    /// </summary>
    private void SpawnNewFigure()
    {
        newFigure = figures[Random.RandomRange(0,4)];
        newFigure.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length-1)];
        Instantiate(newFigure, this.transform.position, newFigure.transform.rotation);
    }

    private void TranslateSpawnPoint()
    {
        float newX = Random.RandomRange(bottomLeft.x + offset, topRight.x - offset);
        this.transform.position = new Vector3(newX, this.transform.position.y,
                                                    this.transform.position.z);
    }
}
