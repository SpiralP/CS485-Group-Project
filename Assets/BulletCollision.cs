using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// shot out of our ship, will hit enemies
public class BulletCollision : MonoBehaviour {
    public GameObject prefab;

    void Start() {

    }

    void Update() {

    }

    void OnCollisionEnter(Collision collision) {
        // fires events other things can listen to
        if (collision.gameObject.tag == "Enemy") {
            //
            // } elseif ("player") {
            //
        }
    }

}
