using System;
using System.Collections.Generic;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000767 RID: 1895
	public class MaterialFeature : OptionListFeature
	{
		// Token: 0x0600330A RID: 13066 RVA: 0x000D3ECE File Offset: 0x000D20CE
		public override void SelectOption(int optionIndex)
		{
			base.SelectOption(optionIndex);
			this.ApplyMaterial(this.materials[optionIndex]);
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x000D3EEC File Offset: 0x000D20EC
		private void ApplyMaterial(MaterialFeature.NamedMaterial mat)
		{
			for (int i = 0; i < this.materialTargets.Count; i++)
			{
				this.materialTargets[i].material = mat.mat;
			}
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000D3F28 File Offset: 0x000D2128
		protected override List<FI_OptionList.Option> GetOptions()
		{
			List<FI_OptionList.Option> list = new List<FI_OptionList.Option>();
			for (int i = 0; i < this.materials.Count; i++)
			{
				list.Add(new FI_OptionList.Option(this.materials[i].matName, this.materials[i].buttonColor, this.materials[i].price));
			}
			return list;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000D3FAE File Offset: 0x000D21AE
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000D3FC7 File Offset: 0x000D21C7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.MaterialFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x000D3FE0 File Offset: 0x000D21E0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000D3FEE File Offset: 0x000D21EE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002409 RID: 9225
		[Header("References")]
		[SerializeField]
		protected List<MeshRenderer> materialTargets = new List<MeshRenderer>();

		// Token: 0x0400240A RID: 9226
		[Header("Material settings")]
		public List<MaterialFeature.NamedMaterial> materials = new List<MaterialFeature.NamedMaterial>();

		// Token: 0x0400240B RID: 9227
		private bool dll_Excuted;

		// Token: 0x0400240C RID: 9228
		private bool dll_Excuted;

		// Token: 0x02000768 RID: 1896
		[Serializable]
		public class NamedMaterial
		{
			// Token: 0x0400240D RID: 9229
			public string matName;

			// Token: 0x0400240E RID: 9230
			public Color buttonColor;

			// Token: 0x0400240F RID: 9231
			public Material mat;

			// Token: 0x04002410 RID: 9232
			public float price = 100f;
		}
	}
}
