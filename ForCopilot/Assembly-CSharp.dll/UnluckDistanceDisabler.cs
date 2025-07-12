using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class UnluckDistanceDisabler : MonoBehaviour
{
	// Token: 0x06000186 RID: 390 RVA: 0x00008418 File Offset: 0x00006618
	public void Start()
	{
		if (this._distanceFromMainCam)
		{
			this._distanceFrom = Camera.main.transform;
		}
		base.InvokeRepeating("CheckDisable", this._disableCheckInterval + UnityEngine.Random.value * this._disableCheckInterval, this._disableCheckInterval);
		base.InvokeRepeating("CheckEnable", this._enableCheckInterval + UnityEngine.Random.value * this._enableCheckInterval, this._enableCheckInterval);
		base.Invoke("DisableOnStart", 0.01f);
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00008495 File Offset: 0x00006695
	public void DisableOnStart()
	{
		if (this._disableOnStart)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000084AC File Offset: 0x000066AC
	public void CheckDisable()
	{
		if (base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude > (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00008508 File Offset: 0x00006708
	public void CheckEnable()
	{
		if (!base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude < (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0400014F RID: 335
	public int _distanceDisable = 1000;

	// Token: 0x04000150 RID: 336
	public Transform _distanceFrom;

	// Token: 0x04000151 RID: 337
	public bool _distanceFromMainCam;

	// Token: 0x04000152 RID: 338
	public float _disableCheckInterval = 10f;

	// Token: 0x04000153 RID: 339
	public float _enableCheckInterval = 1f;

	// Token: 0x04000154 RID: 340
	public bool _disableOnStart;
}
