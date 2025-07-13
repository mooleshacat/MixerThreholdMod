using System;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Handover
{
	// Token: 0x02000B79 RID: 2937
	public class HandoverScreenDetailPanel : MonoBehaviour
	{
		// Token: 0x06004DEF RID: 19951 RVA: 0x00149998 File Offset: 0x00147B98
		public void Open(Customer customer)
		{
			this.NameLabel.text = customer.NPC.fullName;
			if (customer.NPC.RelationData.Unlocked)
			{
				this.RelationshipContainer.gameObject.SetActive(true);
				this.RelationshipScrollbar.value = customer.NPC.RelationData.NormalizedRelationDelta;
				this.AddictionContainer.gameObject.SetActive(true);
				this.AdditionScrollbar.value = customer.CurrentAddiction;
			}
			else
			{
				this.RelationshipContainer.gameObject.SetActive(false);
				this.AddictionContainer.gameObject.SetActive(false);
			}
			this.StandardsStar.color = ItemQuality.GetColor(customer.CustomerData.Standards.GetCorrespondingQuality());
			this.StandardsLabel.text = customer.CustomerData.Standards.GetName();
			this.StandardsLabel.color = this.StandardsStar.color;
			this.EffectsLabel.text = string.Empty;
			for (int i = 0; i < customer.CustomerData.PreferredProperties.Count; i++)
			{
				if (i > 0)
				{
					TextMeshProUGUI effectsLabel = this.EffectsLabel;
					effectsLabel.text += "\n";
				}
				string str = string.Concat(new string[]
				{
					"<color=#",
					ColorUtility.ToHtmlStringRGBA(customer.CustomerData.PreferredProperties[i].LabelColor),
					">•  ",
					customer.CustomerData.PreferredProperties[i].Name,
					"</color>"
				});
				TextMeshProUGUI effectsLabel2 = this.EffectsLabel;
				effectsLabel2.text += str;
			}
			base.gameObject.SetActive(true);
			this.LayoutGroup.CalculateLayoutInputHorizontal();
			this.LayoutGroup.CalculateLayoutInputVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.LayoutGroup.GetComponent<RectTransform>());
			this.LayoutGroup.GetComponent<ContentSizeFitter>().SetLayoutVertical();
			this.Container.anchoredPosition = new Vector2(0f, -this.Container.sizeDelta.y / 2f);
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x000C6C29 File Offset: 0x000C4E29
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04003A3F RID: 14911
		public LayoutGroup LayoutGroup;

		// Token: 0x04003A40 RID: 14912
		public RectTransform Container;

		// Token: 0x04003A41 RID: 14913
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003A42 RID: 14914
		public RectTransform RelationshipContainer;

		// Token: 0x04003A43 RID: 14915
		public Scrollbar RelationshipScrollbar;

		// Token: 0x04003A44 RID: 14916
		public RectTransform AddictionContainer;

		// Token: 0x04003A45 RID: 14917
		public Scrollbar AdditionScrollbar;

		// Token: 0x04003A46 RID: 14918
		public Image StandardsStar;

		// Token: 0x04003A47 RID: 14919
		public TextMeshProUGUI StandardsLabel;

		// Token: 0x04003A48 RID: 14920
		public TextMeshProUGUI EffectsLabel;
	}
}
