using System;
using ScheduleOne.AvatarFramework.Customization;
using UnityEngine;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B88 RID: 2952
	public class BaseCharacterCreatorField : MonoBehaviour
	{
		// Token: 0x06004E59 RID: 20057 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Awake()
		{
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Start()
		{
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void ApplyValue()
		{
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void WriteValue(bool applyValue = true)
		{
		}

		// Token: 0x04003AAB RID: 15019
		public string PropertyName;

		// Token: 0x04003AAC RID: 15020
		public CharacterCreator.ECategory Category;

		// Token: 0x04003AAD RID: 15021
		private CharacterCreator Creator;
	}
}
