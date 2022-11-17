using UnityEngine;
using UnityEngine.EventSystems;

public class LengthDecButton : MonoBehaviour, IPointerClickHandler
{ 
	public Tube tube; 

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData.button == 
				PointerEventData.InputButton.Left)
		{
			if (tube.L > .5f) {
				tube.L -= 0.005f; 
				tube.stopper.transform.position = 
					new Vector3(tube.L, 0f, -0.1f); 

				tube.SetSamplingPoints(); 
				for (int j = 0; j < tube.numRows; j++) {
					for (int i = 0; i < tube.numParticles; i++) {
					    Destroy(tube.particle[j, i]); 
					}
				}
				tube.SetParticlePositions(); 

//				for (int j = 0; j < tube.numRows; j++) {
//					for (int i = 0; i < tube.numParticles; i++) {
//					    Destroy(tube.numParticles[j, i]); 
//					}
//				}
//
//				tube.numParticles= (int)Mathf.Ceil(tube.L / 
//					tube.pointSpacingTarget); 
//				tube. = tube.L / (tube.numPatubeicles - 1); 
//				tube.numRows = 5; 
//
//				tube.InitializeEquilibriumState(tube.numPatubeicles, 
//					tube.numRows, tube.patubeicleHorizSpacing); 
//
//				tube.lengthSlider.value -= 0.01f; 

				// tube.ResetPatubeicles(); 

				// Debug.Log("new length is " + tube.L); 
			}
		}
	}
}

