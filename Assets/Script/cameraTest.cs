using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraTest : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera[]_cameraSettings;
    [SerializeField] private int _whichCamera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            foreach(var c in _cameraSettings)
            {
                c.Priority = 10;
            }

            _cameraSettings[_whichCamera].Priority = 15;
        }
    }
}

// https://medium.com/@austinjy13/using-triggers-to-change-cameras-unity-cinemachine-fb4825fa1885