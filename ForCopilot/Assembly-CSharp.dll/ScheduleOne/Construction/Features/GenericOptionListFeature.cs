using System;
using System.Collections.Generic;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000766 RID: 1894
	public class GenericOptionListFeature : OptionListFeature
	{
		// Token: 0x06003301 RID: 13057 RVA: 0x000D3CF0 File Offset: 0x000D1EF0
		public override void Default()
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].Uninstall();
			}
			this.PurchaseOption(this.defaultOptionIndex);
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x000D3D30 File Offset: 0x000D1F30
		protected override List<FI_OptionList.Option> GetOptions()
		{
			List<FI_OptionList.Option> list = new List<FI_OptionList.Option>();
			foreach (GenericOption genericOption in this.options)
			{
				list.Add(new FI_OptionList.Option(genericOption.optionName, genericOption.optionButtonColor, genericOption.optionPrice));
			}
			return list;
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x000D3DA0 File Offset: 0x000D1FA0
		public override void SelectOption(int optionIndex)
		{
			base.SelectOption(optionIndex);
			if (this.visibleOption != null && this.options[optionIndex] != this.visibleOption)
			{
				this.visibleOption.SetInvisible();
			}
			this.visibleOption = this.options[optionIndex];
			this.visibleOption.SetVisible();
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x000D3E04 File Offset: 0x000D2004
		public override void PurchaseOption(int optionIndex)
		{
			base.PurchaseOption(optionIndex);
			if (this.installedOption != null && this.options[optionIndex] != this.installedOption)
			{
				this.installedOption.Uninstall();
			}
			this.installedOption = this.options[optionIndex];
			this.installedOption.Install();
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x000D3E7A File Offset: 0x000D207A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x000D3E93 File Offset: 0x000D2093
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.GenericOptionListFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x000D3EAC File Offset: 0x000D20AC
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x000D3EBA File Offset: 0x000D20BA
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002404 RID: 9220
		[Header("References")]
		[SerializeField]
		protected List<GenericOption> options = new List<GenericOption>();

		// Token: 0x04002405 RID: 9221
		private GenericOption visibleOption;

		// Token: 0x04002406 RID: 9222
		private GenericOption installedOption;

		// Token: 0x04002407 RID: 9223
		private bool dll_Excuted;

		// Token: 0x04002408 RID: 9224
		private bool dll_Excuted;
	}
}
