using UnityEngine;
using System.Collections;

public class CustomTeleporter : MonoBehaviour
{
	//time to teleport
	public float TeleportTime = 3;

	//only allow specific tag object (empty = allow evey object)
	public string ObjectTag = "";

	public CustomTeleporter Destination;

	//private bool checking if you entered the trigger
	private bool inside;

	//object to teleport
	private Transform subject;

	private float countdown;

	void Start ()
	{
		countdown = TeleportTime;
	}


	void Update ()
	{
		if(inside)
		{
			countdown -= 1*Time.deltaTime;

			if(countdown < 0)
            {
				subject.transform.position = Destination.transform.position + new Vector3(0, 0.2f, 0);
				countdown = TeleportTime;
			}
		}
	}

	void OnTriggerEnter(Collider trig)
	{
		if(ObjectTag != "")
		{
			//if the objects tag is the same as the one allowed in the inspector
			if(trig.gameObject.tag == ObjectTag)
			{
				subject = trig.transform;

				inside = true;
			}
		}
		else
		{
			subject = trig.transform;

			inside = true;
		}
	}

	void OnTriggerExit(Collider trig)
	{
		if(ObjectTag != "")
		{
			//if the objects tag is the same as the one allowed in the inspector
			if(trig.gameObject.tag == ObjectTag)
			{
				inside = false;

				//reset countdown time
				countdown = TeleportTime;

				subject = null;
			}
		}
		else
		{
			inside = false;

			//reset countdown time
			countdown = TeleportTime;

			subject = null;
		}
	}
}
