using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
	private AudioManager AudioManager;
	private GameManager  gameManager;
	void Start()
	{
		AudioManager = GameManager.instance.GetComponent<AudioManager>();
		gameManager = GameManager.instance.GetComponent<GameManager>();
		arrow = GameObject.FindGameObjectWithTag("Arrow");
		spinning = false;
		anglePerItem = 360/prize.Count;
		colliders = GameObject.FindGameObjectsWithTag("Collider");

		try
		{
			string path = Application.dataPath+"/test.txt";
			StreamReader streamReader = new StreamReader(path);
			int i = 0;
			while (i < colliders.Length)
			{
				String input = streamReader.ReadLine();
				if (input == null)
				{
					break;
				}

				colliders[i].GetComponentInChildren<Text>().text = input;
				i++;
			}
		}
		catch (Exception e)
		{
				Debug.Log("oops");
		}


	}
	
	void  Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !spinning) {
		
			randomTime = Random.Range (1, 4);
			itemNumber = Random.Range (0, prize.Count);
			float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
			
			StartCoroutine (SpinTheWheel (5 * randomTime, maxAngle));
		}
		else
		{
			if(!spinning)
				transform.Rotate(Vector3.forward*Random.Range(1,20)*-1);
		}
	}


	public void spin()
	{
		if(spinning)
			return;
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

		float lastLength = AudioManager.waiting.clip.length;
		AudioManager.waiting.pitch = lastLength / time;
		//Debug.Log("time : " + time +"  length :"+lastLength + " now : "+AudioManager.waiting.pitch);
		AudioManager.playWaiting();
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
		
		AudioManager.playWinner();
		gameManager.playVictory();
		
		yield return new WaitForSeconds(AudioManager.winner.clip.length);

		AudioManager.waiting.pitch = 1;
		SceneManager.LoadScene(0);
		spinning = false;
		gameManager.playing = false;
	}	
}
