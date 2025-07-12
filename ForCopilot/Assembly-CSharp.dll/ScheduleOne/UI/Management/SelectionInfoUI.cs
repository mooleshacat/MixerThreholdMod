using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B48 RID: 2888
	public class SelectionInfoUI : MonoBehaviour
	{
		// Token: 0x06004CFC RID: 19708 RVA: 0x001446A8 File Offset: 0x001428A8
		private void Update()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.SelfUpdate)
			{
				List<IConfigurable> list = new List<IConfigurable>();
				list.AddRange(Singleton<ManagementWorldspaceCanvas>.Instance.SelectedConfigurables);
				if (Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable != null && !list.Contains(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable))
				{
					list.Add(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable);
				}
				this.Set(list);
			}
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x00144718 File Offset: 0x00142918
		public void Set(List<IConfigurable> Configurables)
		{
			if (Configurables.Count == 0)
			{
				this.Icon.sprite = this.CrossSprite;
				this.Title.text = "Nothing selected";
				return;
			}
			bool flag = true;
			if (Configurables.Count > 1)
			{
				for (int i = 0; i < Configurables.Count - 1; i++)
				{
					if (Configurables[i].ConfigurableType != Configurables[i + 1].ConfigurableType)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.Icon.sprite = Configurables[0].TypeIcon;
				this.Title.text = Configurables.Count.ToString() + "x " + ConfigurableType.GetTypeName(Configurables[0].ConfigurableType);
				return;
			}
			this.Icon.sprite = this.NonUniformTypeSprite;
			this.Title.text = Configurables.Count.ToString() + "x Mixed types";
		}

		// Token: 0x0400396B RID: 14699
		[Header("References")]
		public Image Icon;

		// Token: 0x0400396C RID: 14700
		public TextMeshProUGUI Title;

		// Token: 0x0400396D RID: 14701
		[Header("Settings")]
		public bool SelfUpdate = true;

		// Token: 0x0400396E RID: 14702
		public Sprite NonUniformTypeSprite;

		// Token: 0x0400396F RID: 14703
		public Sprite CrossSprite;
	}
}
