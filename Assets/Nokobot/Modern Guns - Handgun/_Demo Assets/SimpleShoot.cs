using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Time to destroy the casing object")]
    [SerializeField] private float destroyTimer = 2f;

    [Tooltip("Bullet speed")]
    [SerializeField] private float shotPower = 500f;

    [Tooltip("Casing ejection force")]
    [SerializeField] private float ejectPower = 150f;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    public void StartShoot()
    {
        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            Debug.LogWarning("No se encontró el Animator del arma.");
        }
    }

    // Animation Event: Llama a este método desde un evento de animación
    void Shoot()
    {
        if (muzzleFlashPrefab && barrelLocation)
        {
            GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(tempFlash, destroyTimer);
        }

        if (!bulletPrefab || !barrelLocation)
        {
            Debug.LogWarning("Falta asignar BulletPrefab o BarrelLocation.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(barrelLocation.forward * shotPower);
        }
        else
        {
            Debug.LogWarning("El BulletPrefab no tiene Rigidbody asignado.");
        }
    }

    // Animation Event: Llama a este método desde un evento de animación
    void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab)
        {
            Debug.LogWarning("Falta CasingPrefab o CasingExitLocation.");
            return;
        }

        GameObject tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
        Rigidbody casingRb = tempCasing.GetComponent<Rigidbody>();

        if (casingRb != null)
        {
            casingRb.AddExplosionForce(
                Random.Range(ejectPower * 0.7f, ejectPower),
                casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f,
                1f
            );

            casingRb.AddTorque(
                new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)),
                ForceMode.Impulse
            );
        }

        Destroy(tempCasing, destroyTimer);
    }
}
