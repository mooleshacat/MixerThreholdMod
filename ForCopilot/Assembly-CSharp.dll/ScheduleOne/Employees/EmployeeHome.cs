using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Employees
{
	// Token: 0x02000684 RID: 1668
	public class EmployeeHome : MonoBehaviour
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06002C9F RID: 11423 RVA: 0x000B8057 File Offset: 0x000B6257
		// (set) Token: 0x06002CA0 RID: 11424 RVA: 0x000B805F File Offset: 0x000B625F
		public Employee AssignedEmployee { get; protected set; }

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000B8068 File Offset: 0x000B6268
		private void Awake()
		{
			if (this.Clipboard != null)
			{
				this.Clipboard.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000B8089 File Offset: 0x000B6289
		private void Start()
		{
			this.UpdateStorageText();
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000B8094 File Offset: 0x000B6294
		public void SetAssignedEmployee(Employee employee)
		{
			this.AssignedEmployee = employee;
			if (this.AssignedEmployee != null)
			{
				this.MugshotSprite.sprite = this.AssignedEmployee.MugshotSprite;
				this.NameLabel.text = this.AssignedEmployee.FirstName + "\n" + this.AssignedEmployee.LastName;
				this.Clipboard.gameObject.SetActive(true);
			}
			else
			{
				this.Clipboard.gameObject.SetActive(false);
			}
			if (this.onAssignedEmployeeChanged != null)
			{
				this.onAssignedEmployeeChanged.Invoke();
			}
			this.UpdateStorageText();
			this.UpdateMaterial();
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000B813C File Offset: 0x000B633C
		private void UpdateStorageText()
		{
			if (this.AssignedEmployee != null)
			{
				this.Storage.StorageEntityName = this.AssignedEmployee.FirstName + "'s " + this.HomeType;
				string text = "<color=#54E717>" + MoneyManager.FormatAmount(this.AssignedEmployee.DailyWage, false, false) + "</color>";
				this.Storage.StorageEntitySubtitle = string.Concat(new string[]
				{
					this.AssignedEmployee.fullName,
					" will draw ",
					this.AssignedEmployee.IsMale ? "his" : "her",
					" daily wage of ",
					text,
					" from this ",
					this.HomeType.ToLower()
				});
				return;
			}
			this.Storage.StorageEntityName = this.HomeType;
			this.Storage.StorageEntitySubtitle = string.Empty;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000B8230 File Offset: 0x000B6430
		private void UpdateMaterial()
		{
			foreach (MeshRenderer meshRenderer in this.EmployeeSpecificMeshes)
			{
				if (this.AssignedEmployee != null)
				{
					switch (this.AssignedEmployee.EmployeeType)
					{
					case EEmployeeType.Botanist:
						meshRenderer.material = this.SpecificMat_Botanist;
						break;
					case EEmployeeType.Handler:
						meshRenderer.material = this.SpecificMat_Packager;
						break;
					case EEmployeeType.Chemist:
						meshRenderer.material = this.SpecificMat_Chemist;
						break;
					case EEmployeeType.Cleaner:
						meshRenderer.material = this.SpecificMat_Cleaner;
						break;
					}
				}
				else
				{
					meshRenderer.material = this.SpecificMat_Default;
				}
			}
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000B82D0 File Offset: 0x000B64D0
		public float GetCashSum()
		{
			float num = 0f;
			foreach (ItemSlot itemSlot in this.Storage.ItemSlots)
			{
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is CashInstance)
				{
					num += (itemSlot.ItemInstance as CashInstance).Balance;
				}
			}
			return num;
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000B8350 File Offset: 0x000B6550
		public void RemoveCash(float amount)
		{
			foreach (ItemSlot itemSlot in this.Storage.ItemSlots)
			{
				if (amount <= 0f)
				{
					break;
				}
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is CashInstance)
				{
					CashInstance cashInstance = itemSlot.ItemInstance as CashInstance;
					float num = Mathf.Min(amount, cashInstance.Balance);
					cashInstance.ChangeBalance(-num);
					itemSlot.ReplicateStoredInstance();
					amount -= num;
				}
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000B83EC File Offset: 0x000B65EC
		public static bool IsBuildableEntityAValidEmployeeHome(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			if (!(obj.GetComponent<EmployeeHome>() != null))
			{
				return false;
			}
			EmployeeHome component = obj.GetComponent<EmployeeHome>();
			if (component.AssignedEmployee != null)
			{
				reason = "Already assigned to " + component.AssignedEmployee.fullName;
				return false;
			}
			return true;
		}

		// Token: 0x04001FE5 RID: 8165
		public string HomeType = "Briefcase";

		// Token: 0x04001FE6 RID: 8166
		[Header("References")]
		public GameObject Clipboard;

		// Token: 0x04001FE7 RID: 8167
		public SpriteRenderer MugshotSprite;

		// Token: 0x04001FE8 RID: 8168
		public TextMeshPro NameLabel;

		// Token: 0x04001FE9 RID: 8169
		public StorageEntity Storage;

		// Token: 0x04001FEA RID: 8170
		public MeshRenderer[] EmployeeSpecificMeshes;

		// Token: 0x04001FEB RID: 8171
		public Material SpecificMat_Default;

		// Token: 0x04001FEC RID: 8172
		public Material SpecificMat_Botanist;

		// Token: 0x04001FED RID: 8173
		public Material SpecificMat_Chemist;

		// Token: 0x04001FEE RID: 8174
		public Material SpecificMat_Packager;

		// Token: 0x04001FEF RID: 8175
		public Material SpecificMat_Cleaner;

		// Token: 0x04001FF0 RID: 8176
		public UnityEvent onAssignedEmployeeChanged;
	}
}
