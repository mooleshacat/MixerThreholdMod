using System;
using TMPro;
using UnityEngine.Events;

namespace ScheduleOne.ExtendedComponents
{
	// Token: 0x0200065F RID: 1631
	public class GameInputField : TMP_InputField
	{
		// Token: 0x06002A4F RID: 10831 RVA: 0x000AF069 File Offset: 0x000AD269
		protected override void Awake()
		{
			base.Awake();
			base.onSelect.AddListener(new UnityAction<string>(this.EditStart));
			base.onEndEdit.AddListener(new UnityAction<string>(this.EndEdit));
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000AF09F File Offset: 0x000AD29F
		private void EditStart(string newVal)
		{
			GameInput.IsTyping = true;
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000AF0A7 File Offset: 0x000AD2A7
		private void EndEdit(string newVal)
		{
			GameInput.IsTyping = false;
		}
	}
}
