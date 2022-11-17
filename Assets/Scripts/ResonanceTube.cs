using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems; 

public struct GaussianPulse {
	// public bool exists; 
	public float startTime; 
	public int direction;  
	// public int numCollisionsWithBoundary; 
	public float totDistTrav; 
	public int numReflections; 
	public float distToBoundary; 
}; 

//public struct SinusoidalWave {
//	public bool exists; 
//	public float t0; 
//	public int direction; 
//	public float totDistTrav; 
//	public int numReflections; 
//	public float distToBoundary; 
//	public float numWavelengthsInTubeL; 
//	public float wavelengthOffset; 
//	public float distOffset; 
//}; 

public struct Particle {
	public Vector2 positionEq; 
	public Vector2 position; 
	public Vector2 displacement; 
	public Vector2 velocity; 
	public Vector2 positionPrev; 
	public Vector2 velocityPrev; 
	public Vector2 meanFreePath; 
};

public struct TubePartition {
	public float x0_eq;
	public float x1_eq; 
	public float CoM_eq; 
	public float x0; 
	public float x1; 
	public float pressure; 
	public float density; 
}; 

public class ResonanceTube : MonoBehaviour
{
	public GameObject particlePrefab; 
	public GameObject[,] particles; 
	public GameObject stopper; 
	public GameObject speaker; 

//	public GameObject gridlinePrefab; 
//	public GameObject[] xGridLines; 
//	public GameObject[] yGridLines; 

//	public int numParticles; 
//	public int numRows;  
//	public float particleHorizSpacing_target;  
//	public float particleHorizSpacing; 
//	public int numXGridLines; 
//	public int numYGridLines; 

	public float f; 
	public float v; 
	public float lambda; 
	public float A; 
	public float sigma; 
	public float k; 
	public float omega; 
	public float L; 
	public float H; 
	public float D; 
	public float c; 
	float t; 
	const float dt = 0.05f; 

//	Vector2[,] eqPos; 
//	Vector2[,] pulseDisp; 

	public float speakerPosEq; 
	public float stopperPosEq; 

	// UI
//	public Button startButton; 
//	public Button resetButton; 

	public Slider lengthSlider; 
	public Slider frequencySlider; 

	public Text frequencyText; 
	public Text lambdaText; 
	public Text lengthText; 
	public Text speedText; 

	// graphing
	public LineRenderer displacementLine;  
	public LineRenderer pressureLine;  

	// new stuff
	public int numPartitions; 
	public float partitionSpacingTarget; 
	public float partitionSpacing; 
	public TubePartition[] partition; 

	public int numParticles; 
	public float particleSpacingTarget; 
	public float particleHorizSpacing; 
	public float particleVertSpacing; 
	public int numRows; 
	public Particle[] particle; 
	public GameObject[] particleInstance; 
	public bool randomParticles; 
	public bool tubeClosed; 
	// public float[] particleDisp; 
	// public float[] pressureDisp; 
	public SinusoidalWave wave; 

	public void InitializeEquilibriumState(int numParticles, 
		int numRows, float particleHorizSpacing)
	{
//		particles = new GameObject[numRows, numParticles]; 
//		eqPos = new Vector2[numRows, numParticles]; 
//		pulseDisp = new Vector2[numRows, numParticles]; 
//
//		pressureDisp = new float[numParticles]; 
//
////		displacementLine.positionCount = numParticles; 
////		pressureLine.positionCount = numParticles; 
//
//		for (int j = 0; j < numRows; j++) {
//			for (int i = 0; i < numParticles; i++) {
//				eqPos[j, i] = new Vector2(
//					particleHorizSpacing * i, 
//					-0.20f + 0.1f * j
//					); 
//				pulseDisp[j, i] = new Vector2(0, 0); 
//			}
//		}
//
//		for (int j = 0; j < numRows; j++) {
//			for (int i = 0; i < numParticles; i++) {
//				particles[j, i] = 
//					GameObject.Instantiate(
//					particlePrefab) as GameObject; 
//				particles[j, i].transform.position = 
//					new Vector2(
//						eqPos[j, i].x + 
//						pulseDisp[j, i].x, 
//						eqPos[j, i].y + 
//						pulseDisp[j, i].y
//						); 
//			}
//		}
	}

	public void SetSystemToEquilibrium()
	{
		partition = new TubePartition[numPartitions]; 
		
		for (int i = 0; i < numPartitions; i++) {
			partition[i].x0_eq = partitionSpacing * i; 
			partition[i].x1_eq = partitionSpacing * (i + 1); 
		}

		if (randomParticles == true) {
			Debug.Log("need to add randomiser"); 
		}

		else {
			int numParticlesPerRow = (int)Mathf.Ceil(L / 
					particleSpacingTarget); 
			particleHorizSpacing = L / (numParticlesPerRow - 1); 
			float effectiveH = H - particleSpacingTarget; 
			numRows = (int)Mathf.Floor(effectiveH / 
					particleSpacingTarget); 
			particleVertSpacing = effectiveH / (numRows - 1); 
			numParticles = numRows * numParticlesPerRow; 

			particle = new Particle[numParticles]; 
			particleInstance = new GameObject[numParticles]; 

			int k = 0; 
			for (int i = 0; i < numRows; i++) {
				for (int j = 0; j < numParticlesPerRow; j++) {
					particle[k].positionEq = new Vector2(
						0f + j * particleHorizSpacing, 
						(0.5f * particleSpacingTarget - 
						 H / 2) + i * 
						particleVertSpacing
						); 
					k++; 
				}
			}

			for (int i = 0; i < numParticles; i++) {
				particleInstance[i] = GameObject.Instantiate(
					particlePrefab) as GameObject; 
				particleInstance[i].transform.position = 
					particle[i].positionEq; 
			}
		}
	}

	void Start()
	{
		// defaults
		f = 3.40f;  
		v = 3.40f; 
		lambda = v / f; 
		A = 0.001f; 
		sigma = 0.2f; 
		k = 2 * Mathf.PI / lambda; 
		omega = 2 * Mathf.PI * f; 
		L = 2.00f; 
		H = 0.6f; 
		D = 1f; 
		c = 0.6f; 
		partitionSpacingTarget = 0.1f; 
		particleSpacingTarget = 0.1f; 

		numPartitions = (int)Mathf.Ceil(L / partitionSpacingTarget); 
		partitionSpacing = L / numPartitions; 
		randomParticles = false; 

		speakerPosEq = -.19f; 
		stopperPosEq = 0f; 

		SetSystemToEquilibrium(); 

		displacementLine.positionCount = numParticles; 
		for (int i = 0; i < numParticles; i++) {
			displacementLine.SetPosition(i, new Vector2(
						particle[i].positionEq.x, 
						-1f + 0f)); 
		}

		stopper.transform.position = new Vector3(stopperPosEq + L, 0f, -.1f); 


//		displacementLine.positionCount = numPartitions + 1; 
//
//		for (int i = 0; i < numPartitions; i++) {
//			displacementLine.SetPosition(i, 
//				new Vector2(partition[i].x0_eq, 0f)); 
//		}
//		displacementLine.SetPosition(numPartitions + 1, 
//				new Vector2(partition[numPartitions].x1_eq, 0f)); 
//		particleHorizSpacing_target = 0.05f; 


//		numParticles = (int)Mathf.Ceil(L / 
//			particleHorizSpacing_target); 
//		particleHorizSpacing = L / (numParticles - 1); 
//		numRows = 5; 
//
//		molecularSpacingTarget = 0.02f; 
//		numAirMolecules = (int)Mathf.Ceil(L / molecularSpacingTarget); 
//
//		InitializeEquilibriumState(
//			numParticles, 
//			numRows,
//			particleHorizSpacing
//			); 

		t = 0.0f; 
	}

	int RoundToLowestMultiple(int x, int multiple)
	{
		return x - x % multiple; 
	}

	public void ResetParticles()
	{
		wave.exists = false; 
		wave.t0 = 0.0f; 
		wave.t = 0.0f; 
		wave.totDistTrav = 0.0f; 
		wave.numReflections = 0; 
		wave.distToNextBoundary = 0.0f; 
//		for (int i = 0; i < numParticles; i++) 
//			pressureDisp[i] = 0; 
		speaker.transform.position = new Vector3(speakerPosEq, 0, -.1f); 
		Debug.Log("speaker off"); 
	}

	public void CreateSinusoidalWave()
	{
		wave.exists = true; 
		wave.t0 = 0f; 
		wave.numWavelengthsInTubeL = L / lambda; 
		wave.wavelengthOffset = wave.numWavelengthsInTubeL - 
			Mathf.Floor(wave.numWavelengthsInTubeL); 
		wave.distOffset = wave.wavelengthOffset * lambda; 
	}

//	float PosSinWaveDisplacement(float x) 
//	{
//		float disp; 
//		disp = -A * D * Mathf.Sin(k * x - omega * (t - 
//			wave.startTime)); 
//		return disp - (c * disp);  
//	}
//
//	float NegSinWaveDisplacement(float x) 
//	{
//		float disp; 
//		disp = -A * D * Mathf.Sin(k * x + omega * (t - 
//			wave.startTime)); 
//		return disp - (c * disp);  
//	}

//	float PosPressureWaveDisplacement(float x, float damp) 
//	{
//		float disp; 
//		disp = A * D * Mathf.Cos(k * x - omega * (t - 
//			wave.startTime)); 
//		return disp - (c * disp);  
//	}
//
//	float NegPressureWaveDisplacement(float x, float damp) 
//	{
//		float disp; 
//		disp = A * D * Mathf.Cos(k * x + omega * (t - 
//			wave.startTime)); 
//		return disp - (c * disp);  
//	}

//	float[] RunSinusoidalDemo(float t)
//	{
//		float[] totalDisp = new float[numParticles]; 
//		// Debug.Log(f); 
//
//		startButton.onClick.AddListener(
//			delegate {CreateSinusoidalWave(t); }); 
//
//		wave.totDistTrav = v * (t - wave.startTime); 
//		wave.numReflections = (int)Mathf.Floor(
//				wave.totDistTrav / L); 
//		wave.distToBoundary = (wave.numReflections + 1) * L - 
//			wave.totDistTrav; 
//
//		if (wave.numReflections % 2 == 0) 
//			wave.direction = 1; 
//		else
//			wave.direction = -1; 
//
//		Debug.Log(wave.numReflections); 
//
//		D = 1f; 
//		c = .5f; 
//
//		if (wave.exists == true) {
//			for (int i = 0; i < numParticles; i++) {
//				if (wave.numReflections < 35) {
//					for (int n = 0; n < (wave.numReflections + 1); n++) {
//						// add negative waves
//						if (wave.totDistTrav > (n * L) && 
//							(L - eqPos[0, i].x) < 
//							(wave.totDistTrav - n * L) &&
//							n > 0) {
//							totalDisp[i] += 
//								NegSinWaveDisplacement(
//									eqPos[0, i].x - ((n + 1) * L), 
//									Mathf.Pow(1f, n)
//									); 
//							pressureDisp[i] += 
//								NegPressureWaveDisplacement(
//									eqPos[0, i].x - ((n + 1) * L), 
//									Mathf.Pow(1f, n)
//									); 
//						}
//
//						// add positive waves
//						if (wave.totDistTrav > (n * L) && 
//							eqPos[0, i].x < (wave.totDistTrav - (n * L))) {
//							totalDisp[i] += 
//								PosSinWaveDisplacement(
//									eqPos[0, i].x + n * L,
//									Mathf.Pow(1f, n)
//									); 
//							pressureDisp[i] += 
//								PosPressureWaveDisplacement(
//									eqPos[0, i].x + n * L,
//									Mathf.Pow(1f, n)
//									); 
//						}
//					}
//				}
//
//				else {
//					for (int n = 0; n < 35; n++) {
//						// add negative waves
//						if (wave.totDistTrav > (n * L) && 
//							(L - eqPos[0, i].x) < 
//							(wave.totDistTrav - n * L) && 
//							n > 0) { 
//							totalDisp[i] += 
//								NegSinWaveDisplacement(
//									eqPos[0, i].x - ((n + 1) * L), 
//									Mathf.Pow(1f, n)
//									); 
//							pressureDisp[i] += 
//								NegPressureWaveDisplacement(
//									eqPos[0, i].x - ((n + 1) * L), 
//									Mathf.Pow(1f, n)
//									); 
//						}
//
//						// add positive waves
//						if (wave.totDistTrav > (n * L) && 
//							eqPos[0, i].x < (wave.totDistTrav - (n * L))) {
//							totalDisp[i] += 
//								PosSinWaveDisplacement(
//									eqPos[0, i].x + n * L,
//									Mathf.Pow(1f, n)
//									); 
//							pressureDisp[i] += 
//								PosPressureWaveDisplacement(
//									eqPos[0, i].x + n * L, 
//									Mathf.Pow(1f, n)
//									); 
//						}
//					}
//				}
//
//
//
//			}
//
//			float speakerDisp = PosSinWaveDisplacement(0, 1) * 2f; 
//			speaker.transform.position = new Vector2(
//					speakerPosEq + speakerDisp, 0); 
//		}
//
//		return totalDisp; 
//	}

	void FixedUpdate()
	{

		float inputL = lengthSlider.value; 
		if (inputL != L) {
			for (int i = 0; i < numParticles; i++) {
			    Destroy(particleInstance[i]); 
			}
			int numParticlesPerRow = (int)Mathf.Ceil(inputL / 
					particleSpacingTarget); 
			particleHorizSpacing = inputL / (numParticlesPerRow - 1); 
			numParticles = numRows * numParticlesPerRow; 

			particle = new Particle[numParticles]; 
			particleInstance = new GameObject[numParticles]; 

			int k = 0; 
			for (int i = 0; i < numRows; i++) {
				for (int j = 0; j < numParticlesPerRow; j++) {
					particle[k].positionEq = new Vector2(
						0f + j * particleHorizSpacing, 
						(0.5f * particleSpacingTarget - 
						 H / 2) + i * 
						particleVertSpacing
						); 
					k++; 
				}
			}

			for (int i = 0; i < numParticles; i++) {
				particleInstance[i] = GameObject.Instantiate(
					particlePrefab) as GameObject; 
				particleInstance[i].transform.position = 
					particle[i].positionEq; 
			}

			stopper.transform.position = new Vector3(stopperPosEq + inputL, 0f, -0.1f); 
			L = inputL; 
		}

//		float inputF = frequencySlider.value / 100; 
//		if (inputF != f) {
//			f = inputF; 
//			omega = 2 * Mathf.PI * f; 
//			lambda = v / f; 
//		}

		for (int i = 0; i < numParticles; i++) {
			particle[i].position = particle[i].positionEq + 
				particle[i].displacement; 
			particleInstance[i].transform.position = 
				particle[i].position; 
		}

		float fScaled = f * 100; 
		float vScaled = v * 100; 

		frequencyText.text = fScaled.ToString("f2") + " Hz"; 
		lambdaText.text = lambda.ToString("f2") + " m"; 
		lengthText.text = L.ToString("f2") + " m"; 
		speedText.text = vScaled.ToString("f2") + " m"; 


//		resetButton.onClick.AddListener(ResetParticles); 
//
//		// float lengthInput = lengthInputField.text.ToFloat(); 
//
//
////		waveformDropdown.onValueChanged.AddListener(
////			delegate {ChangeWaveformType(waveformDropdown.value); }); 
//
//		float[] totalDisp = new float[numParticles]; 
//
//		float inputL = lengthSlider.value; 
//		if (inputL != L) {
//			for (int j = 0; j < numRows; j++) {
//				for (int i = 0; i < numParticles; i++) {
//				    Destroy(particles[j, i]); 
//				}
//			}
//
//			numParticles = (int)Mathf.Ceil(inputL / 
//				particleHorizSpacing_target); 
//			particleHorizSpacing = inputL / (numParticles - 1); 
//			numRows = 5; 
//
//			InitializeEquilibriumState(numParticles, 
//				numRows, particleHorizSpacing); 
//
//			stopper.transform.position = new Vector3(inputL, 0f, -0.1f); 
//
//			L = inputL; 
//		}
//
//		float inputF = frequencySlider.value; 
//		if ((inputF / 100) != f) {
//			f = inputF / 100; 
//		}
//
//		// totalDisp = RunGaussianPulseDemo(); 
//		totalDisp = RunSinusoidalDemo(t); 
//
//		for (int j = 0; j < numRows; j++) {
//			for (int i = 0; i < numParticles; i++) {
//				pulseDisp[j, i].x = totalDisp[i]; 
//			}
//		}
//
//		for (int j = 0; j < numRows; j++) {
//			for (int i = 0; i < numParticles; i++) {
//				particles[j, i].transform.position = 
//					new Vector2(
//					eqPos[j, i].x + pulseDisp[j, i].x,
//					eqPos[j, i].y + pulseDisp[j, i].y
//					); 
//			}
//		}
//
////		for (int i = 0; i < numParticles; i++) {
////			displacementLine.SetPosition(i, new Vector3(
////				eqPos[0, i].x, -1f + totalDisp[i] * 5f, 0.0f)); 
////			pressureLine.SetPosition(i, new Vector3(
////				eqPos[0, i].x, -1f + pressureDisp[i] * 5f, 0.0f)); 
////		}
//
//		float fScaled = f * 100; 
//		float vScaled = v * 100; 
//
//		frequencyText.text = fScaled.ToString("f2") + " Hz"; 
//		lambdaText.text = lambda.ToString("f2") + " m"; 
//		lengthText.text = L.ToString("f2") + " m"; 
//		speedText.text = vScaled.ToString("f2") + " m"; 
//
//
		t += dt; 
	}
}
