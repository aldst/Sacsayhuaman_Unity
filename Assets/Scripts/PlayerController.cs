using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 7.5f;
    [HideInInspector] public float originalSpeed;
    [HideInInspector] public bool started = false;
    public GameObject wayPointsParent;
    public List<Transform> Waypoints;
    public int previousPoint;
    public int currentPoint;
    public int nextPoint;


    public PanelManager panelManager;

    public TourSoundingManager tourSoundingManager;


    private void Start()
    {
        panelManager.disabelPanels();
        previousPoint = 0;
        currentPoint = 0;
        nextPoint = 1;
        originalSpeed = speed;


        Waypoints = new List<Transform>(wayPointsParent.GetComponentsInChildren<Transform>());
        Waypoints.RemoveAt(0);
    }

    void Update()
    {
        if (started)
        {
            if (Input.GetKeyDown(KeyCode.W) && currentPoint != Waypoints.Count - 1)
            {
                panelManager.disabelPanels();
                speed = originalSpeed;
                if (transform.position != Waypoints[currentPoint].position)
                {
                    if (nextPoint < currentPoint)
                    {
                        nextPoint = currentPoint;
                        currentPoint -= 1;
                    }
                    else if (nextPoint == currentPoint)
                    {
                        nextPoint = currentPoint + 1;
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, Waypoints[nextPoint].position, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.W) && currentPoint != Waypoints.Count - 1)
            {
                if (transform.position != Waypoints[currentPoint].position)
                {
                    if (nextPoint < currentPoint)
                    {
                        nextPoint = currentPoint;
                        currentPoint -= 1;
                    }
                    else if (nextPoint == currentPoint)
                    {
                        nextPoint = currentPoint + 1;
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, Waypoints[nextPoint].position, speed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.S) && currentPoint != 0)
            {
                panelManager.disabelPanels();
                if (transform.position != Waypoints[currentPoint].position)
                {
                    if (nextPoint >= currentPoint)
                    {
                        nextPoint = currentPoint;
                    }
                    else
                    {
                        nextPoint = previousPoint;
                    }
                }
                else
                {
                    nextPoint = previousPoint;
                }
                transform.position = Vector3.MoveTowards(transform.position, Waypoints[nextPoint].position, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (transform.position != Waypoints[currentPoint].position)
                {
                    if (nextPoint >= currentPoint)
                    {
                        nextPoint = currentPoint;
                    }
                    else
                    {
                        nextPoint = previousPoint;
                    }
                }
                else
                {
                    nextPoint = previousPoint;
                }
                transform.position = Vector3.MoveTowards(transform.position, Waypoints[nextPoint].position, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                panelManager.showManu();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WayPoint")
        {
            WayPoint temp = other.gameObject.GetComponent<WayPoint>();
            if (temp.name != "-")
            {
                currentPoint = int.Parse(temp.name);
                previousPoint = int.Parse(temp.previousPoint.name);
                nextPoint = int.Parse(temp.nextPoint.name);
            }
            panelManager.panelControl(temp.type, temp.numberInteraction);

            tourSoundingManager.handleNewDestiny(temp.numberTourClip);
        }
    }
}
