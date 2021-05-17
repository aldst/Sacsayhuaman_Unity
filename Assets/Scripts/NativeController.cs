using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeController : MonoBehaviour
{
    public List<Transform> wayPoints;
    Animator compAnimator;
    public int currentTargetPoint = 0;
    float speed = 1.87f;
    float minSpeed = 0.90f;
    float maxSpeed = 1.87f;

    public Transform handIKTarget;
    public Transform stone;
    bool stoneDropped = false;
    Vector3 stoneFinalPosition=new Vector3(136.13f, 3.65f, -54.19f);
    // Start is called before the first frame update
    void Start()
    {
        compAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position=Vector3.MoveTowards(gameObject.transform.position, wayPoints[currentTargetPoint].transform.position, speed*Time.deltaTime);


        Vector3 targetXYPosition = new Vector3(wayPoints[currentTargetPoint].transform.position.x,gameObject.transform.position.y, wayPoints[currentTargetPoint].transform.position.z);
        Quaternion currentRotation = Quaternion.LookRotation(targetXYPosition - gameObject.transform.position);
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, currentRotation, 24f* Time.deltaTime);

        if (Vector3.Distance(gameObject.transform.position, wayPoints[currentTargetPoint].transform.position) < 0.2){
            if (currentTargetPoint < wayPoints.Count - 1)
            {
                currentTargetPoint++;

                if (currentTargetPoint == 1)
                {
                    compAnimator.SetTrigger("stairs");
                    speed = minSpeed;
                }else if (currentTargetPoint == 2)
                {
                    speed = maxSpeed;
                    compAnimator.SetTrigger("grounded");
                }else if (currentTargetPoint == 3)
                {

                    compAnimator.SetTrigger("stepDown");
                }
            }
            else
            {
                compAnimator.SetTrigger("drop");
            }
        }
        if(stoneDropped && stone.transform.position!=stoneFinalPosition)
        {
            stone.transform.position = Vector3.MoveTowards(stone.transform.position, stoneFinalPosition, 2f * Time.deltaTime);
        }
    }
    void OnAnimatorIK (int indexLayer)
    {
        float weight = compAnimator.GetFloat("carrying_weight");
        if (weight > 0.9)
        {
            compAnimator.SetLayerWeight(1, 0);
            Vector3 localPos = stone.transform.position;
            Vector3 worldPos = stone.transform.TransformPoint(localPos);
            stone.parent = null;
            stoneDropped = true;
        }
        compAnimator.SetIKPosition(AvatarIKGoal.LeftHand, handIKTarget.transform.position);
        compAnimator.SetIKPosition(AvatarIKGoal.RightHand, handIKTarget.transform.position);
        compAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
        compAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
    }
}
