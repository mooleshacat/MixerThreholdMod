using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B34 RID: 2868
	public class ObjectListFieldUI : MonoBehaviour
	{
		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004C75 RID: 19573 RVA: 0x00141CDC File Offset: 0x0013FEDC
		// (set) Token: 0x06004C76 RID: 19574 RVA: 0x00141CE4 File Offset: 0x0013FEE4
		public List<ObjectListField> Fields { get; protected set; } = new List<ObjectListField>();

		// Token: 0x06004C77 RID: 19575 RVA: 0x00141CF0 File Offset: 0x0013FEF0
		public void Bind(List<ObjectListField> field)
		{
			this.Fields = new List<ObjectListField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedObjects);
			if (field.Count == 1)
			{
				this.EditIcon.gameObject.SetActive(true);
				this.NoMultiEdit.gameObject.SetActive(false);
				this.Button.interactable = true;
				return;
			}
			this.EditIcon.gameObject.SetActive(false);
			this.NoMultiEdit.gameObject.SetActive(true);
			this.Button.interactable = false;
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x00141DC0 File Offset: 0x0013FFC0
		private void Refresh(List<BuildableItem> newVal)
		{
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			bool flag = this.AreFieldsUniform();
			if (flag)
			{
				if (this.Fields[0].SelectedObjects.Count == 0)
				{
					this.NoneSelected.SetActive(true);
				}
			}
			else
			{
				this.MultipleSelected.SetActive(true);
			}
			if (this.Fields.Count == 1)
			{
				this.FieldLabel.text = string.Concat(new string[]
				{
					this.FieldText,
					" (",
					newVal.Count.ToString(),
					"/",
					this.Fields[0].MaxItems.ToString(),
					")"
				});
			}
			else
			{
				this.FieldLabel.text = this.FieldText;
			}
			for (int i = 0; i < this.Entries.Length; i++)
			{
				if (flag && this.Fields[0].SelectedObjects.Count > i)
				{
					this.Entries[i].Find("Title").GetComponent<TextMeshProUGUI>().text = this.Fields[0].SelectedObjects[i].ItemInstance.Name;
					this.Entries[i].Find("Title").gameObject.SetActive(true);
				}
				else
				{
					this.Entries[i].Find("Title").gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x00141F5C File Offset: 0x0014015C
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (!this.Fields[i].SelectedObjects.SequenceEqual(this.Fields[i + 1].SelectedObjects))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00141FB0 File Offset: 0x001401B0
		public void Clicked()
		{
			List<BuildableItem> list = new List<BuildableItem>();
			if (this.AreFieldsUniform())
			{
				list.AddRange(this.Fields[0].SelectedObjects);
			}
			Singleton<ManagementInterface>.Instance.ObjectSelector.Open(this.InstructionText, this.ExtendedInstructionText, this.Fields[0].MaxItems, list, this.Fields[0].TypeRequirements, this.Fields[0].ParentConfig.Configurable.ParentProperty, new ObjectSelector.ObjectFilter(this.ObjectValid), new Action<List<BuildableItem>>(this.ObjectsSelected), null);
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x00142054 File Offset: 0x00140254
		private bool ObjectValid(BuildableItem obj, out string reason)
		{
			string text = string.Empty;
			for (int i = 0; i < this.Fields.Count; i++)
			{
				if (this.Fields[i].objectFilter == null || this.Fields[i].objectFilter(obj, out reason))
				{
					reason = string.Empty;
					return true;
				}
				text = reason;
			}
			reason = text;
			return false;
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x001420BC File Offset: 0x001402BC
		public void ObjectsSelected(List<BuildableItem> objs)
		{
			foreach (ObjectListField objectListField in this.Fields)
			{
				new List<BuildableItem>().AddRange(objs);
				objectListField.SetList(objs, true);
			}
		}

		// Token: 0x04003900 RID: 14592
		[Header("References")]
		public string FieldText = "Objects";

		// Token: 0x04003901 RID: 14593
		public string InstructionText = "Select <ObjectType>";

		// Token: 0x04003902 RID: 14594
		public string ExtendedInstructionText = string.Empty;

		// Token: 0x04003903 RID: 14595
		public TextMeshProUGUI FieldLabel;

		// Token: 0x04003904 RID: 14596
		public GameObject NoneSelected;

		// Token: 0x04003905 RID: 14597
		public GameObject MultipleSelected;

		// Token: 0x04003906 RID: 14598
		public RectTransform[] Entries;

		// Token: 0x04003907 RID: 14599
		public Button Button;

		// Token: 0x04003908 RID: 14600
		public GameObject EditIcon;

		// Token: 0x04003909 RID: 14601
		public GameObject NoMultiEdit;
	}
}
