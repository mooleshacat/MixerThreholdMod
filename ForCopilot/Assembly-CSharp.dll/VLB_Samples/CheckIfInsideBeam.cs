using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x02000165 RID: 357
	[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(MeshRenderer))]
	public class CheckIfInsideBeam : MonoBehaviour
	{
		// Token: 0x060006EA RID: 1770 RVA: 0x0001EE4C File Offset: 0x0001D04C
		private void Start()
		{
			this.m_Collider = base.GetComponent<Collider>();
			MeshRenderer component = base.GetComponent<MeshRenderer>();
			if (component)
			{
				this.m_Material = component.material;
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001EE80 File Offset: 0x0001D080
		private void Update()
		{
			if (this.m_Material)
			{
				this.m_Material.SetColor("_Color", this.isInsideBeam ? Color.green : Color.red);
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001EEB3 File Offset: 0x0001D0B3
		private void FixedUpdate()
		{
			this.isInsideBeam = false;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001EEBC File Offset: 0x0001D0BC
		private void OnTriggerStay(Collider trigger)
		{
			DynamicOcclusionRaycasting component = trigger.GetComponent<DynamicOcclusionRaycasting>();
			if (component)
			{
				this.isInsideBeam = !component.IsColliderHiddenByDynamicOccluder(this.m_Collider);
				return;
			}
			this.isInsideBeam = true;
		}

		// Token: 0x0400079A RID: 1946
		private bool isInsideBeam;

		// Token: 0x0400079B RID: 1947
		private Material m_Material;

		// Token: 0x0400079C RID: 1948
		private Collider m_Collider;
	}
}
