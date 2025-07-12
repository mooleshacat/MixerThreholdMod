using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200004A RID: 74
[ExecuteInEditMode]
public class WaterPipeModular : MonoBehaviour
{
	// Token: 0x0600017F RID: 383 RVA: 0x00008284 File Offset: 0x00006484
	private void Start()
	{
		this.largeWaterPipe = (Resources.Load("Models/Water_Pipe_Long") as GameObject);
		this.mediumWaterPipe = (Resources.Load("Models/Water_Pipe_Medium") as GameObject);
		this.smallWaterpipe = (Resources.Load("Models/Water_Pipe_Small") as GameObject);
		this.innerCorner = (Resources.Load("Models/Water_Pipe_left") as GameObject);
		this.outerCorner = (Resources.Load("Models/Water_Pipe_right") as GameObject);
	}

	// Token: 0x06000180 RID: 384 RVA: 0x000082FC File Offset: 0x000064FC
	public void BuildNextItem(GameObject item)
	{
		if (this.itemsList.Count == 0)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item, base.transform.position, item.transform.rotation);
			gameObject.transform.SetParent(base.transform);
			this.itemsList.Add(gameObject);
			return;
		}
		Transform child = this.itemsList.Last<GameObject>().transform.GetChild(0);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(item, child.position, child.rotation);
		gameObject2.transform.SetParent(base.transform);
		this.itemsList.Add(gameObject2);
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00008398 File Offset: 0x00006598
	public void DeleteLastItem()
	{
		GameObject gameObject = this.itemsList.Last<GameObject>();
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if (Application.isEditor)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		this.itemsList.Remove(gameObject);
	}

	// Token: 0x04000147 RID: 327
	[HideInInspector]
	public List<GameObject> itemsList = new List<GameObject>();

	// Token: 0x04000148 RID: 328
	[HideInInspector]
	public GameObject largeWaterPipe;

	// Token: 0x04000149 RID: 329
	[HideInInspector]
	public GameObject mediumWaterPipe;

	// Token: 0x0400014A RID: 330
	[HideInInspector]
	public GameObject smallWaterpipe;

	// Token: 0x0400014B RID: 331
	[HideInInspector]
	public GameObject innerCorner;

	// Token: 0x0400014C RID: 332
	[HideInInspector]
	public GameObject outerCorner;
}
