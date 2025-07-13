using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B93 RID: 2963
	public class CharacterCreatorToggle : CharacterCreatorField<int>
	{
		// Token: 0x06004E80 RID: 20096 RVA: 0x0014BA0E File Offset: 0x00149C0E
		protected override void Awake()
		{
			base.Awake();
			this.Button1.onClick.AddListener(new UnityAction(this.OnButton1));
			this.Button2.onClick.AddListener(new UnityAction(this.OnButton2));
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0014BA4E File Offset: 0x00149C4E
		public override void ApplyValue()
		{
			base.ApplyValue();
			this.Button1.interactable = (base.value != 0);
			this.Button2.interactable = (base.value == 0);
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x0014BA7E File Offset: 0x00149C7E
		public void OnButton1()
		{
			base.value = 0;
			this.WriteValue(true);
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0014BA8E File Offset: 0x00149C8E
		public void OnButton2()
		{
			base.value = 1;
			this.WriteValue(true);
		}

		// Token: 0x04003ACE RID: 15054
		[Header("References")]
		public Button Button1;

		// Token: 0x04003ACF RID: 15055
		public Button Button2;
	}
}
