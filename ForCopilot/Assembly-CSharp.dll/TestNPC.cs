using System;
using ScheduleOne.Dialogue;
using ScheduleOne.Interaction;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class TestNPC : MonoBehaviour
{
	// Token: 0x06000095 RID: 149 RVA: 0x000052E9 File Offset: 0x000034E9
	public void Hovered()
	{
		if (DialogueHandler.activeDialogue == null)
		{
			this.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			return;
		}
		this.intObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00005311 File Offset: 0x00003511
	public void Interacted()
	{
		this.handler.InitializeDialogue("TestDialogue", true, "BRANCH_CHECKPASS");
	}

	// Token: 0x0400008B RID: 139
	[Header("References")]
	[SerializeField]
	protected InteractableObject intObj;

	// Token: 0x0400008C RID: 140
	[SerializeField]
	protected DialogueHandler handler;
}
