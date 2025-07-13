using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B2F RID: 2863
	public class NPCFieldUI : MonoBehaviour
	{
		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06004C54 RID: 19540 RVA: 0x00141397 File Offset: 0x0013F597
		// (set) Token: 0x06004C55 RID: 19541 RVA: 0x0014139F File Offset: 0x0013F59F
		public List<NPCField> Fields { get; protected set; } = new List<NPCField>();

		// Token: 0x06004C56 RID: 19542 RVA: 0x001413A8 File Offset: 0x0013F5A8
		public void Bind(List<NPCField> field)
		{
			this.Fields = new List<NPCField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onNPCChanged.AddListener(new UnityAction<NPC>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedNPC);
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x00141414 File Offset: 0x0013F614
		private void Refresh(NPC newVal)
		{
			this.IconImg.gameObject.SetActive(false);
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			if (this.AreFieldsUniform())
			{
				if (newVal != null)
				{
					this.IconImg.sprite = newVal.MugshotSprite;
					this.SelectionLabel.text = newVal.FirstName + "\n" + newVal.LastName;
					this.IconImg.gameObject.SetActive(true);
				}
				else
				{
					this.NoneSelected.SetActive(true);
					this.SelectionLabel.text = "None";
				}
			}
			else
			{
				this.MultipleSelected.SetActive(true);
				this.SelectionLabel.text = "Mixed";
			}
			NPCField npcfield = this.Fields.FirstOrDefault((NPCField x) => x.SelectedNPC != null);
			this.ClearButton.gameObject.SetActive(npcfield != null);
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x00141524 File Offset: 0x0013F724
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedNPC != this.Fields[i + 1].SelectedNPC)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x00141578 File Offset: 0x0013F778
		public void Clicked()
		{
			this.AreFieldsUniform();
			Singleton<ManagementInterface>.Instance.NPCSelector.Open("Select " + this.FieldLabel.text, this.Fields[0].TypeRequirement, new Action<NPC>(this.NPCSelected));
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x001415D0 File Offset: 0x0013F7D0
		public void NPCSelected(NPC npc)
		{
			if (npc != null && npc.GetType() != this.Fields[0].TypeRequirement)
			{
				Console.LogError("Wrong NPC type selection", null);
				return;
			}
			foreach (NPCField npcfield in this.Fields)
			{
				npcfield.SetNPC(npc, true);
			}
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x00141658 File Offset: 0x0013F858
		public void ClearClicked()
		{
			this.NPCSelected(null);
		}

		// Token: 0x040038E6 RID: 14566
		[Header("References")]
		public TextMeshProUGUI FieldLabel;

		// Token: 0x040038E7 RID: 14567
		public Image IconImg;

		// Token: 0x040038E8 RID: 14568
		public TextMeshProUGUI SelectionLabel;

		// Token: 0x040038E9 RID: 14569
		public GameObject NoneSelected;

		// Token: 0x040038EA RID: 14570
		public GameObject MultipleSelected;

		// Token: 0x040038EB RID: 14571
		public RectTransform ClearButton;
	}
}
