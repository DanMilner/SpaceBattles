using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float coolDownTime = 10.0f;
    public float bulletSpeed = 10.0f;
    public float bulletLifeSpan = 10.0f;

    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cannonTower;
    [SerializeField] private Transform cannonGun;
    
    private float range;
    private Transform cannonTarget;
    private float coolDown;
    private Rigidbody shipRigidbody;
    private Quaternion rotation;
    private LineOfSight lineOfSight;
    private Transform bulletHolder;

    private void Awake()
    {
        shipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
        coolDown = coolDownTime;
        lineOfSight = cannonGun.GetComponent<LineOfSight>();
        bulletHolder = GameObject.FindGameObjectWithTag("BulletHolder").transform;
    }

    private void Update()
    {
        coolDown -= Time.deltaTime;

        cannonTower.LookAt(cannonTarget);
        rotation = new Quaternion(0, cannonTower.localRotation.y, 0, cannonTower.localRotation.w);
        cannonTower.localRotation = rotation;

        rotation = Quaternion.LookRotation(cannonTarget.position - cannonGun.position, transform.up);
        cannonGun.rotation = rotation;
    }

    public void FireCannon()
    {
        if (CanFire())
        {
            coolDown = coolDownTime;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position,
                bulletSpawn.transform.rotation, bulletHolder);
            bullet.GetComponent<IndividualBulletController>()
                .Fire(bulletSpawn.transform, shipRigidbody.velocity, bulletSpeed);

            Destroy(bullet, bulletLifeSpan);
        }
    }

    private bool CanFire()
    {
        if(coolDown > 0) {return false;}
        
        if(Vector3.Distance(cannonTarget.position, transform.position) > range) {return false;}
        
        return lineOfSight.SafeToFire();
    }

    public void SetRange(float range)
    {
        this.range = range;
    }

    public void SetCannonTarget(GameObject target)
    {
        cannonTarget = target.transform;
    }
}