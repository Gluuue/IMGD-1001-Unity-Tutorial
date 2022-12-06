using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public System.Action destroyed;
    private Collider2D targetObject;

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log(other.gameObject);
        if (!other.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
        }
    }

}
