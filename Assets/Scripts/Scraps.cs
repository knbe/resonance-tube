
// gaussian pulse code 

//	float PosGaussianPulseDisplacement(float x, float t, float x0, 
//		float t0)
//	{
//		float disp = A * Mathf.Exp(-1 * (Mathf.Pow(x - x0 - 
//			v_s * (t - t0), 2)) / (2 * sigma * sigma)); 
//		return disp; 
//	}
//
//	float NegGaussianPulseDisplacement(float x, float t, float x0, 
//		float t0)
//	{
//		float disp = -A * Mathf.Exp(-1 * (Mathf.Pow(x - x0 + 
//			v_s * (t - t0), 2)) / (2 * sigma * sigma)); 
//		return disp; 
//	}
//
//	void CreateGaussianPulse(float currTime) 
//	{
//		if (t != pulses[numPulses].startTime) {
//			numPulses++;  
//			pulses[numPulses].startTime = currTime; 
//		}
//	}

//	void ChangeWaveformType(int value)
//	{
//		waveformType = value; 
//		ResetParticles(); 
//		// Debug.Log(waveformType); 
//	}

//	float[] RunGaussianPulseDemo()
//	{
//		float[] totalDisp = new float[numParticles]; 
//
//		startButton.onClick.AddListener(
//			delegate {CreateGaussianPulse(t); }); 
//
//		if (numPulses > 0) {
//			for (int k = 1; k < numPulses + 1; k++) {
//				// compute displacement due to pulse[i]
//				for (int i = 0; i < numParticles; i++) {
//					int numReflections_LCM = 
//						RoundToLowestMultiple(pulses[k].numReflections, 2); 
//
//					// add positive moving waves
//					float startpoint = -L * numReflections_LCM;  
//					float increment = 2 * L; 
//					for (int j = 0; j < 5; j++) {
//						totalDisp[i] += PosGaussianPulseDisplacement(
//							eqPos[0, i].x, 
//							t - pulses[k].startTime, 
//							startpoint - increment * j, 
//							0
//							); 
//					}
//
//					// add negative moving waves
//					startpoint = 2 * L; 
//					increment = 2 * L; 
//					for (int j = 0; j < 5; j++) {
//						totalDisp[i] += NegGaussianPulseDisplacement(
//							eqPos[0, i].x, 
//							t - pulses[k].startTime, 
//							startpoint + increment * j, 
//							0
//							); 
//					}
//				}
//
//				pulses[k].totDistTrav += v_s * dt; 
//				pulses[k].numReflections = (int)Mathf.Floor(pulses[k].totDistTrav / L); 
//				pulses[k].distToBoundary = (pulses[k].numReflections + 1) * L - 
//					pulses[k].totDistTrav; 
//
//				if (pulses[k].numReflections % 2 == 0)
//					pulses[k].direction = 1; 
//				else
//					pulses[k].direction = -1; 
//			}
//
////			if (pulses[1].numReflections == 0) {
////				speaker.transform.position = new Vector3(
////					PosGaussianPulseDisplacement(
////						0, t - pulses[1].startTime, 
////						0, 0), 
////					0, -.1f); 
////			}
////			else {
////				speaker.transform.position = new Vector3(
////					-.12f, 0, -.1f); 
////			}
//		}
//
//		return totalDisp; 
//	}

//	float PosSinuWaveDisplacement(float x)
//	{
//		// float damping = 1 - D * (t - sinuPulseStartTime); 
//		float disp = -A * 
//			Mathf.Sin(k * x  - omega_s * (t - sinuPulseStartTime)); 
//		return disp; 
//	}
//
//	float NegSinuWaveDisplacement(float x)
//	{
//		float numWavelengthsTrav = L / lambda; 
//		float wavelengthOffset = numWavelengthsTrav - Mathf.Floor(numWavelengthsTrav);  
//		float distOffset = wavelengthOffset * lambda;  
//
////		Debug.Log("num wavelengths trav = " + numWavelengthsTrav + 
////				" wavelength offset = " + wavelengthOffset + 
////				" distoffset = " + distOffset); 
//
//		// float disp = A * Mathf.Sin(k * (x) + omega_s * (t - sinuPulseStartTime) - 0.5f * Mathf.PI - distOffset);  
//		float disp = -A * Mathf.Sin(k * (x - distOffset - L) + omega_s * (t - sinuPulseStartTime)); 
//
//		return disp; 
//	}






// old methods for sinusoidal sim

//				if (sinwave.totDistTrav > L && (L - eqPos[0, i].x) < (sinwave.totDistTrav - L)) {
//					totalDisp[i] += 
//						NegSinWaveDisplacement(
//							eqPos[0, i].x - 2 * L
//							); 
//				}
//
//				if (sinwave.totDistTrav > (2 * L) && 
//					eqPos[0, i].x < (sinwave.totDistTrav - (2 * L))) {
//					totalDisp[i] += 
//						PosSinWaveDisplacement(
//							eqPos[0, i].x + 2 * L
//							); 
//				}
//
//				if (sinwave.totDistTrav > (3 * L) && 
//					(L - eqPos[0, i].x) < (sinwave.totDistTrav - 3 * L)) {
//					totalDisp[i] += 
//						NegSinWaveDisplacement(
//							eqPos[0, i].x - 4 * L
//							); 
//
//				}


//		if (sinuPulseExists == true) 
//		{
//			// Debug.Log("sinu pulse exists start time" + sinuPulseStartTime); 
//
//			sinuPulseTotDistTrav = v_s * (t - sinuPulseStartTime); 
//			// Debug.Log(sinuPulseTotDistTrav); 
//
//			sinuPulseNumReflections = (int)Mathf.Floor(sinuPulseTotDistTrav / L); 
//			sinuPulseDistToBoundary = (sinuPulseNumReflections + 1) * L - 
//				sinuPulseTotDistTrav; 
//
//			if (sinuPulseNumReflections % 2 == 0)
//				sinuPulseDirection = 1; 
//			else
//				sinuPulseDirection = -1; 
//			Debug.Log(sinuPulseDirection); 
//
//			// Debug.Log(sinuPulseNumReflections); 
//
//			// int numReflections_LCM = RoundToLowestMultiple(sinuPulseNumReflections, 2); 
//
//			// add positive moving waves
//			for (int i = 0; i < numParticles; i++)
//			{
//				if (eqPos[0, i].x < sinuPulseTotDistTrav)
//				{
//					totalDisp[i] += PosSinuWaveDisplacement(eqPos[0, i].x); 
//				}
//
//				if (sinuPulseTotDistTrav > L && 
//					(L - eqPos[0, i].x) < (sinuPulseTotDistTrav - L))
//				{
//					// Debug.Log("negative moving wave should be here"); 
//					totalDisp[i] += NegSinuWaveDisplacement(eqPos[0, i].x); 
//				}
//
//				if (sinuPulseTotDistTrav > (2 * L) && 
//					eqPos[0, i].x < (sinuPulseTotDistTrav - (2 * L)))
//				{
//					// Debug.Log("negative moving wave should be here"); 
//					totalDisp[i] += PosSinuWaveDisplacement(eqPos[0, i].x); 
//				}
//
//				if (sinuPulseTotDistTrav > (3 * L) && 
//					(L - eqPos[0, i].x) < (sinuPulseTotDistTrav - 3 * L))
//				{
//					// Debug.Log("negative moving wave should be here"); 
//					totalDisp[i] += NegSinuWaveDisplacement(eqPos[0, i].x); 
//				}
//			}
//
//		}
