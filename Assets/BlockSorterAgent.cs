using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BlockSorterAgent : Agent
{

    public GameObject floor;
    public GameObject env;
    public Bounds areaBounds;
    public GameObject target;
    public GameObject block;
    public Rigidbody blockRigidbody;
    public Rigidbody agentRigidbody;

    public override void Initialize()
    {
       agentRigidbody = GetComponent<Rigidbody>();
       blockRigidbody = block.GetComponent<Rigidbody>();
       areaBounds = floor.GetComponent<Collider>().bounds;
    }

    public void GoalScored()
    {
       AddReward(5f);
       EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
       var discreteActionsOut = actionsOut.DiscreteActions;
       discreteActionsOut[0] = 0;
       if (Input.GetKey(KeyCode.D))
       {
          discreteActionsOut[0] = 3;
       }
       else if (Input.GetKey(KeyCode.W))
       {
          discreteActionsOut[0] = 1;
       }
       else if (Input.GetKey(KeyCode.A))
       {
          discreteActionsOut[0] = 4;
       }
       else if (Input.GetKey(KeyCode.S)) 
       {
          discreteActionsOut[0] = 2;
       }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
       var direction = Vector3.zero;
       var rotation = Vector3.zero;
       var action = act[0];
       switch (action)
       {
           case 1:
              direction = transform.forward * 1f;
              break;
           case 2:
              direction = transform.forward * -1f;
              break;
           case 3:
              rotation = transform.up * 1f;
              break;
           case 4:
              rotation = transform.up * -1f;
              break;
           case 5:
              direction = transform.right * -0.75f;
              break;
           case 6:
              direction = transform.right * 0.75f;
              break;
       }
       transform.Rotate(rotation, Time.fixedDeltaTime * 200f);
       agentRigidbody.AddForce(direction * 1, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
			MoveAgent(actions.DiscreteActions);
			SetReward(-1f / MaxStep);
    }

    public override void OnEpisodeBegin()
    {
       var rotation = Random.Range(0, 4);
       var rotationAngle = rotation * 90f;
       this.block.transform.position = GetRandomPosition();
       this.blockRigidbody.velocity = Vector3.zero;
       this.blockRigidbody.angularVelocity = Vector3.zero;
       this.transform.position = GetRandomPosition();
       this.agentRigidbody.velocity = Vector3.zero;
       this.agentRigidbody.angularVelocity = Vector3.zero;
       this.env.transform.Rotate(new Vector3(0f, rotationAngle, 0f));
    }

    public Vector3 GetRandomPosition()
    {
       Bounds floorBounds = floor.GetComponent<Collider>().bounds;
       Bounds targetBounds = target.GetComponent<Collider>().bounds;
       Vector3 pointOnFloor;
       var watchdogTimer = System.Diagnostics.Stopwatch.StartNew();
       float margin = 1.0f;
       do
       {
          if (watchdogTimer.ElapsedMilliseconds > 30)
          {
             throw new System.TimeoutException("Too long man!");
          }
          pointOnFloor = new Vector3(Random.Range(floorBounds.min.x + margin, floorBounds.max.x - margin), floorBounds.max.y, Random.Range(floorBounds.min.z + margin, floorBounds.max.z - margin));
       } while (targetBounds.Contains(pointOnFloor));
       return pointOnFloor;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
