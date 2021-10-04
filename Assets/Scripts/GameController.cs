using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;
    [SerializeField] private float timeBetweenWaves;

    private int waveSize = 1;
    private float timeUntilNextWave;
    
    void Start()
    {
        timeUntilNextWave = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if(timeUntilNextWave > 0)
        {
            timeUntilNextWave -= Time.deltaTime;            
        }
        else if(FindObjectsOfType<Enemy>().Length < 4)
        {
            for(int i = 0; i < waveSize; i++)
            {
                Instantiate(enemy, getSpawnPosition(), Quaternion.identity);
            }
            timeUntilNextWave = timeBetweenWaves;
            if(timeBetweenWaves > 2)
            {
                timeBetweenWaves--;
            }            
        }
    }

    private Vector3 getSpawnPosition()
    {
        int spawnArea = Random.Range(0, 4);

        if(spawnArea == 0)
        {
            return mainCamera.ScreenToWorldPoint(new Vector3(Random.Range(0,mainCamera.pixelWidth),mainCamera.pixelHeight + 50,mainCamera.nearClipPlane));
        }
        else if(spawnArea == 1)
        {
            return mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth + 50,Random.Range(0,mainCamera.pixelHeight), mainCamera.nearClipPlane));
        }
        else if(spawnArea == 2)
        {
            return mainCamera.ScreenToWorldPoint(new Vector3(Random.Range(0, mainCamera.pixelWidth), -50, mainCamera.nearClipPlane));
        }
        else
        {
            return mainCamera.ScreenToWorldPoint(new Vector3(-50, Random.Range(0, mainCamera.pixelHeight), mainCamera.nearClipPlane));
        }
    }
}
