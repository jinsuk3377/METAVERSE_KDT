using Mirror;
using Mirror.Examples.BilliardsPredicted;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using Cinemachine;

public class SessionPlayer : NetworkBehaviour
{
    public Transform playerCameraRoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // »Æ¿Œ : https://mirror-networking.gitbook.io/docs/manual/components/networkbehaviour
    public override void OnStartLocalPlayer()
    {
        GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
        GetComponent<ThirdPersonController>().enabled = true;
        FindObjectOfType<CinemachineVirtualCamera>().Follow = playerCameraRoot;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
