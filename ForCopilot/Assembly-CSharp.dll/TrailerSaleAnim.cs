using System;
using ScheduleOne.NPCs;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class TrailerSaleAnim : MonoBehaviour
{
	// Token: 0x060000AA RID: 170 RVA: 0x00005670 File Offset: 0x00003870
	public void PlayAnim()
	{
		Debug.Log("Playing");
		NPC[] npcs = this.NPCs;
		for (int i = 0; i < npcs.Length; i++)
		{
			npcs[i].Avatar.Anim.SetTrigger("GrabItem");
		}
	}

	// Token: 0x0400009F RID: 159
	public NPC[] NPCs;
}
