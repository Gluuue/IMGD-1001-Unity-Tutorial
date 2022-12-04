using UnityEngine;
using UnityEngine.SceneManagement;

public class Archer : MonoBehaviour
{

    public static Archer instance;
    public Projectile arrowPrefab;
    public float archerAttackRate = 1.0f;
    public Vector3 _direction = Vector3.left;

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating(nameof(ArcherAttack), this.archerAttackRate, this.archerAttackRate);
    }

    private void Update()
    {
        
    }

    private void ArcherAttack()
    {
        Instantiate(this.arrowPrefab, transform.position, Quaternion.FromToRotation(Vector3.down, Vector3.left));
    }

}
