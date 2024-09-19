using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static GunController main;

    public RectTransform crosshair;
    public Transform gunModel;
    public float distance;

    public Gun currentGun;
    public MuzzleFlash muzzleFlash;

    Vector3 rayDirection;
    
    public LayerMask layerMask;

    [SerializeField] AudioSource gunshotClip;
    
    void Awake()
    {
        if (main) Destroy(gameObject);
        else main = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGunModelRotation();
    }

    public void Shoot()
    {
        gunshotClip.Play();
        muzzleFlash.Flash();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, rayDirection, out hit, Mathf.Infinity, layerMask))
        {
            if (TryShootEffect(hit) == false) {
                TryShootPhysics(hit);
            }
        }
    }

    bool TryShootEffect(RaycastHit hit)
    {
        IShootable shootable = null;
        try {
            shootable = hit.collider.GetComponent<IShootable>();
            shootable.TakeShot();
            return true;
        }
        catch
        {
            return false;
        }
    }

    bool TryShootPhysics(RaycastHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null) return false;

        rb.AddForce(rayDirection * currentGun.knockbackForce);
        return true;
    }

    public void UpdateCrosshairPostiton(Vector2 screenPosition)
    {
        crosshair.anchoredPosition = screenPosition;
    }

    void UpdateGunModelRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(crosshair.anchoredPosition + new Vector2(Screen.width / 2, Screen.height / 2));
        rayDirection = ray.direction;

        Vector3 target = ray.direction * distance + Camera.main.transform.position;

        Vector3 targetDirection = gunModel.transform.position - target;
        Vector3 newDirection = Vector3.RotateTowards(gunModel.forward, targetDirection, Mathf.PI, 0);
        gunModel.transform.rotation = Quaternion.LookRotation(newDirection);
    }    
}
