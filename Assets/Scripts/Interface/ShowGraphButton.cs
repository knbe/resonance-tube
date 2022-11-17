using UnityEngine;
using UnityEngine.EventSystems;

public class ShowGraphButton : MonoBehaviour, IPointerClickHandler
{
	public GraphCanvas gc; 

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData.button == 
				PointerEventData.InputButton.Left)
		{
			gc.showGraphs = true; 
		}
	}
}
