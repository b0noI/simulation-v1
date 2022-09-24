using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScore : MonoBehaviour
{

    public BlockSorterAgent agent;

    private void OnCollisionEnter(Collision collision)
    {
	if (collision.gameObject.CompareTag("goal"))
	{
	    agent.GoalScored();
	}
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
