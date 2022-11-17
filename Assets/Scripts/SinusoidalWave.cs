using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SinusoidalWave : MonoBehaviour
{
	public ResonanceTube rt; 
	public bool exists; 
	public float t0; 
	public float t; 
	public int direction; 
	public float totDistTrav; 
	public int numReflections; 
	public float distToNextBoundary; 
	public float numWavelengthsInTubeL; 
	public float wavelengthOffset; 
	public float distOffset; 
	private float dt; 

	public float A; 
	public float D; 
	public float k; 
	public float omega; 
	public float v; 
	public float L; 
	public float c;
	public float f; 
	public int numParticles; 
	public float[] xCoord; 

	void Start()
	{
		exists = false; 
		direction = 1; 
		t = 0f; 
		dt = 0.01f; 
	}

	float PosSinWaveDisplacement(float x)
	{
		Debug.Log(omega); 
		float disp; 
		disp = -A * D * Mathf.Sin(k * x - omega * (t - t0)); 
		return disp - (c * disp);  
	}

	float NegSinWaveDisplacement(float x)
	{
		float disp; 
		disp = -A * D * Mathf.Sin(k * x + omega * (t - t0)); 
		return disp - (c * disp);  
	}

	float[] GetParticleDisplacements(float t)
	{
		float[] totalDisp = new float[rt.numParticles]; 
		// Debug.Log(t); 

		A = rt.A; 
		D = rt.D; 
		k = rt.k; 
		omega = rt.omega; 
		f = rt.f; 
		v = rt.v; 
		L = rt.L; 
		c = rt.c; 
		numParticles = rt.numParticles; 
		xCoord = new float[numParticles]; 

		for (int i = 0; i < numParticles; i++) 
			xCoord[i] = rt.particle[i].positionEq.x; 

//		numWavelengthsInTubeL = L / lambda; 
//		wavelengthOffset = numWavelengthsInTubeL - 
//			Mathf.Floor(numWavelengthsInTubeL); 
//		distOffset = wavelengthOffset * lambda; 

		totDistTrav = v * (t - t0); 
		numReflections = (int)Mathf.Floor(totDistTrav / L); 
		distToNextBoundary = (numReflections + 1) * L - totDistTrav; 

		if (numReflections % 2 == 0) 
			direction = 1; 
		else
			direction = -1; 

		// Debug.Log("f " + f + " omega " + omega); 

		// Debug.Log(numReflections); 

		for (int i = 0; i < numParticles; i++) {
			// if (numReflections < 35) {
				for (int n = 0; n < (numReflections + 1); n++) {
					// add negative waves
					if (totDistTrav > (n * L) && 
						(L - xCoord[i]) < (totDistTrav - n * L) &&
						n > 0) {
						totalDisp[i] += 
							NegSinWaveDisplacement(
								xCoord[i] - ((n + 1) * L) 
								); 

//						pressureDisp[i] += 
//							NegPressureWaveDisplacement(
//								xCoord[0, i].x - ((n + 1) * L), 
//								Mathf.Pow(1f, n)
//								); 

					}

					// add positive waves
					if (totDistTrav > (n * L) && 
						xCoord[i] < (totDistTrav - (n * L))) {
						totalDisp[i] += 
							PosSinWaveDisplacement(
								xCoord[i] + n * L
								); 

//						pressureDisp[i] += 
//							PosPressureWaveDisplacement(
//								eqPos[0, i].x + n * L,
//								Mathf.Pow(1f, n)
//								); 

					}
				}
			// }

//			else {
//				for (int n = 0; n < 35; n++) {
//					// add negative waves
//					if (totDistTrav > (n * L) && 
//						(L - xCoord[i]) < 
//						(totDistTrav - n * L) && 
//						n > 0) { 
//						totalDisp[i] += 
//							NegSinWaveDisplacement(
//								xCoord[i] - ((n + 1) * L) 
//								); 
//
////						pressureDisp[i] += 
////							NegPressureWaveDisplacement(
////								eqPos[0, i].x - ((n + 1) * L), 
////								Mathf.Pow(1f, n)
////								); 
//
//					}
//
//					// add positive waves
//					if (totDistTrav > (n * L) && 
//						xCoord[i] < (totDistTrav - (n * L))) {
//						totalDisp[i] += 
//							PosSinWaveDisplacement(
//								xCoord[i] + n * L
//								); 
//
////						pressureDisp[i] += 
////							PosPressureWaveDisplacement(
////								eqPos[0, i].x + n * L, 
////								Mathf.Pow(1f, n)
////								); 
//
//					}
//				}
//
//			}
		}

		float speakerDisp = PosSinWaveDisplacement(0) * 2f; 
		rt.speaker.transform.position = new Vector2(
				rt.speakerPosEq + speakerDisp, 0); 

		return totalDisp; 
	}


	void FixedUpdate()
	{
		if (exists == true)
		{
			float[] totalDisp = new float[rt.numParticles]; 
			totalDisp = GetParticleDisplacements(t); 

			for (int i = 0; i < numParticles; i++) {
				rt.particle[i].displacement.x = totalDisp[i]; 
				rt.particleInstance[i].transform.position = new Vector2(
						rt.particle[i].positionEq.x + totalDisp[i], 
						rt.particle[i].positionEq.y
						); 
			}

			t += dt; 
		}
	}
}
