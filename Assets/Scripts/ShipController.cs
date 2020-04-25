using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
    public Transform movementPlane;

    private float stepX;
    private float stepY;
    //movement speed in units per second

    // start in the middle: 0, 0
    private int posX = 0;
    private int posY = 0;

    public bool canPlayerMove;

    void Start() {
        canPlayerMove = true;
        stepX = (movementPlane.localScale.x * 5f) / Game.gridPositionsX;
        stepY = (movementPlane.localScale.z * 5f) / Game.gridPositionsY;
    }

    void Update() {
        if (canPlayerMove) {
            Vector3 wantedPos = new Vector3(posX * stepX, posY * stepY, 0);

            if (Input.GetKeyDown("w")) {
                // move up
                posY = Mathf.Min(posY + 1, Game.gridPositionsY - 1);
            } else if (Input.GetKeyDown("s")) {
                posY = Mathf.Max(posY - 1, -1 * Game.gridPositionsY + 1);
            } else if (Input.GetKeyDown("a")) {
                posX = Mathf.Max(posX - 1, -1 * Game.gridPositionsX + 1);
            } else if (Input.GetKeyDown("d")) {
                posX = Mathf.Min(posX + 1, Game.gridPositionsX - 1);
            }

            transform.position = Vector3.MoveTowards(transform.position, wantedPos, 10f * Time.deltaTime);

        }
    }
}
