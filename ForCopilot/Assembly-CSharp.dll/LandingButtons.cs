using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class LandingButtons : MonoBehaviour
{
	// Token: 0x060001C3 RID: 451 RVA: 0x0000A2DC File Offset: 0x000084DC
	public void OnGUI()
	{
		GUI.Label(new Rect(20f, 20f, 125f, 18f), "Landing Spots: " + this._landingSpotController.transform.childCount.ToString());
		if (GUI.Button(new Rect(20f, 40f, 125f, 18f), "Scare All"))
		{
			this._landingSpotController.ScareAll();
		}
		if (GUI.Button(new Rect(20f, 60f, 125f, 18f), "Land In Reach"))
		{
			this._landingSpotController.LandAll();
		}
		if (GUI.Button(new Rect(20f, 80f, 125f, 18f), "Land Instant"))
		{
			base.StartCoroutine(this._landingSpotController.InstantLand(0.01f));
		}
		if (GUI.Button(new Rect(20f, 100f, 125f, 18f), "Destroy"))
		{
			this._flockController.destroyBirds();
		}
		GUI.Label(new Rect(20f, 120f, 125f, 18f), "Bird Amount: " + this._flockController._childAmount.ToString());
		this._flockController._childAmount = (int)GUI.HorizontalSlider(new Rect(20f, 140f, 125f, 18f), (float)this._flockController._childAmount, 0f, 250f);
	}

	// Token: 0x040001CE RID: 462
	public LandingSpotController _landingSpotController;

	// Token: 0x040001CF RID: 463
	public FlockController _flockController;

	// Token: 0x040001D0 RID: 464
	public float hSliderValue = 250f;
}
