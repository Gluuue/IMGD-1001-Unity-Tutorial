using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;
    public bool isActive = true;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        
        if (isActive) {
            Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
        }
        
    }

}
