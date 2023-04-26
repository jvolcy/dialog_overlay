using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform player;
    public float TrackingGain = 0.01f;

    float deltaX;
    float deltaZ;

    // Start is called before the first frame update
    void Start()
    {
        deltaX = transform.position.x - player.position.x;
        deltaZ = transform.position.z - player.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float zpos = transform.position.z + TrackingGain * (player.position.z - transform.position.z);
        transform.position = new Vector3(player.position.x + deltaX, transform.position.y, zpos);
    }
}
