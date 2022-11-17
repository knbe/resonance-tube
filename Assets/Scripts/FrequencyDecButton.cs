using UnityEngine;
using UnityEngine.EventSystems;

public class FrequencyDecButton : MonoBehaviour, IPointerClickHandler
{ 
	public Tube tube; 
	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData.button == 
				PointerEventData.InputButton.Left)
		{
			if (tube.f > 1.440f) {
				tube.f -= .005f; 
				tube.omega = 2 * Mathf.PI * tube.f; 
				tube.fScaled = tube.f * tube.scalingFactor; 
			}
		}
	}
}
