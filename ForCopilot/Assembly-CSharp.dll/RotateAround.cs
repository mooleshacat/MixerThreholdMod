using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class RotateAround : MonoBehaviour
{
	// Token: 0x06000136 RID: 310 RVA: 0x000045B1 File Offset: 0x000027B1
	private void Start()
	{
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00006F7F File Offset: 0x0000517F
	private void Update()
	{
		base.transform.RotateAround(this.rot_center.position, Vector3.up, 0.25f);
	}

	// Token: 0x04000106 RID: 262
	public Transform rot_center;
}
