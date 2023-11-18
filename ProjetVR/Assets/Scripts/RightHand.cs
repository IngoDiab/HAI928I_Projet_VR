using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightHand : MonoBehaviour
{
    [SerializeField] XRRayInteractor xRRayInteractor = null;
    [SerializeField] Weapon mWeapon = null;
    [SerializeField] InputActionReference pushButtonReference = null;

    private void Awake()
    {
        xRRayInteractor.selectEntered.AddListener((nothing) =>
        {
            XRBaseInteractable _interactable = xRRayInteractor.interactablesSelected[0] as XRBaseInteractable;
            if (!_interactable) return;
            Debug.Log("Grab : " + _interactable.name);
            mWeapon = _interactable.GetComponent<Weapon>();
            if (!mWeapon) return;
            Debug.Log("Now having a weapon : " + mWeapon.name);
        });

        xRRayInteractor.selectExited.AddListener((nothing) =>
        {
            mWeapon = null;
        });

        pushButtonReference.action.performed += UseWeapon;
        //pushButtonReference.action.canceled += StopUsingWeapon;
    }

    void UseWeapon(InputAction.CallbackContext obj)
    {
        if (!mWeapon || mWeapon.IsFiring) return;
        mWeapon.TriggerPressed();
    }

    /*void StopUsingWeapon(InputAction.CallbackContext obj)
    {
        if (!mWeapon) return;
        mWeapon.IsFiring = false;
    }*/

    // Update is called once per frame
    void Update()
    {

    }
}
