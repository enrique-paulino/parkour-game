using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallRun : MonoBehaviour {

    public float wallRunUpForce;
    public float wallRunPushForce;

    bool isWallRunning;

    public Rigidbody rb;
    public Transform head, playerCamera, parentCamera, orientation;

    GameObject[] walls;
    GameObject wall;
    public float sphereRad;
    bool dir;

    Vector3 v;

    void Start() {
        walls = GameObject.FindGameObjectsWithTag("Runnable");
    }

    void Update() {
        if (isWallRunning) JumpOffWall();
        wallCheck();
    }

    void FixedUpdate() {
        v = rb.velocity;    
    }

    void wallCheck() {

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 3f);
        float smallestDist = Mathf.Infinity;
        foreach (var hit in hitColliders) {
            if (hit.transform.tag == ("Runnable")) {
                Vector3 wallPosition = hit.ClosestPointOnBounds(this.transform.position); 
                if (Vector3.Distance(this.transform.position, wallPosition) < smallestDist) wall = hit.gameObject;

            }
        }

        RaycastHit wallfound;
        if (Physics.Raycast(playerCamera.position, playerCamera.right, out wallfound, 5f)) {
            dir = false;
        }
        if (Physics.Raycast(playerCamera.position, -playerCamera.right, out wallfound, 5f)) {
            dir = true;
        }


    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.transform.CompareTag("Runnable")) {
            isWallRunning = true;
            rb.useGravity = false;

            if (dir == true) {
                playerCamera.transform.localEulerAngles = new Vector3(0f, 0f, -10f);
            } else {
                playerCamera.transform.localEulerAngles = new Vector3(0f, 0f, 10f);
            }
            
        }
    }


    void JumpOffWall() {
        if (Input.GetKeyDown(KeyCode.Space) && isWallRunning) {
            rb.useGravity = true;

            if (dir == true) {
                rb.AddForce(orientation.transform.right * wallRunPushForce, ForceMode.Impulse);
            }
            else {
                rb.AddForce(-orientation.transform.right * wallRunPushForce, ForceMode.Impulse);
            }

            rb.AddForce(Vector3.up * wallRunPushForce, ForceMode.Impulse);

            playerCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            isWallRunning = false;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.transform.CompareTag("Runnable")) {
            playerCamera.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            isWallRunning = false;
            rb.useGravity = true;
        }
    }
}
