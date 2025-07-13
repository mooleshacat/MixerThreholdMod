using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.UI;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using ScheduleOne.UI.Management;
using TMPro;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x020005C3 RID: 1475
	public class ManagementInterface : Singleton<ManagementInterface>
	{
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x00095099 File Offset: 0x00093299
		// (set) Token: 0x0600245B RID: 9307 RVA: 0x000950A1 File Offset: 0x000932A1
		public ManagementClipboard_Equippable EquippedClipboard { get; protected set; }

		// Token: 0x0600245C RID: 9308 RVA: 0x000950AA File Offset: 0x000932AA
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000950B4 File Offset: 0x000932B4
		public void Open(List<IConfigurable> configurables, ManagementClipboard_Equippable _equippedClipboard)
		{
			this.Configurables = new List<IConfigurable>();
			this.Configurables.AddRange(configurables);
			this.EquippedClipboard = _equippedClipboard;
			this.areConfigurablesUniform = true;
			if (this.Configurables.Count > 1)
			{
				for (int i = 0; i < this.Configurables.Count - 1; i++)
				{
					if (this.Configurables[i].ConfigurableType != this.Configurables[i + 1].ConfigurableType)
					{
						this.areConfigurablesUniform = false;
						break;
					}
				}
			}
			this.UpdateMainLabels();
			this.InitializeConfigPanel();
			Singleton<InputPromptsCanvas>.Instance.LoadModule("backonly_rightclick");
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x00095158 File Offset: 0x00093358
		public void Close(bool preserveState = false)
		{
			if (this.ItemSelectorScreen.IsOpen)
			{
				this.ItemSelectorScreen.Close();
			}
			if (this.RecipeSelectorScreen.IsOpen)
			{
				this.RecipeSelectorScreen.Close();
			}
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "exitonly")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			this.DestroyConfigPanel();
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000951BB File Offset: 0x000933BB
		private void UpdateMainLabels()
		{
			this.NothingSelectedLabel.gameObject.SetActive(this.Configurables.Count == 0);
			this.DifferentTypesSelectedLabel.gameObject.SetActive(!this.areConfigurablesUniform);
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000951F4 File Offset: 0x000933F4
		private void InitializeConfigPanel()
		{
			if (this.loadedPanel != null)
			{
				Console.LogWarning("InitializeConfigPanel called when there is an existing config panel. Destroying existing.", null);
				this.DestroyConfigPanel();
			}
			if (!this.areConfigurablesUniform || this.Configurables.Count == 0)
			{
				return;
			}
			ConfigPanel configPanelPrefab = this.GetConfigPanelPrefab(this.Configurables[0].ConfigurableType);
			this.loadedPanel = UnityEngine.Object.Instantiate<ConfigPanel>(configPanelPrefab, this.PanelContainer).GetComponent<ConfigPanel>();
			this.loadedPanel.Bind((from x in this.Configurables
			select x.Configuration).ToList<EntityConfiguration>());
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0009529F File Offset: 0x0009349F
		private void DestroyConfigPanel()
		{
			if (this.loadedPanel != null)
			{
				UnityEngine.Object.Destroy(this.loadedPanel.gameObject);
				this.loadedPanel = null;
			}
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000952C8 File Offset: 0x000934C8
		public ConfigPanel GetConfigPanelPrefab(EConfigurableType type)
		{
			return this.ConfigPanelPrefabs.FirstOrDefault((ManagementInterface.ConfigurableTypePanel x) => x.Type == type).Panel;
		}

		// Token: 0x04001AF1 RID: 6897
		public const float PANEL_SLIDE_TIME = 0.1f;

		// Token: 0x04001AF3 RID: 6899
		[Header("References")]
		public TextMeshProUGUI NothingSelectedLabel;

		// Token: 0x04001AF4 RID: 6900
		public TextMeshProUGUI DifferentTypesSelectedLabel;

		// Token: 0x04001AF5 RID: 6901
		public RectTransform PanelContainer;

		// Token: 0x04001AF6 RID: 6902
		public ClipboardScreen MainScreen;

		// Token: 0x04001AF7 RID: 6903
		public ScheduleOne.UI.Management.ItemSelector ItemSelectorScreen;

		// Token: 0x04001AF8 RID: 6904
		public NPCSelector NPCSelector;

		// Token: 0x04001AF9 RID: 6905
		public ScheduleOne.UI.Management.ObjectSelector ObjectSelector;

		// Token: 0x04001AFA RID: 6906
		public RecipeSelector RecipeSelectorScreen;

		// Token: 0x04001AFB RID: 6907
		public TransitEntitySelector TransitEntitySelector;

		// Token: 0x04001AFC RID: 6908
		[SerializeField]
		protected ManagementInterface.ConfigurableTypePanel[] ConfigPanelPrefabs;

		// Token: 0x04001AFD RID: 6909
		public List<IConfigurable> Configurables = new List<IConfigurable>();

		// Token: 0x04001AFE RID: 6910
		private bool areConfigurablesUniform;

		// Token: 0x04001AFF RID: 6911
		private ConfigPanel loadedPanel;

		// Token: 0x020005C4 RID: 1476
		[Serializable]
		public class ConfigurableTypePanel
		{
			// Token: 0x04001B00 RID: 6912
			public EConfigurableType Type;

			// Token: 0x04001B01 RID: 6913
			public ConfigPanel Panel;
		}
	}
}
