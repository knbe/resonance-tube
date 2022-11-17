using UnityEngine;
using UnityEngine.EventSystems;

public class SelectParticle : MonoBehaviour, IPointerClickHandler
{
	public Tube tube; 

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData.button == 
				PointerEventData.InputButton.Left)
		{
			// tube.InitializeSinusoidalSignal(); 
			Debug.Log("particle selected"); 
		}
	}
}
