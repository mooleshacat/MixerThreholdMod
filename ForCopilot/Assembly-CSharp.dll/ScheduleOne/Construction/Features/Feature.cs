using System;
using FishNet.Object;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000764 RID: 1892
	public abstract class Feature : NetworkBehaviour
	{
		// Token: 0x060032F3 RID: 13043 RVA: 0x000D3C19 File Offset: 0x000D1E19
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Construction.Features.Feature_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x00065656 File Offset: 0x00063856
		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000D3C2D File Offset: 0x000D1E2D
		public virtual FI_Base CreateInterface(Transform parent)
		{
			FI_Base component = UnityEngine.Object.Instantiate<GameObject>(this.featureInterfacePrefab, parent).GetComponent<FI_Base>();
			component.Initialize(this);
			return component;
		}

		// Token: 0x060032F6 RID: 13046
		public abstract void Default();

		// Token: 0x060032F8 RID: 13048 RVA: 0x000D3C5A File Offset: 0x000D1E5A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x000D3C6D File Offset: 0x000D1E6D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.FeatureAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x000D3C80 File Offset: 0x000D1E80
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void dll()
		{
		}

		// Token: 0x040023F6 RID: 9206
		public string featureName = "Feature name";

		// Token: 0x040023F7 RID: 9207
		public Sprite featureIcon;

		// Token: 0x040023F8 RID: 9208
		public Transform featureIconLocation;

		// Token: 0x040023F9 RID: 9209
		public GameObject featureInterfacePrefab;

		// Token: 0x040023FA RID: 9210
		public bool disableRoofDisibility;

		// Token: 0x040023FB RID: 9211
		private bool dll_Excuted;

		// Token: 0x040023FC RID: 9212
		private bool dll_Excuted;
	}
}
