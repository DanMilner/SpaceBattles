using UnityEngine;

public class BulletImpactController : MonoBehaviour
{
    [SerializeField] private float damage = 1.0f;

    private void OnParticleCollision(GameObject other)
    {
        other.GetComponentInChildren<ShipHealth>().TakeDamage(damage);
    }
}