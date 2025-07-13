using System;
using TMPro;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000731 RID: 1841
	[ExecuteInEditMode]
	public class PlaceholderBuilding : MonoBehaviour
	{
		// Token: 0x060031C8 RID: 12744 RVA: 0x000CFE66 File Offset: 0x000CE066
		private void Awake()
		{
			if (Application.isPlaying)
			{
				this.Model.GetComponent<Collider>().enabled = true;
			}
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000CFE80 File Offset: 0x000CE080
		protected virtual void LateUpdate()
		{
			if (Application.isPlaying)
			{
				return;
			}
			base.gameObject.name = "Placeholder (" + this.Name + ")";
			this.Label.text = this.Name;
			this.Model.localScale = this.Dimensions;
			if (base.transform.position != this.lastFramePosition)
			{
				RaycastHit raycastHit;
				if (this.AutoGround && Physics.Raycast(base.transform.position + Vector3.up * 50f, Vector3.down, ref raycastHit, 100f, 1 << LayerMask.NameToLayer("Default")))
				{
					this.Model.transform.position = new Vector3(this.Model.transform.position.x, raycastHit.point.y + this.Dimensions.y / 2f, this.Model.transform.position.z);
				}
				this.lastFramePosition = base.transform.position;
			}
			this.Label.transform.position = new Vector3(this.Label.transform.position.x, this.Model.transform.position.y + this.Dimensions.y / 2f + 0.1f, this.Label.transform.position.z);
		}

		// Token: 0x04002310 RID: 8976
		[Header("Settings")]
		public string Name;

		// Token: 0x04002311 RID: 8977
		public Vector3 Dimensions;

		// Token: 0x04002312 RID: 8978
		public bool AutoGround = true;

		// Token: 0x04002313 RID: 8979
		[Header("References")]
		public Transform Model;

		// Token: 0x04002314 RID: 8980
		public TextMeshPro Label;

		// Token: 0x04002315 RID: 8981
		private Vector3 lastFramePosition = Vector3.zero;
	}
}
