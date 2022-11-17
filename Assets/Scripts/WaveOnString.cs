using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public struct GaussianPulse_tmp {
	// public bool exists; 
	public float startTime; 
	public int direction;  
	// public int numCollisionsWithBoundary; 
	public float totDistTrav; 
	public int numLengths; 
	public float distToBoundary; 
}; 

public struct SinusoidalPulse_tmp {
	public float startTime; 
	public int direction;  
	public float totDistTrav; 
	public int numLengths; 
	public float distToBoundary; 
}; 

public class WaveOnString : MonoBehaviour
{
	const int numParticles = 60; 
	public GameObject particlePrefab; 
	public GameObject prodder; 
	private GameObject[] Particles = new GameObject[numParticles];

	Vector2 prodderEqPos;  

	const float f = 0.11f; 
	const float v = 0.14f; 
	const float lambda = v / f; 
	const float A = 0.1f;
	const float sigma = 0.2f; 
	const float k = 2 * Mathf.PI / lambda; 
	const float omega = 2 * Mathf.PI * f; 
	public const float L = 2.0f; 
	// float boundary0 = 0.0f; 
	// float boundary1 = L; 
	float particleSpacing = L / (numParticles - 1); 
	// float delay = 0.0f; 

	const float dt = 0.05f; 
	float t; 

	Vector2[] eqPos = new Vector2[numParticles]; 
	Vector2[] pulseDisp = new Vector2[numParticles]; 

	// pulse
	public Button sendPulseButton; 
	public Button resetStringButton; 

	public Dropdown waveformDropdown; 
	List<string> waveDropdownOptions = new List<string> { "pulse", "sinusoidal" }; 
	int waveformType; 

	const int maxNumPulses = 10; 
	GaussianPulse_tmp[] pulses = new GaussianPulse_tmp[maxNumPulses]; 
	int numPulses; 

	// sinusoidal 
	bool sinuPulseExists; 
	float sinuPulseStartTime; 
	float sinuPulseTotDistTrav; 
	int sinuPulseNumReflections; 
	float sinuPulseDistToBoundary; 
	const int maxNumSinuPulses = 6; 
	SinusoidalPulse_tmp[] sspulse = new SinusoidalPulse_tmp[maxNumSinuPulses]; 
	public Button startSinuWaveButton; 
	int sinuPulseDirection; 

	void Start()
	{
		for (int i = 0; i < numParticles; i++) {
			eqPos[i] = new Vector2(particleSpacing * i, 0); 
			pulseDisp[i] = new Vector2(0, 0); 
		}

		for (int i = 0; i < numParticles; i++) {
			Particles[i] = GameObject.Instantiate(particlePrefab) 
				as GameObject; 
			Particles[i].transform.position = 
				new Vector2(eqPos[i].x + pulseDisp[i].x, 
					eqPos[i].y + pulseDisp[i].y); 
		}

		numPulses = 0; 
		pulses[0].startTime = 0.0f; 

		prodderEqPos = new Vector2(0.0f, -0.126f); 
		prodder.transform.position = prodderEqPos; 

		waveformDropdown.ClearOptions(); 
		waveformDropdown.AddOptions(waveDropdownOptions); 

		waveformType = 0; 

		sspulse[0].startTime = 0.0f; 
		sinuPulseExists = false; 
		sinuPulseStartTime = 0.0f; 
		sinuPulseTotDistTrav = 0.0f; 
		sinuPulseNumReflections = 0; 
		sinuPulseDistToBoundary = 0.0f; 
		sinuPulseDirection = 1; 

		t = 0.0f; 
	}

	float PosGaussianPulse_tmpDisplacement(float x, float t, float x0, float t0)
	{
		float disp = A * Mathf.Exp(-1 * (Mathf.Pow(x - x0 - 
			v * (t - t0), 2)) / (2 * sigma * sigma)); 
		return disp; 
	}

	float NegGaussianPulse_tmpDisplacement(float x, float t, float x0, float t0)
	{
		float disp = -A * Mathf.Exp(-1 * (Mathf.Pow(x - x0 + 
			v * (t - t0), 2)) / (2 * sigma * sigma)); 
		return disp; 
	}

	void CreateGaussianPulse_tmp(float currTime) 
	{
		// Debug.Log("You clicked at time " + t); 
		if (t != pulses[numPulses].startTime) 
		{
			numPulses++;  
			// pulses[numPulses].exists = true; 
			pulses[numPulses].startTime = currTime; 
		}
		// Debug.Log("pulse # " + numPulses + "; t = " + currTime + "; pulse starttime = " + pulses[numPulses].startTime); 
	}

	void ResetString()
	{
		numPulses = 0; 
		for (int i = 1; i < maxNumPulses; i++)
		{
			pulses[i].startTime = 0.0f; 
			pulses[i].direction = 1; 
			pulses[i].totDistTrav = 0.0f; 
			pulses[i].numLengths = 0; 
			pulses[i].distToBoundary = 0.0f; 
		}

		for (int i = 1; i < maxNumSinuPulses; i++)
		{
			sspulse[i].startTime = 0.0f; 
			sspulse[i].direction = 1; 
			sspulse[i].totDistTrav = 0.0f; 
			sspulse[i].numLengths = 0; 
			sspulse[i].distToBoundary = 0.0f; 
		}

		sinuPulseExists = false; 
		sinuPulseStartTime = 0.0f; 
		sinuPulseTotDistTrav = 0.0f; 
	}

	void ChangeWaveformType(int value)
	{
		waveformType = value; 
		ResetString(); 
		Debug.Log(waveformType); 
	}

	int RoundToLowestMultiple(int x, int multiple)
	{
		return x - x % multiple; 
	}

	float[] RunPulseDemo()
	{
		float[] totalDisp = new float[numParticles]; 

		sendPulseButton.onClick.AddListener(
			delegate {CreateGaussianPulse_tmp(t); }); 

		if (numPulses > 0) 
		{
			for (int k = 1; k < numPulses + 1; k++)
			{
				// compute displacement due to pulse[i]
				for (int i = 0; i < numParticles; i++)
				{
					int ncl = RoundToLowestMultiple(pulses[k].numLengths, 2); 
					// Debug.Log(ncl); 

					// add positive moving waves
					float startpoint = -2 * ncl;  
					float increment = 2 * L; 
					for (int j = 0; j < 5; j++)
					{
						totalDisp[i] += PosGaussianPulse_tmpDisplacement(
							eqPos[i].x, 
							t - pulses[k].startTime, 
							startpoint - increment * j, 
							0); 
					}

					// add negative moving waves
					startpoint = 2 * ncl; 
					increment = 2 * L; 

					if (pulses[k].numLengths < 1) 
					{
						for (int j = 1; j < 6; j++)
						{
							totalDisp[i] += NegGaussianPulse_tmpDisplacement(
								eqPos[i].x, 
								t - pulses[k].startTime, 
								startpoint + increment * j, 
								0); 
						}
					}
					else 
					{
						for (int j = 0; j < 5; j++)
						{
							totalDisp[i] += NegGaussianPulse_tmpDisplacement(
								eqPos[i].x, 
								t - pulses[k].startTime, 
								startpoint + increment * j, 
								0); 
						}
					}

				}

				pulses[k].totDistTrav += v * dt; 
				pulses[k].numLengths = (int)Mathf.Floor(pulses[k].totDistTrav / L); 
				pulses[k].distToBoundary = (pulses[k].numLengths + 1) * L - 
					pulses[k].totDistTrav; 

				if (pulses[k].numLengths % 2 == 0)
					pulses[k].direction = 1; 
				else
					pulses[k].direction = -1; 
			}
		}

		return totalDisp; 
	}

	void CreateSinusoidalWave(float currTime)
	{
		ResetString(); 
		Debug.Log("You clicked at time " + t); 
		if (t != sinuPulseStartTime)
		{
			sinuPulseStartTime = currTime; 
			sinuPulseExists = true; 
		}
		Debug.Log("t " + currTime + "starttime " + sinuPulseStartTime); 
	}

	float PosSinuWaveDisplacement(float x)
	{
		float disp = -A * Mathf.Sin(k * x  - omega * (t - sinuPulseStartTime)); 
		return disp; 
	}

	float NegSinuWaveDisplacement(float x)
	{
		float numWavelengthsTrav = L / lambda; 
		float wavelengthOffset = numWavelengthsTrav - Mathf.Floor(numWavelengthsTrav);  
		float distOffset = wavelengthOffset * lambda;  

//		Debug.Log("num wavelengths trav = " + numWavelengthsTrav + 
//				" wavelength offset = " + wavelengthOffset + 
//				" distoffset = " + distOffset); 

		// float disp = A * Mathf.Sin(k * (x) + omega * (t - sinuPulseStartTime) - 0.5f * Mathf.PI - distOffset);  
		float disp = -A * Mathf.Sin(k * (x - distOffset - L) + omega * (t - sinuPulseStartTime)); 

		return disp; 
	}

	float[] RunContinuousSinusoidalDemo(float t)
	{
		float[] totalDisp = new float[numParticles]; 

		startSinuWaveButton.onClick.AddListener(
			delegate {CreateSinusoidalWave(t); }); 

		if (sinuPulseExists == true) 
		{
			// Debug.Log("sinu pulse exists start time" + sinuPulseStartTime); 

			sinuPulseTotDistTrav = v * (t - sinuPulseStartTime); 
			// Debug.Log(sinuPulseTotDistTrav); 

			sinuPulseNumReflections = (int)Mathf.Floor(sinuPulseTotDistTrav / L); 
			sinuPulseDistToBoundary = (sinuPulseNumReflections + 1) * L - 
				sinuPulseTotDistTrav; 

			// int ncl = RoundToLowestMultiple(sinuPulseNumReflections, 2); 

			int n = sinuPulseNumReflections; 

			if (sinuPulseNumReflections % 2 == 0)
				sinuPulseDirection = 1; 
			else
				sinuPulseDirection = -1; 

			// add positive moving waves
			for (int i = 0; i < numParticles; i++)
			{
				if (eqPos[i].x < sinuPulseTotDistTrav)
				{
					totalDisp[i] += PosSinuWaveDisplacement(eqPos[i].x); 
				}

				if (sinuPulseTotDistTrav > L && 
					(L - eqPos[i].x) < (sinuPulseTotDistTrav - L))
				{
					// Debug.Log("negative moving wave should be here"); 
					totalDisp[i] += NegSinuWaveDisplacement(eqPos[i].x); 
				}

				if (sinuPulseTotDistTrav > (2 * L) && 
					eqPos[i].x < (sinuPulseTotDistTrav - (2 * L)))
				{
					// Debug.Log("negative moving wave should be here"); 
					totalDisp[i] += PosSinuWaveDisplacement(eqPos[i].x); 
				}

				if (sinuPulseTotDistTrav > (3 * L) && 
					(L - eqPos[i].x) < (sinuPulseTotDistTrav - 3 * L))
				{
					// Debug.Log("negative moving wave should be here"); 
					totalDisp[i] += NegSinuWaveDisplacement(eqPos[i].x); 
				}
			}
		}
		
		Debug.Log(sinuPulseNumReflections); 

		return totalDisp; 
	}

	float[] RunSinusoidalWiggleDemo(float t)
	{
		float[] totalDisp = new float[numParticles]; 

		startSinuWaveButton.onClick.AddListener(
			delegate {CreateSinusoidalWave(t); }); 

		if (sinuPulseExists == true) 
		{
			// Debug.Log("sinu pulse exists start time" + sinuPulseStartTime); 

			sinuPulseTotDistTrav = v * (t - sinuPulseStartTime); 
			// Debug.Log(sinuPulseTotDistTrav); 

			sinuPulseNumReflections = (int)Mathf.Floor(sinuPulseTotDistTrav / L); 
			sinuPulseDistToBoundary = (sinuPulseNumReflections + 1) * L - 
				sinuPulseTotDistTrav; 

			// int ncl = RoundToLowestMultiple(sinuPulseNumReflections, 2); 

			int n = sinuPulseNumReflections; 

			// add positive moving waves
			for (int i = 0; i < numParticles; i++)
			{
				if (eqPos[i].x < sinuPulseTotDistTrav)
				{
					totalDisp[i] += PosSinuWaveDisplacement(eqPos[i].x); 
				}

				if (sinuPulseTotDistTrav > L && 
					(L - eqPos[i].x) < (sinuPulseTotDistTrav - L))
				{
					// Debug.Log("negative moving wave should be here"); 
					totalDisp[i] += NegSinuWaveDisplacement(eqPos[i].x); 
				}

				if (sinuPulseTotDistTrav > (2 * L) && 
					eqPos[i].x < (sinuPulseTotDistTrav - (2 * L)))
				{
					// Debug.Log("negative moving wave should be here"); 
					totalDisp[i] += PosSinuWaveDisplacement(eqPos[i].x); 
				}

//				if (sinuPulseTotDistTrav > (3 * L) && 
//					(L - eqPos[i].x) < (sinuPulseTotDistTrav - (3 * L)))
//				{
//					// Debug.Log("negative moving wave should be here"); 
//					totalDisp[i] += NegSinuWaveDisplacement(eqPos[i].x); 
//				}
			}
		}

		if (sinuPulseNumReflections % 2 == 0)
			sinuPulseDirection = 1; 
		else
			sinuPulseDirection = -1; 

		
		Debug.Log(sinuPulseNumReflections); 

		return totalDisp; 
	}

	void FixedUpdate()
	{
		resetStringButton.onClick.AddListener(ResetString); 

		waveformDropdown.onValueChanged.AddListener(
			delegate {ChangeWaveformType(waveformDropdown.value); }); 

		float[] totalDisp = new float[numParticles]; 

		if (waveformType == 0)
			totalDisp = RunPulseDemo(); 
		else if (waveformType == 1)
			totalDisp = RunContinuousSinusoidalDemo(t); 
			totalDisp = RunSinusoidalWiggleDemo(t); 

		// update accumulated displacements
		for (int i = 0; i < numParticles; i++)
		{
			pulseDisp[i].y = totalDisp[i]; 
		}

		// move particles to new position
		for (int i = 0; i < numParticles; i++)
		{
			Particles[i].transform.position = new Vector2(
				eqPos[i].x + pulseDisp[i].x, 
				eqPos[i].y + pulseDisp[i].y); 

			prodder.transform.position = new Vector2(
				prodderEqPos.x, prodderEqPos.y + 
				pulseDisp[0].y); 
		}

		t += dt; 
	}
}
