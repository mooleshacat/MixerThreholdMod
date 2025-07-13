using System;
using UnityEngine;

namespace Beautify.Universal
{
	// Token: 0x020001FF RID: 511
	public class SphereAnimator : MonoBehaviour
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x00031400 File Offset: 0x0002F600
		private void Start()
		{
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00031410 File Offset: 0x0002F610
		private void FixedUpdate()
		{
			if (base.transform.position.z < 2.5f)
			{
				this.rb.AddForce(Vector3.forward * 200f * Time.fixedDeltaTime);
				return;
			}
			if (base.transform.position.z > 8f)
			{
				this.rb.AddForce(Vector3.back * 200f * Time.fixedDeltaTime);
			}
		}

		// Token: 0x04000BE9 RID: 3049
		private Rigidbody rb;
	}
}
