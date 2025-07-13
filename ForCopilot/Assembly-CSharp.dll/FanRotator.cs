using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class FanRotator : MonoBehaviour
{
	// Token: 0x06000133 RID: 307 RVA: 0x00006F3A File Offset: 0x0000513A
	private void Start()
	{
		this.thisTransform = base.GetComponent<Transform>();
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00006F48 File Offset: 0x00005148
	private void Update()
	{
		this.thisTransform.Rotate(0f, this.speed * Time.deltaTime, 0f, Space.Self);
	}

	// Token: 0x04000104 RID: 260
	private Transform thisTransform;

	// Token: 0x04000105 RID: 261
	public float speed = 90f;
}
