﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpinWheel : MonoBehaviour
{
	public List<int> prize;
	public List<AnimationCurve> animationCurves;
	
	private bool spinning;	
	private float anglePerItem;	
	private int randomTime;
	private int itemNumber;
	private GameObject[] colliders;
	private GameObject arrow;
	[SerializeField] private GameObject winner;
	
	void Start()
	{
		arrow = GameObject.FindGameObjectWithTag("Arrow");
		spinning = false;
		anglePerItem = 360/prize.Count;
		colliders = GameObject.FindGameObjectsWithTag("Collider");

	}
	
	void  Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !spinning) {
		
			randomTime = Random.Range (1, 4);
			itemNumber = Random.Range (0, prize.Count);
			float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
			
			StartCoroutine (SpinTheWheel (5 * randomTime, maxAngle));
		}
	}


	public void spin()
	{
		randomTime = Random.Range (3, 6);
		itemNumber = Random.Range (0, prize.Count);
		float maxAngle = 360 * (randomTime*5) + (itemNumber * anglePerItem);
			
		StartCoroutine (SpinTheWheel (5 * randomTime, maxAngle));
	}
	IEnumerator SpinTheWheel (float time, float maxAngle)
	{
	
		spinning = true;
		
		float timer = 0.0f;		
		float startAngle = transform.eulerAngles.z;		
		maxAngle = maxAngle - startAngle;
		
		int animationCurveNumber = Random.Range (0, animationCurves.Count);
		Debug.Log ("Animation Curve No. : " + animationCurveNumber);
		
		while (timer < time) {
			
		//to calculate rotation
		float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(1 - (timer / time)) + anglePerItem *
		              itemNumber;
			transform.eulerAngles = new Vector3 (0.0f, 0.0f, angle + startAngle);
			//transform.Rotate(angle * Vector3.forward);



			//transform.eulerAngles = Vector3.Slerp(transform.eulerAngles*startAngle,transform.eulerAngles*maxAngle,timer/time);
			timer += Time.deltaTime;
			yield return 0;
		}
		
		//transform.eulerAngles = new Vector3 (0.0f, 0.0f, maxAngle + startAngle);
		spinning = false;

		Vector3 lastPos = arrow.transform.position;
		String prize = "pooch";

		if(!(lastPos.x > colliders[colliders.Length-1].transform.position.x && lastPos.x < colliders[0].transform.position.x))
			for (int i = 1; i < colliders.Length; i++)
			{

				if (lastPos.x > colliders[i - 1].transform.position.x && lastPos.x < colliders[i].transform.position.x)
				{
					prize = colliders[i-1].GetComponentInChildren<Text>().text;
					break;
				}
			}
		else
		{
			prize = colliders[colliders.Length-1].GetComponentInChildren<Text>().text;
		}
		winner.gameObject.SetActive(true);
		winner.GetComponentInChildren<Text>().text = prize;
		Debug.Log ("Prize: " + prize);//use prize[itemNumnber] as per requirement
		
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(0);
	}	
}
