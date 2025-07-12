using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class ResetPosition : MonoBehaviour
{
	// Token: 0x06000268 RID: 616 RVA: 0x0000DBBC File Offset: 0x0000BDBC
	private void Start()
	{
		this.startPosition = base.transform.position;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000DBCF File Offset: 0x0000BDCF
	private void Update()
	{
		if (Vector3.Distance(this.startPosition, base.transform.position) >= this.distanceToReset)
		{
			base.transform.position = this.startPosition;
		}
	}

	// Token: 0x0400028E RID: 654
	public float distanceToReset = 5f;

	// Token: 0x0400028F RID: 655
	private Vector3 startPosition;
}
