using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {
    public GameObject player;
    private Transform ptransform;
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject topBound;
    public GameObject bottomBound;
    private Camera cam;
    public float dif;
    // Use this for initialization
    void Start () {
        try
        {
            ptransform = player.transform;
        }
        catch (MissingReferenceException e) { }
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        if(ptransform != null)
        {
            Vector3 playerpos = ptransform.position;
            playerpos.z = transform.position.z;
            float height = cam.orthographicSize;
            float width = height * cam.aspect;
            float minY = playerpos.y - height;
            float maxX = playerpos.x + width;
            float maxY = playerpos.y + height;
            float minX = playerpos.x - width;

            if (bottomBound.transform.position.y >= minY)
            {
                playerpos.y = bottomBound.transform.position.y + height;
            }
            if (topBound.transform.position.y <= maxY)
            {
                playerpos.y = topBound.transform.position.y - height;
            }
            if (leftBound.transform.position.x >= minX)
            {
                playerpos.x = leftBound.transform.position.x + width;
            }
            if (rightBound.transform.position.x <= maxX)
            {
                playerpos.x = rightBound.transform.position.x - width;
            }
            if (dif > 0)
                transform.position = new Vector3(playerpos.x + dif, 0, playerpos.z);
            else
                transform.position = new Vector3(playerpos.x + dif, playerpos.y, playerpos.z);
        }
        
    }



}
