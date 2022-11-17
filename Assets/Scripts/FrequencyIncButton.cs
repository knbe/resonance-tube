using UnityEngine;
using UnityEngine.EventSystems;

public class FrequencyIncButton : MonoBehaviour, IPointerClickHandler
{ 
	public Tube tube; 
	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData.button == 
				PointerEventData.InputButton.Left)
		{
			if (tube.f < 20.0f) {
				tube.f += .005f; 
				tube.omega = 2 * Mathf.PI * tube.f; 
				tube.fScaled = tube.f * tube.scalingFactor; 
			}
		}
	}

//	public ResonanceTube rt; 
//
//	public void OnPointerClick(PointerEventData pointerEventData)
//	{
//		if (pointerEventData.button == PointerEventData.InputButton.Left)
//		{
//			if (rt.f < 6.0f) {
//				rt.f += 0.01f; 
//				rt.omega = 2 * Mathf.PI * rt.f; 
//
//				rt.frequencySlider.value = rt.f * 100f; 
//
//				rt.ResetParticles(); 
//
//				// Debug.Log("new length is " + rt.L); 
//			}
//		}
//	}
}

