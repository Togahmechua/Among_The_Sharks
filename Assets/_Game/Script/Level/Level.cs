using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform lookAt;

    public PlayerController playerCtrl;

    private void Start()
    {
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();

        if (cam != null)
        {
            cam.Follow = player;
            cam.LookAt = lookAt;
        }
    }

}
