using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
	static int numParticles = 40; 
	public GameObject Prefab; 
	private GameObject[] Particles = new GameObject[numParticles];

	public const float stringLength = 4.0f; 
	float interval = stringLength / numParticles; 
	float x0 = 0.0f; 

	float[] initialX = new float[numParticles]; 

	void Start()
	{
		for (int i = 0; i < numParticles; i++) 
		{
			Particles[i] = GameObject.Instantiate(Prefab) 
				as GameObject; 

			Particles[i].transform.position = 
				new Vector3(x0 + interval * i, 0, 0); 

			initialX[i] = x0 + interval * i; 

//			Debug.Log(Particles[i].transform.position.x); 
//			Debug.Log(initialX[i]); 
		}
	}

	float dt = 0.005f; 
	const float lambda = 2.0f; 
	const float k = 2 * Mathf.PI / lambda; 
	const float freq = 1.0f; 
	const float omega = 2 * Mathf.PI * freq; 
	const float ampl = 0.1f; 

	float t = 0.0f; 

	void Update()
	{
//		// transverse
//		for (int i = 0; i < numParticles; i++)
//		{
//			float x = Particles[i].transform.position.x; 
//			// float y = ampl * Mathf.Sin(k * x - omega * t); 
//
//			float y = (ampl * Mathf.Sin(k * x)) * 
//				Mathf.Sin(omega * t); 
//
//			Particles[i].transform.position = 
//				new Vector3(x, y, 0); 
//		}

		// longitudinal
		for (int i = 0; i < numParticles; i++)
		{
			// float x = Particles[i].transform.position.x; 
			float x = initialX[i]; 

			float y = (ampl * Mathf.Sin(k * x)) * 
				Mathf.Sin(omega * t);  

			Particles[i].transform.position = 
				new Vector3(x + y, 0, 0); 
		}

		t += dt; 
	}
}
