using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class RotateMoveCamera : MonoBehaviour
{
	// Token: 0x06000139 RID: 313 RVA: 0x00006FA4 File Offset: 0x000051A4
	private void Update()
	{
		float axis = Input.GetAxis("Mouse X");
		float axis2 = Input.GetAxis("Mouse Y");
		if (axis != this.MouseX || axis2 != this.MouseY)
		{
			this.rotationX += axis * this.sensX * Time.deltaTime;
			this.rotationY += axis2 * this.sensY * Time.deltaTime;
			this.rotationY = Mathf.Clamp(this.rotationY, this.minY, this.maxY);
			this.MouseX = axis;
			this.MouseY = axis2;
			this.Camera.transform.localEulerAngles = new Vector3(-this.rotationY, this.rotationX, 0f);
		}
		if (Input.GetKey(KeyCode.W))
		{
			base.transform.Translate(new Vector3(0f, 0f, 0.1f));
		}
		else if (Input.GetKey(KeyCode.S))
		{
			base.transform.Translate(new Vector3(0f, 0f, -0.1f));
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Translate(new Vector3(0.1f, 0f, 0f));
			return;
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Translate(new Vector3(-0.1f, 0f, 0f));
		}
	}

	// Token: 0x04000107 RID: 263
	public GameObject Camera;

	// Token: 0x04000108 RID: 264
	public float minX = -360f;

	// Token: 0x04000109 RID: 265
	public float maxX = 360f;

	// Token: 0x0400010A RID: 266
	public float minY = -45f;

	// Token: 0x0400010B RID: 267
	public float maxY = 45f;

	// Token: 0x0400010C RID: 268
	public float sensX = 100f;

	// Token: 0x0400010D RID: 269
	public float sensY = 100f;

	// Token: 0x0400010E RID: 270
	private float rotationY;

	// Token: 0x0400010F RID: 271
	private float rotationX;

	// Token: 0x04000110 RID: 272
	private float MouseX;

	// Token: 0x04000111 RID: 273
	private float MouseY;
}
