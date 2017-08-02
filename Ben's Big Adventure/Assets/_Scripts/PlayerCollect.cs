using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour {
    private void OnTriggerEnter(Collider collider)
    {
        //Player collect object
        print(name + " has been collected");
    }
}
