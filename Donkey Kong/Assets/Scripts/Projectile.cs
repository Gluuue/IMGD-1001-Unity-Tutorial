using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public System.Action destroyed;

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D other)
    { 
        Destroy(this.gameObject);
    }

}
