using UnityEngine;
using UnityEngine.EventSystems;

public class GraphCanvas : MonoBehaviour
{
	public Tube tube; 

	public LineRenderer gridlinePrefab; 
	public LineRenderer[] xGridlines; 
	public LineRenderer[] yGridlines; 
	// public LineRenderer pressureLine; 
	// public LineRenderer displacementLine; 

	public int numXGridlines; 
	public int numYGridlines; 
	public float xGridlineSpacing; 
	public float yGridlineSpacing; 
	public float y0; 

	public bool showGraphs; 

	void Start()
	{
		numXGridlines = 6; 
		xGridlines = new LineRenderer[numXGridlines]; 
		xGridlineSpacing = 1f; 

		numYGridlines = 1; 
		yGridlines = new LineRenderer[numYGridlines]; 
		yGridlineSpacing = .2f; 
		y0 = -1f; 

		showGraphs = true; 

		for (int i = 0; i < numXGridlines; i++) {
			xGridlines[i] = LineRenderer.Instantiate(
				gridlinePrefab) as LineRenderer; 
			xGridlines[i].SetPosition(0, new Vector2(
					0f + xGridlineSpacing * i, -2f)); 
			xGridlines[i].SetPosition(1, new Vector2(
					0f + xGridlineSpacing * i, .3f)); 
		}

		for (int i = 0; i < numYGridlines; i++) {
			yGridlines[i] = LineRenderer.Instantiate(
				gridlinePrefab) as LineRenderer; 
			yGridlines[i].SetPosition(0, new Vector2(
					0f, y0 + yGridlineSpacing * i)); 
			yGridlines[i].SetPosition(1, new Vector2(
					5f, y0 + yGridlineSpacing * i)); 
		}
	}

	void Update()
	{
		// if (showGraphs == true) {
//
//			rt.displacementLine.positionCount = rt.numParticles; 
//			for (int i = 0; i < rt.numParticles; i++) {
//				rt.displacementLine.SetPosition(i, 
//					new Vector3(
//						rt.particle[i].positionEq.x, 
//						y0 + rt.particle[i].displacement.x * 10, 
//						0f
//						)); 
//			}
//
		// }

	}
}
