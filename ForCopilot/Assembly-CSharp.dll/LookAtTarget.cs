using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class LookAtTarget : MonoBehaviour
{
	// Token: 0x060001E7 RID: 487 RVA: 0x0000B62E File Offset: 0x0000982E
	private void Update()
	{
		this._lookAtTarget = Vector3.Lerp(this._lookAtTarget, this._target.position, Time.deltaTime * this._speed);
		base.transform.LookAt(this._lookAtTarget);
	}

	// Token: 0x040001F7 RID: 503
	[SerializeField]
	private Transform _target;

	// Token: 0x040001F8 RID: 504
	[SerializeField]
	private float _speed = 0.5f;

	// Token: 0x040001F9 RID: 505
	private Vector3 _lookAtTarget;
}
