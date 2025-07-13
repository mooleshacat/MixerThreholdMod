using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class FlockWaypointTrigger : MonoBehaviour
{
	// Token: 0x060001C0 RID: 448 RVA: 0x0000A260 File Offset: 0x00008460
	public void Start()
	{
		if (this._flockChild == null)
		{
			this._flockChild = base.transform.parent.GetComponent<FlockChild>();
		}
		float num = UnityEngine.Random.Range(this._timer, this._timer * 3f);
		base.InvokeRepeating("Trigger", num, num);
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000A2B6 File Offset: 0x000084B6
	public void Trigger()
	{
		this._flockChild.Wander(0f);
	}

	// Token: 0x040001CC RID: 460
	public float _timer = 1f;

	// Token: 0x040001CD RID: 461
	public FlockChild _flockChild;
}
