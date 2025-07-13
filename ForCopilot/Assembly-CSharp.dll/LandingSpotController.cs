using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class LandingSpotController : MonoBehaviour
{
	// Token: 0x060001D4 RID: 468 RVA: 0x0000B2B4 File Offset: 0x000094B4
	public void Start()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._flock == null)
		{
			this._flock = (FlockController)UnityEngine.Object.FindObjectOfType(typeof(FlockController));
			Debug.Log(((this != null) ? this.ToString() : null) + " has no assigned FlockController, a random FlockController has been assigned");
		}
		if (this._landOnStart)
		{
			base.StartCoroutine(this.InstantLandOnStart(0.1f));
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000B339 File Offset: 0x00009539
	public void ScareAll()
	{
		this.ScareAll(0f, 1f);
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000B34C File Offset: 0x0000954C
	public void ScareAll(float minDelay, float maxDelay)
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().Invoke("ReleaseFlockChild", UnityEngine.Random.Range(minDelay, maxDelay));
			}
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000B3AC File Offset: 0x000095AC
	public void LandAll()
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				base.StartCoroutine(component.GetFlockChild(0f, 2f));
			}
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000B411 File Offset: 0x00009611
	public IEnumerator InstantLandOnStart(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000B427 File Offset: 0x00009627
	public IEnumerator InstantLand(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x040001DD RID: 477
	public bool _randomRotate = true;

	// Token: 0x040001DE RID: 478
	public Vector2 _autoCatchDelay = new Vector2(10f, 20f);

	// Token: 0x040001DF RID: 479
	public Vector2 _autoDismountDelay = new Vector2(10f, 20f);

	// Token: 0x040001E0 RID: 480
	public float _maxBirdDistance = 20f;

	// Token: 0x040001E1 RID: 481
	public float _minBirdDistance = 5f;

	// Token: 0x040001E2 RID: 482
	public bool _takeClosest;

	// Token: 0x040001E3 RID: 483
	public FlockController _flock;

	// Token: 0x040001E4 RID: 484
	public bool _landOnStart;

	// Token: 0x040001E5 RID: 485
	public bool _soarLand = true;

	// Token: 0x040001E6 RID: 486
	public bool _onlyBirdsAbove;

	// Token: 0x040001E7 RID: 487
	public float _landingSpeedModifier = 0.5f;

	// Token: 0x040001E8 RID: 488
	public float _landingTurnSpeedModifier = 5f;

	// Token: 0x040001E9 RID: 489
	public Transform _featherPS;

	// Token: 0x040001EA RID: 490
	public Transform _thisT;

	// Token: 0x040001EB RID: 491
	public int _activeLandingSpots;

	// Token: 0x040001EC RID: 492
	public float _snapLandDistance = 0.1f;

	// Token: 0x040001ED RID: 493
	public float _landedRotateSpeed = 0.01f;

	// Token: 0x040001EE RID: 494
	public float _gizmoSize = 0.2f;
}
