using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControler : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms-1")] [SerializeField] float ControlSpeed = 50f;
    [Tooltip("In m")] [SerializeField] float xRange = 24f;
    [Tooltip("In m")] [SerializeField] float yRange = 18f;
    [SerializeField] GameObject[] guns;
    
    [Header("Screen-Position Based")]
    [SerializeField] float positionRollFactor = 1f;
    [SerializeField] float positionPitchFactor = -1.5f;
    [SerializeField] float positionYawFactor = 2.5f;
    
    [Header("Control-Throw Based")]
    [SerializeField] float controlPitchFactor = 1f;
    [SerializeField] float controlYawFactor = 1f;
    [SerializeField] float controlRollFactor = 0f;


    float xThrow;
    float yThrow;
    float zThrow;
    bool isControlEnabled = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    void OnPlayerDeath()
    {
        isControlEnabled = false;
    }
    private void ProcessTranslation()
    {
        float xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * ControlSpeed * Time.deltaTime;
        float yOffset = yThrow * ControlSpeed * Time.deltaTime;

        float rawNewX = transform.localPosition.x + xOffset;
        float rawNewY = transform.localPosition.y + yOffset;

        float clampedX = Mathf.Clamp(rawNewX, -xRange, xRange);
        float clampedY = Mathf.Clamp(rawNewY, -yRange, yRange);

        transform.localPosition = new Vector3(clampedX, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = new Vector3(transform.localPosition.x, clampedY, transform.localPosition.z);
    }

    private void ProcessRotation()
    {
        float zThrow = 1;

        float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controlPitchFactor;
        float yaw = transform.localPosition.x * positionYawFactor + xThrow * controlYawFactor;
        float roll = transform.localPosition.z * positionRollFactor * zThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(false);
        }
    }
    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns) // may affect death FX
        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

    
}
     
    

