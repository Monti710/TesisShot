using UnityEngine;

public class VRShoot : MonoBehaviour
{
    public SimpleShoot simpleShoot;           // Referencia al script de disparo
    public OVRInput.Button shootButton;       // Botón asignado para disparar (ej. PrimaryIndexTrigger)

    private OVRGrabbable grabbable;           // Componente de agarre
    private AudioSource audio;                // Sonido al disparar

    void Start()
    {
        grabbable = GetComponent<OVRGrabbable>();
        audio = GetComponent<AudioSource>();

        if (grabbable == null)
            Debug.LogWarning("OVRGrabbable no encontrado en " + gameObject.name);

        if (audio == null)
            Debug.LogWarning("AudioSource no encontrado en " + gameObject.name);

        if (simpleShoot == null)
            Debug.LogWarning("SimpleShoot no está asignado en el Inspector.");
    }

    void Update()
    {
        // Asegurarse que todo está asignado antes de ejecutar disparo
        if (grabbable != null && grabbable.isGrabbed && grabbable.grabbedBy != null)
        {
            if (simpleShoot != null)
            {
                if (OVRInput.GetDown(shootButton, grabbable.grabbedBy.GetController()))
                {
                    simpleShoot.StartShoot();

                    if (audio != null)
                        audio.Play();
                }
            }
        }
    }
}
