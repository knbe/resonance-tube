using UnityEngine;
using UnityEngine.EventSystems;

public class LengthIncButton : MonoBehaviour, IPointerClickHandler
{ 
	public Tube tube; 

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData.button == 
				PointerEventData.InputButton.Left)
		{
			if (tube.L < 5f) {
				tube.L += 0.005f; 
				tube.stopper.transform.position = 
					new Vector3(tube.L, 0f, -0.1f); 

				tube.SetSamplingPoints(); 
				for (int j = 0; j < tube.numRows; j++) {
					for (int i = 0; i < tube.numParticles; i++) {
					    Destroy(tube.particle[j, i]); 
					}
				}
				tube.SetParticlePositions(); 
			}
		}

//			if (rt.L < 4.9f) {
//				rt.L += 0.01f; 
//				rt.stopper.transform.position = 
//					new Vector3(rt.L, 0f, -0.1f); 
//
//				for (int j = 0; j < rt.numRows; j++) {
//					for (int i = 0; i < rt.numParticles; i++) {
//					    Destroy(rt.particles[j, i]); 
//					}
//				}
//
//				rt.numParticles = (int)Mathf.Ceil(rt.L / 
//					rt.particleHorizSpacing_target); 
//				rt.particleHorizSpacing = rt.L / (rt.numParticles - 1); 
//				rt.numRows = 5; 
//
//				rt.InitializeEquilibriumState(rt.numParticles, 
//					rt.numRows, rt.particleHorizSpacing); 
//
//				rt.lengthSlider.value += 0.01f; 
//
//				// rt.ResetParticles(); 
//
//				// Debug.Log("new length is " + rt.L); 
//			}
//		}
	}
}

