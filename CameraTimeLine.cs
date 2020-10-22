using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CameraWayPoints))]
public class CameraTimeLine : MonoBehaviour
{
    private CameraWayPoints wayPoints;
    public enum MODE
    { 
        ONE_WAY,
        CIRCULAR
    }

    public MODE timelineMode;
    public float movementSmoothing;
    public float angleSmoothing;
    public bool resetOnStart;
    public int currentIndex;
    public bool tweening;
    // Start is called before the first frame update
    void Start()
    {
        wayPoints = GetComponent<CameraWayPoints>();
        if (!resetOnStart)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex = -1;
            ChangePosition();
        }
    }

    [ContextMenu("Next Point")]
    public void ChangePosition()
    {
        if (tweening) return;
        if(timelineMode==MODE.ONE_WAY)
        {
            if (currentIndex < wayPoints.points.Count-1)
            {
                currentIndex++;
            }
        }
        else if(timelineMode == MODE.CIRCULAR)
        {
            currentIndex++;
            currentIndex %= wayPoints.points.Count;
        }
        StartCoroutine(Interpolate(currentIndex));
    }


    IEnumerator Interpolate(int index)
    {
        tweening = true;
        while(Vector3.Distance(transform.position,wayPoints.points[index].position)>0)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints.points[index].position, movementSmoothing* Time.deltaTime);
            transform.rotation= Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(wayPoints.points[index].angles), angleSmoothing* Time.deltaTime);
            yield return null;
        }
        tweening = false;
    }
}
