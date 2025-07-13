using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200014A RID: 330
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-skewinghandle-sd/")]
	public class SkewingHandleSD : MonoBehaviour
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x0001C076 File Offset: 0x0001A276
		public bool IsAttachedToSelf()
		{
			return this.volumetricLightBeam != null && this.volumetricLightBeam.gameObject == base.gameObject;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001C09E File Offset: 0x0001A29E
		public bool CanSetSkewingVector()
		{
			return this.volumetricLightBeam != null && this.volumetricLightBeam.canHaveMeshSkewing;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001C0BB File Offset: 0x0001A2BB
		public bool CanUpdateEachFrame()
		{
			return this.CanSetSkewingVector() && this.volumetricLightBeam.trackChangesDuringPlaytime;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001C0D2 File Offset: 0x0001A2D2
		private bool ShouldUpdateEachFrame()
		{
			return this.shouldUpdateEachFrame && this.CanUpdateEachFrame();
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001C0E4 File Offset: 0x0001A2E4
		private void OnEnable()
		{
			if (this.CanSetSkewingVector())
			{
				this.SetSkewingVector();
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001C0F4 File Offset: 0x0001A2F4
		private void Start()
		{
			if (Application.isPlaying && this.ShouldUpdateEachFrame())
			{
				base.StartCoroutine(this.CoUpdate());
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001C112 File Offset: 0x0001A312
		private IEnumerator CoUpdate()
		{
			while (this.ShouldUpdateEachFrame())
			{
				this.SetSkewingVector();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001C124 File Offset: 0x0001A324
		private void SetSkewingVector()
		{
			Vector3 skewingLocalForwardDirection = this.volumetricLightBeam.transform.InverseTransformPoint(base.transform.position);
			this.volumetricLightBeam.skewingLocalForwardDirection = skewingLocalForwardDirection;
		}

		// Token: 0x040006E2 RID: 1762
		public const string ClassName = "SkewingHandleSD";

		// Token: 0x040006E3 RID: 1763
		public VolumetricLightBeamSD volumetricLightBeam;

		// Token: 0x040006E4 RID: 1764
		public bool shouldUpdateEachFrame;
	}
}
