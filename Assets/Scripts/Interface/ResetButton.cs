using UnityEngine;
using UnityEngine.EventSystems;

public class ResetButton : MonoBehaviour, IPointerClickHandler
{
	public Tube tube; 

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData.button == 
				PointerEventData.InputButton.Left)
		{
			tube.StopSinusoidalSignal(); 
		}
	}
}
