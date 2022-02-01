using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{

    [Header("General")]
    public Rigidbody rb;
    public Transform orientation, player, playerCamera;

    [Header("Skills")]
    public bool dash;
    public bool swing;

    [Header("Dash Skill")]
    public float dashStrength;

    [Header("Swing Skill")]
    public LineRenderer lr;
    public Transform swingLocation;
    public float startSwingMaxDistance;
    public LayerMask swingableLayer;
    Vector3 swingPoint;
    SpringJoint joint;


    public void Update() {
        CheckSkills();
        DrawRope();
    }

    

    public void CheckSkills() {
        if (dash && Input.GetKeyDown(KeyCode.R)) Dash();

        if (swing && Input.GetMouseButtonDown(0)) StartSwing();
        if (swing && Input.GetMouseButtonUp(0)) StopSwing();

    }
    
    // Dash Skill
    public void Dash() {
        Vector3 direction = orientation.transform.forward;
        rb.AddForce(direction * dashStrength * 2, ForceMode.Impulse);
    }

    // Swinging Skill
    void StartSwing() {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, startSwingMaxDistance, swingableLayer)) {
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.4f;
            joint.minDistance = distanceFromPoint * 0.2f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentSwingPosition = swingLocation.position;
        }

    }

    void StopSwing() {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentSwingPosition;

    void DrawRope() {
        if (!joint) return;

        currentSwingPosition = Vector3.Lerp(currentSwingPosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, swingLocation.position);
        lr.SetPosition(1, currentSwingPosition);
    }

    public bool IsSwinging() { 
        return joint != null; 
    }

    public Vector3 GetSwingPoint() {
        return swingPoint;
    }

}
