using System.Collections;
using UnityEngine;

public class SpawnAndDestroy : MonoBehaviour
{
    public GameObject[] objectPrefabs; // List of object prefabs to spawn
    public Transform spawnPoint; // Spawn point for the instances
   
    GameObject spawnedObject;
    int i = 0;
    private void Start()
    {
        StartCoroutine(SpawnInstances());
        StartCoroutine(Destroytances());
   
    }

    private IEnumerator SpawnInstances()
    {
        while (true)
        {
           
            int randomIndex = Random.Range(0, objectPrefabs.Length);
            spawnedObject = Instantiate(objectPrefabs[randomIndex], objectPrefabs[randomIndex].transform.position, objectPrefabs[randomIndex].transform.rotation);
            spawnedObject.transform.parent = transform;
            yield return new WaitForSeconds(0.4f);
            

        }
    }
    int f = 0;
    private IEnumerator Destroytances()
    {
        while (true)
        {
         

            yield return new WaitForSeconds(2.0f);
            GameObject ch = transform.GetChild(0).gameObject;
            Destroy(ch);
            //Debug.Log(transform.childCount);
            if (transform.childCount > 20) {
                for (int i = 0; i < 10; i++) {
                    Destroy(transform.GetChild(i).gameObject);

                }
            
            }
           
        }
    }
}
