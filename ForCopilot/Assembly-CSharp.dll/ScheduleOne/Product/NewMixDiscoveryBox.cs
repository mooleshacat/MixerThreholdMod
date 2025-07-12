using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Packaging;
using ScheduleOne.Properties;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Product
{
	// Token: 0x02000926 RID: 2342
	public class NewMixDiscoveryBox : MonoBehaviour
	{
		// Token: 0x06003F0A RID: 16138 RVA: 0x0010906C File Offset: 0x0010726C
		public void Start()
		{
			this.closedLidPose = new Pose(this.Lid.localPosition, this.Lid.localRotation);
			this.CloseCase();
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.IntObj.gameObject.SetActive(false);
			bool isMixComplete = NetworkSingleton<ProductManager>.Instance.IsMixComplete;
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x001090D8 File Offset: 0x001072D8
		public void ShowProduct(ProductDefinition baseDefinition, List<Property> properties)
		{
			this.PropertiesText.text = string.Empty;
			foreach (Property property in properties)
			{
				if (this.PropertiesText.text.Length > 0)
				{
					TextMeshPro propertiesText = this.PropertiesText;
					propertiesText.text += "\n";
				}
				TextMeshPro propertiesText2 = this.PropertiesText;
				propertiesText2.text = string.Concat(new string[]
				{
					propertiesText2.text,
					"<color=#",
					ColorUtility.ToHtmlStringRGBA(property.LabelColor),
					">",
					property.Name,
					"</color>"
				});
			}
			for (int i = 0; i < this.Visuals.Length; i++)
			{
				this.Visuals[i].Visuals.gameObject.SetActive(false);
			}
			ProductDefinition productDefinition = UnityEngine.Object.Instantiate<ProductDefinition>(baseDefinition);
			switch (baseDefinition.DrugType)
			{
			case EDrugType.Marijuana:
			{
				WeedDefinition weedDefinition = productDefinition as WeedDefinition;
				WeedAppearanceSettings appearanceSettings = WeedDefinition.GetAppearanceSettings(properties);
				weedDefinition.Initialize(properties, new List<EDrugType>
				{
					EDrugType.Marijuana
				}, appearanceSettings);
				(weedDefinition.GetDefaultInstance(1) as WeedInstance).SetupPackagingVisuals(this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Marijuana).Visuals);
				this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Marijuana).Visuals.gameObject.SetActive(true);
				break;
			}
			case EDrugType.Methamphetamine:
			{
				MethDefinition methDefinition = productDefinition as MethDefinition;
				MethAppearanceSettings appearanceSettings2 = MethDefinition.GetAppearanceSettings(properties);
				methDefinition.Initialize(properties, new List<EDrugType>
				{
					EDrugType.Methamphetamine
				}, appearanceSettings2);
				(methDefinition.GetDefaultInstance(1) as MethInstance).SetupPackagingVisuals(this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Methamphetamine).Visuals);
				this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Methamphetamine).Visuals.gameObject.SetActive(true);
				break;
			}
			case EDrugType.Cocaine:
			{
				CocaineDefinition cocaineDefinition = productDefinition as CocaineDefinition;
				CocaineAppearanceSettings appearanceSettings3 = CocaineDefinition.GetAppearanceSettings(properties);
				cocaineDefinition.Initialize(properties, new List<EDrugType>
				{
					EDrugType.Cocaine
				}, appearanceSettings3);
				(cocaineDefinition.GetDefaultInstance(1) as CocaineInstance).SetupPackagingVisuals(this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Cocaine).Visuals);
				this.Visuals.First((NewMixDiscoveryBox.DrugTypeVisuals x) => x.DrugType == EDrugType.Cocaine).Visuals.gameObject.SetActive(true);
				break;
			}
			default:
				Console.LogError("Drug type not supported", null);
				break;
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x00109404 File Offset: 0x00107604
		private void CloseCase()
		{
			this.isOpen = false;
			this.Lid.localPosition = this.closedLidPose.position;
			this.Lid.localRotation = this.closedLidPose.rotation;
		}

		// Token: 0x06003F0D RID: 16141 RVA: 0x00109439 File Offset: 0x00107639
		private void OpenCase()
		{
			this.isOpen = true;
			this.Animation.Play("New mix box open");
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x00109453 File Offset: 0x00107653
		private void Interacted()
		{
			if (!this.isOpen)
			{
				this.OpenCase();
			}
			Registry.GetItem(this.currentMix.ProductID);
		}

		// Token: 0x04002D12 RID: 11538
		private bool isOpen;

		// Token: 0x04002D13 RID: 11539
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x04002D14 RID: 11540
		public TextMeshPro PropertiesText;

		// Token: 0x04002D15 RID: 11541
		public NewMixDiscoveryBox.DrugTypeVisuals[] Visuals;

		// Token: 0x04002D16 RID: 11542
		public Animation Animation;

		// Token: 0x04002D17 RID: 11543
		public InteractableObject IntObj;

		// Token: 0x04002D18 RID: 11544
		public Transform Lid;

		// Token: 0x04002D19 RID: 11545
		private Pose closedLidPose;

		// Token: 0x04002D1A RID: 11546
		private NewMixOperation currentMix;

		// Token: 0x02000927 RID: 2343
		[Serializable]
		public class DrugTypeVisuals
		{
			// Token: 0x04002D1B RID: 11547
			public EDrugType DrugType;

			// Token: 0x04002D1C RID: 11548
			public FilledPackagingVisuals Visuals;
		}
	}
}
