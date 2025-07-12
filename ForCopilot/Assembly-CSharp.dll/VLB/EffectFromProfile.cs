using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000FF RID: 255
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-from-profile/")]
	public class EffectFromProfile : MonoBehaviour
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00016B13 File Offset: 0x00014D13
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00016B1B File Offset: 0x00014D1B
		public EffectAbstractBase effectProfile
		{
			get
			{
				return this.m_EffectProfile;
			}
			set
			{
				this.m_EffectProfile = value;
				this.InitInstanceFromProfile();
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00016B2A File Offset: 0x00014D2A
		public void InitInstanceFromProfile()
		{
			if (this.m_EffectInstance)
			{
				if (this.m_EffectProfile)
				{
					this.m_EffectInstance.InitFrom(this.m_EffectProfile);
					return;
				}
				this.m_EffectInstance.enabled = false;
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00016B64 File Offset: 0x00014D64
		private void OnEnable()
		{
			if (this.m_EffectInstance)
			{
				this.m_EffectInstance.enabled = true;
				return;
			}
			if (this.m_EffectProfile)
			{
				this.m_EffectInstance = (base.gameObject.AddComponent(this.m_EffectProfile.GetType()) as EffectAbstractBase);
				this.InitInstanceFromProfile();
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00016BBF File Offset: 0x00014DBF
		private void OnDisable()
		{
			if (this.m_EffectInstance)
			{
				this.m_EffectInstance.enabled = false;
			}
		}

		// Token: 0x04000593 RID: 1427
		public const string ClassName = "EffectFromProfile";

		// Token: 0x04000594 RID: 1428
		[SerializeField]
		private EffectAbstractBase m_EffectProfile;

		// Token: 0x04000595 RID: 1429
		private EffectAbstractBase m_EffectInstance;
	}
}
