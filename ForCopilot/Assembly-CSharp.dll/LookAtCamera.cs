using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class LookAtCamera : MonoBehaviour
{
	// Token: 0x0600018B RID: 395 RVA: 0x0000858A File Offset: 0x0000678A
	public void Start()
	{
		if (this.lookAtCamera == null)
		{
			this.lookAtCamera = Camera.main;
		}
		if (this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x000085B3 File Offset: 0x000067B3
	public void Update()
	{
		if (!this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000085C3 File Offset: 0x000067C3
	public void LookCam()
	{
		base.transform.LookAt(this.lookAtCamera.transform);
	}

	// Token: 0x04000155 RID: 341
	public Camera lookAtCamera;

	// Token: 0x04000156 RID: 342
	public bool lookOnlyOnAwake;
}
