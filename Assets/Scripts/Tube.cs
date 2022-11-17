using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 

public struct SinusoidalSignal {
	public bool on; 
	public float t0; 
	public float t; 
	public float dt; 
	public float totalDistTrav; 
	public float numReflections; 
	public int direction; 
	public float distToNextBoundary; 
}; 

public class Tube : MonoBehaviour
{
	public GameObject tube; 
	public GameObject speaker; 
	public GameObject stopper; 

	public float L; 
	public float H; 
	public float f; 
	public float fScaled; 
	public float v; 
	public float vScaled; 
	public float lambda; 
	public float omega; 
	public float k; 
	public float A; 
	// public float dt; 
	// public float t; 
	public float D; 

	public int numPartitions; 
	public struct Partition {

	}; 

	public int numSamplingPoints; 
	public float[] xPoints; 
	public float pointSpacingTarget; 
	public float pointSpacing; 
	// public float[] displacement; 

	public GameObject[,] particle; 
	public GameObject particlePrefab; 
	public int numParticles; 
	public int numRows; 

	public SinusoidalSignal signal; 

	public bool randomParticles;  
	public bool tubeOpen; 

	public float timedamper; 
	public float scalingFactor; 

	public Vector3 speakerEqPos; 
	public Vector3 stopperEqPos; 

	public LineRenderer displacementLine; 

	public Text lengthDisplay; 
	public Text frequencyDisplay; 
	public Text wavelengthDisplay; 
	public Text speedDisplay; 
	public Text numReflectionsDisplay; 

	public void SetSamplingPoints()
	{
		numSamplingPoints = (int)Mathf.Ceil(L / pointSpacingTarget); 
		pointSpacing = L / (numSamplingPoints - 1); 
		xPoints = new float[numSamplingPoints]; 

		for (int i = 0; i < numSamplingPoints; i++) 
			xPoints[i] = pointSpacing * i; 

		// displacement = new float[numSamplingPoints]; 
		displacementLine.positionCount = numSamplingPoints; 
	}

	public void SetParticlePositions()
	{
		if (randomParticles == true) {
			Debug.Log("oops! need to write script for randoms"); 
		}	

		else {
			numParticles = numSamplingPoints; 
			float effectiveH = H - 2 * pointSpacingTarget; 
			numRows = (int)Mathf.Floor(effectiveH / 
					pointSpacingTarget); 
			float vertSpacing = effectiveH / (numRows - 1); 

			particle = new GameObject[numRows, numSamplingPoints]; 

			for (int j = 0; j < numRows; j++) {
				for (int i = 0; i < numSamplingPoints; i++) {
					particle[j, i] = 
						GameObject.Instantiate(
						particlePrefab) as GameObject;

					particle[j, i].transform.position = 
						new Vector2(
							xPoints[i], 
							(pointSpacingTarget - 
							H / 2) + j *
							vertSpacing
							); 

					// displacement[i] = 0f; 
				}
			}
		}
	}

	void Start()
	{
		f = .340f; 
		v = .340f;
		lambda = v / f; 
		A = .05f; 
		omega = 2 * Mathf.PI * f; 
		L = 2.0f; 
		H = .6f; 
		k = 2 * Mathf.PI / lambda; 
		pointSpacingTarget = .05f; 
		timedamper = 1f; 
		scalingFactor = 1000f; 
		D = 0f; 

		fScaled = f * scalingFactor; 
		vScaled = v * scalingFactor;

		randomParticles = false; 
		tubeOpen = false; 

		speakerEqPos = new Vector3(0f, 0f, -.1f); 
		stopperEqPos = new Vector3(L, 0f, -.1f); 

		signal.on = false; 

		SetSamplingPoints(); 
		SetParticlePositions(); 

		speaker.transform.position = speakerEqPos; 
		stopper.transform.position = stopperEqPos; 
	}

	public void InitializeSinusoidalSignal()
	{
		signal.on = true; 
		signal.dt = .01f; 
	}

	public void PauseSinusoidalSignal()
	{
		signal.on = false; 
	}

	public void StopSinusoidalSignal()
	{
		signal.on = false; 
		signal.t = 0f; 
		signal.totalDistTrav = 0f; 
		signal.numReflections = 0; 
		for (int j = 0; j < numRows; j++) {
			for (int i = 0; i < numSamplingPoints; i++) {
				Destroy(particle[j, i]); 
			}
		}
		SetParticlePositions(); 
	}

	float GetPosSignalDisplacement(float x, float t, int dir)
	{
		float disp; 
		disp = -A * Mathf.Sin(k * x - (omega * t)); 
		return disp - (D * disp); 
	}

	float GetNegSignalDisplacement(float x, float t, int dir)
	{
		float disp; 
		// disp = -A * Mathf.Sin(k * x + (omega * t)); 
		// return disp - (D * disp); 

		disp = A * Mathf.Sin(k * x + (omega * t)); 
		return disp; 
	}

	public void RunSinusoidalSignal()
	{
		signal.totalDistTrav = v * (signal.t - signal.t0); 
		signal.numReflections = (int)Mathf.Floor(
				signal.totalDistTrav / L); 
		if (signal.numReflections %2 == 0)
			signal.direction = 1; 
		else
			signal.direction = -1; 

		float[] displacement = new float[numSamplingPoints]; 

		for (int i = 0; i < numSamplingPoints; i++) {
			// for (int n = 0; n < (signal.numReflections + 1); n++) {

				// draw static waves
				// positive
				displacement[i] += -GetPosSignalDisplacement(
					-xPoints[i], 
					0f,
					signal.direction
					); 

//				// positive waves
//				if (xPoints[i] < signal.totalDistTrav) {
//					displacement[i] += GetPosSignalDisplacement(
//						xPoints[i], 
//						signal.t - signal.t0, 
//						signal.direction
//						); 
//				}

//				// negative waves
//				if (signal.totalDistTrav > L &&
//					(L - xPoints[i]) < (signal.totalDistTrav - L)) 
//				{
//					displacement[i] += -1 * GetPosSignalDisplacement(
//						-xPoints[i] - L, 
//						signal.t - signal.t0, 
//						signal.direction
//						); 
//				}


//				if (signal.totalDistTrav > L && 
//					(L - xPoints[i]) < (signal.totalDistTrav - L)) 
//				{
//					displacement[i] += 
//						-1 * GetPosSignalDisplacement(
//							xPoints[i] - L, 
//							signal.t - signal.t0, 
//							signal.direction
//							); 
//				}

//				// add negative waves
//				if (signal.totalDistTrav > (n * L) && 
//					(L - xPoints[i]) < 
//					(signal.totalDistTrav - n * L) &&
//					n > 0) {
//					displacement[i] += 
//						GetNegSignalDisplacement(
//						xPoints[i] - ((n + 1) * L), 
//						signal.t - signal.t0, 
//						signal.direction
//						); 
//				}

//				// add positive waves
//				if (signal.totalDistTrav > (n * L) && 
//					xPoints[i] < (signal.totalDistTrav - 
//						(n * L))) {
//					displacement[i] += 
//						GetPosSignalDisplacement(
//						xPoints[i] + n * L,
//						signal.t - signal.t0, 
//						signal.direction
//						); 
//				}

			//}

//			if (xPoints[i] < signal.totalDistTrav) {
//				displacement[i] += GetSignalDisplacement(
//					xPoints[i], 
//					signal.t - signal.t0, 
//					signal.direction
//					); 
//			}
//
//			if (signal.totalDistTrav > L && 
//				(L - xPoints[i]) < 
//				(signal.totalDistTrav - L)) {
//				displacement[i] += GetSignalDisplacement(
//					xPoints[i] - 2 * L, 
//					signal.t - signal.t0, 
//					signal.direction
//					); 
//			}

		}

		for (int j = 0; j < numRows; j++) {
			for (int i = 0; i < numSamplingPoints; i++) {
				particle[j, i].transform.position = 
					new Vector2(
						xPoints[i] + displacement[i], 
						particle[j, i].
						transform.position.y
						); 
			}
		}

		for (int i = 0; i < numSamplingPoints; i++) {
			displacementLine.SetPosition(i, new Vector2(
					xPoints[i], 
					-1f + displacement[i] * 5f 
					)); 
		}

		signal.t += signal.dt * timedamper; 
	}

	void UpdateDisplays()
	{
		lengthDisplay.text = L.ToString("f3") + " m"; 
		frequencyDisplay.text = fScaled.ToString("f3") + " Hz"; 
		wavelengthDisplay.text = lambda.ToString("f3") + " m"; 
		speedDisplay.text = vScaled.ToString("f3") + " m/s"; 
		numReflectionsDisplay.text = "Reflections " + 
			signal.numReflections; 
	}

	void Update()
	{
		if (signal.on == true) 
			RunSinusoidalSignal(); 

		UpdateDisplays(); 
	}
}
