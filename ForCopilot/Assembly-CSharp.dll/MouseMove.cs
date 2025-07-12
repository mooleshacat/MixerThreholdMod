using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class MouseMove : MonoBehaviour
{
	// Token: 0x060001E9 RID: 489 RVA: 0x0000B67C File Offset: 0x0000987C
	private void Start()
	{
		this._originalPos = base.transform.position;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000B690 File Offset: 0x00009890
	private void Update()
	{
		Vector3 vector = Input.mousePosition;
		vector.x /= (float)Screen.width;
		vector.y /= (float)Screen.height;
		vector.x -= 0.5f;
		vector.y -= 0.5f;
		vector *= 2f * this._sensitivity;
		base.transform.position = this._originalPos + vector;
	}

	// Token: 0x040001FA RID: 506
	[SerializeField]
	private float _sensitivity = 0.5f;

	// Token: 0x040001FB RID: 507
	private Vector3 _originalPos;
}
