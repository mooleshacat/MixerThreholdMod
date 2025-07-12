using System;
using EasyButtons;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Equipping;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000892 RID: 2194
	public class EquipUtility : MonoBehaviour
	{
		// Token: 0x06003BCE RID: 15310 RVA: 0x000FCDD7 File Offset: 0x000FAFD7
		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				this.Equip();
			}
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x000FCDE8 File Offset: 0x000FAFE8
		[Button]
		public void Equip()
		{
			base.GetComponent<Avatar>().SetEquippable(this.Equippable.AssetPath);
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x000FCE01 File Offset: 0x000FB001
		[Button]
		public void Unequip()
		{
			base.GetComponent<Avatar>().SetEquippable(string.Empty);
		}

		// Token: 0x04002ABC RID: 10940
		public AvatarEquippable Equippable;
	}
}
