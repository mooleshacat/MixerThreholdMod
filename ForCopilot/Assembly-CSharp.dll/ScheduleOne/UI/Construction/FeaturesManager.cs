using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.Construction.Features;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Construction.Features;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Construction
{
	// Token: 0x02000BD3 RID: 3027
	public class FeaturesManager : Singleton<FeaturesManager>
	{
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x0600505D RID: 20573 RVA: 0x00153FBA File Offset: 0x001521BA
		public bool isActive
		{
			get
			{
				return this.activeConstructable != null;
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x00153FC8 File Offset: 0x001521C8
		protected override void Awake()
		{
			base.Awake();
			this.CloseFeatureMenu();
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x00153FD6 File Offset: 0x001521D6
		private void LateUpdate()
		{
			if (!this.isActive)
			{
				return;
			}
			if (this.featureIcons.Count > 0)
			{
				this.UpdateIconTransforms();
			}
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x00153FF8 File Offset: 0x001521F8
		public void OpenFeatureMenu(Feature feature)
		{
			if (this.selectedFeature != null)
			{
				this.CloseFeatureMenu();
			}
			this.selectedFeature = feature;
			this.featureMenuRect.gameObject.SetActive(true);
			this.featureMenuTitleLabel.text = Singleton<ConstructionMenu>.Instance.SelectedConstructable.ConstructableName + " > " + this.selectedFeature.featureName;
			if (feature.disableRoofDisibility && Singleton<ConstructionMenu>.Instance.SelectedConstructable is Constructable_GridBased)
			{
				(Singleton<ConstructionMenu>.Instance.SelectedConstructable as Constructable_GridBased).SetRoofVisible(false);
				this.roofSetInvisible = true;
			}
			this.currentFeatureInterface = feature.CreateInterface(this.featureInterfaceContainer);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001540A8 File Offset: 0x001522A8
		public void CloseFeatureMenu()
		{
			if (this.currentFeatureInterface != null)
			{
				this.currentFeatureInterface.Close();
			}
			if (this.roofSetInvisible)
			{
				if (Singleton<ConstructionMenu>.Instance.SelectedConstructable is Constructable_GridBased)
				{
					(Singleton<ConstructionMenu>.Instance.SelectedConstructable as Constructable_GridBased).SetRoofVisible(true);
				}
				this.roofSetInvisible = false;
			}
			this.selectedFeature = null;
			this.featureMenuRect.gameObject.SetActive(false);
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0015411C File Offset: 0x0015231C
		public void DeselectFeature()
		{
			if (this.selectedFeature == null)
			{
				return;
			}
			foreach (FeatureIcon featureIcon in this.featureIcons)
			{
				if (featureIcon.isSelected)
				{
					featureIcon.SetIsSelected(false);
				}
			}
			this.CloseFeatureMenu();
			this.selectedFeature = null;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x00154194 File Offset: 0x00152394
		public void Activate(Constructable constructable)
		{
			this.Deactivate();
			this.activeConstructable = constructable;
			this.CreateIcons();
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001541A9 File Offset: 0x001523A9
		public void Deactivate()
		{
			this.ClearIcons();
			if (this.selectedFeature != null)
			{
				this.CloseFeatureMenu();
			}
			this.activeConstructable = null;
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001541CC File Offset: 0x001523CC
		private void ClearIcons()
		{
			for (int i = 0; i < this.featureIcons.Count; i++)
			{
				UnityEngine.Object.Destroy(this.featureIcons[i].gameObject);
			}
			this.featureIcons.Clear();
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x00154210 File Offset: 0x00152410
		private void CreateIcons()
		{
			foreach (Feature feature in this.activeConstructable.features)
			{
				FeatureIcon component = UnityEngine.Object.Instantiate<GameObject>(this.featureIconPrefab, this.featureIconsContainer).GetComponent<FeatureIcon>();
				component.AssignFeature(feature);
				this.featureIcons.Add(component);
			}
			this.UpdateIconTransforms();
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x00154294 File Offset: 0x00152494
		private void UpdateIconTransforms()
		{
			foreach (FeatureIcon featureIcon in this.featureIcons)
			{
				featureIcon.UpdateTransform();
			}
		}

		// Token: 0x04003C53 RID: 15443
		public Constructable activeConstructable;

		// Token: 0x04003C54 RID: 15444
		public Feature selectedFeature;

		// Token: 0x04003C55 RID: 15445
		[Header("References")]
		[SerializeField]
		protected RectTransform featureIconsContainer;

		// Token: 0x04003C56 RID: 15446
		[SerializeField]
		protected RectTransform featureMenuRect;

		// Token: 0x04003C57 RID: 15447
		[SerializeField]
		protected TextMeshProUGUI featureMenuTitleLabel;

		// Token: 0x04003C58 RID: 15448
		[SerializeField]
		protected RectTransform featureInterfaceContainer;

		// Token: 0x04003C59 RID: 15449
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject featureIconPrefab;

		// Token: 0x04003C5A RID: 15450
		private FI_Base currentFeatureInterface;

		// Token: 0x04003C5B RID: 15451
		private bool roofSetInvisible;

		// Token: 0x04003C5C RID: 15452
		protected List<FeatureIcon> featureIcons = new List<FeatureIcon>();
	}
}
