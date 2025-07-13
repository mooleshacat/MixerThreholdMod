using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class FlockScare : MonoBehaviour
{
	// Token: 0x060001B9 RID: 441 RVA: 0x0000A080 File Offset: 0x00008280
	private void CheckProximityToLandingSpots()
	{
		this.IterateLandingSpots();
		if (this.currentController._activeLandingSpots > 0 && this.CheckDistanceToLandingSpot(this.landingSpotControllers[this.lsc]))
		{
			this.landingSpotControllers[this.lsc].ScareAll();
		}
		base.Invoke("CheckProximityToLandingSpots", this.scareInterval);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000A0DC File Offset: 0x000082DC
	private void IterateLandingSpots()
	{
		this.ls += this.checkEveryNthLandingSpot;
		this.currentController = this.landingSpotControllers[this.lsc];
		int childCount = this.currentController.transform.childCount;
		if (this.ls > childCount - 1)
		{
			this.ls -= childCount;
			if (this.lsc < this.landingSpotControllers.Length - 1)
			{
				this.lsc++;
				return;
			}
			this.lsc = 0;
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000A164 File Offset: 0x00008364
	private bool CheckDistanceToLandingSpot(LandingSpotController lc)
	{
		Transform child = lc.transform.GetChild(this.ls);
		return child.GetComponent<LandingSpot>().landingChild != null && (child.position - base.transform.position).sqrMagnitude < this.distanceToScare * this.distanceToScare;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000A1C8 File Offset: 0x000083C8
	private void Invoker()
	{
		for (int i = 0; i < this.InvokeAmounts; i++)
		{
			float num = this.scareInterval / (float)this.InvokeAmounts * (float)i;
			base.Invoke("CheckProximityToLandingSpots", this.scareInterval + num);
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000A20B File Offset: 0x0000840B
	private void OnEnable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
		if (this.landingSpotControllers.Length != 0)
		{
			this.Invoker();
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000A227 File Offset: 0x00008427
	private void OnDisable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
	}

	// Token: 0x040001C4 RID: 452
	public LandingSpotController[] landingSpotControllers;

	// Token: 0x040001C5 RID: 453
	public float scareInterval = 0.1f;

	// Token: 0x040001C6 RID: 454
	public float distanceToScare = 2f;

	// Token: 0x040001C7 RID: 455
	public int checkEveryNthLandingSpot = 1;

	// Token: 0x040001C8 RID: 456
	public int InvokeAmounts = 1;

	// Token: 0x040001C9 RID: 457
	private int lsc;

	// Token: 0x040001CA RID: 458
	private int ls;

	// Token: 0x040001CB RID: 459
	private LandingSpotController currentController;
}
