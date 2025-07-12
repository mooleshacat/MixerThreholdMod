using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemIconCreator
{
	// Token: 0x02000233 RID: 563
	[ExecuteInEditMode]
	public class PrefabIconCreator : IconCreator
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x000377AA File Offset: 0x000359AA
		public override void BuildIcons()
		{
			base.StartCoroutine(this.BuildAllIcons());
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000377B9 File Offset: 0x000359B9
		public override bool CheckConditions()
		{
			if (!base.CheckConditions())
			{
				return false;
			}
			if (this.itemsToShot.Length == 0)
			{
				Debug.LogError("There's no prefab to shoot");
				return false;
			}
			if (this.itemPosition == null)
			{
				Debug.LogError("Item position is null");
				return false;
			}
			return true;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000377F8 File Offset: 0x000359F8
		protected override void Update()
		{
			if (this.preview && !this.isCreatingIcons)
			{
				if (this.instantiatedItem != null)
				{
					if (this.dynamicFov)
					{
						base.UpdateFOV(this.instantiatedItem);
					}
					if (this.lookAtObjectCenter)
					{
						base.LookAtTargetCenter(this.instantiatedItem);
					}
					this.instantiatedItem.transform.position = this.itemPosition.transform.position;
					this.instantiatedItem.transform.rotation = this.itemPosition.transform.rotation;
				}
				else if (this.instantiatedItem == null && this.itemsToShot.Length != 0)
				{
					this.ClearShit();
					if (this.itemPosition.childCount > 0 && this.itemPosition.GetChild(0).GetComponent<MeshRenderer>() != null)
					{
						this.instantiatedItem = this.itemPosition.GetChild(0).gameObject;
					}
					else
					{
						this.instantiatedItem = UnityEngine.Object.Instantiate<GameObject>(this.itemsToShot[0], this.itemPosition.transform.position, this.itemPosition.transform.rotation, this.itemPosition);
					}
				}
			}
			base.Update();
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00037938 File Offset: 0x00035B38
		private void ClearShit()
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < this.itemPosition.childCount; i++)
			{
				list.Add(this.itemPosition.GetChild(i));
			}
			for (int j = 0; j < list.Count; j++)
			{
				UnityEngine.Object.DestroyImmediate(list[j].gameObject);
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00037995 File Offset: 0x00035B95
		public IEnumerator BuildAllIcons()
		{
			base.Initialize();
			int num;
			for (int i = 0; i < this.itemsToShot.Length; i = num + 1)
			{
				this.finalPath = "C:/Users/Tyler/Desktop/";
				if (this.instantiatedItem != null)
				{
					UnityEngine.Object.DestroyImmediate(this.instantiatedItem);
				}
				if (this.whiteCam != null)
				{
					this.whiteCam.enabled = false;
				}
				if (this.blackCam != null)
				{
					this.blackCam.enabled = false;
				}
				this.ClearShit();
				this.instantiatedItem = UnityEngine.Object.Instantiate<GameObject>(this.itemsToShot[i], this.itemPosition.transform.position, this.itemPosition.transform.rotation);
				if (IconCreatorCanvas.instance != null)
				{
					IconCreatorCanvas.instance.SetInfo(this.itemsToShot.Length, i, this.itemsToShot[i].name, true, this.nextIconKey);
				}
				this.currentObject = this.instantiatedItem.transform;
				if (this.dynamicFov)
				{
					base.UpdateFOV(this.instantiatedItem);
				}
				if (this.lookAtObjectCenter)
				{
					base.LookAtTargetCenter(this.instantiatedItem);
				}
				if (this.mode == IconCreator.Mode.Manual)
				{
					this.CanMove = true;
					yield return new WaitUntil(() => Input.GetKeyDown(this.nextIconKey));
					this.CanMove = false;
				}
				if (IconCreatorCanvas.instance != null)
				{
					IconCreatorCanvas.instance.SetTakingPicture();
					yield return null;
					yield return null;
				}
				yield return base.CaptureFrame(this.itemsToShot[i].name, i);
				num = i;
			}
			if (IconCreatorCanvas.instance != null)
			{
				IconCreatorCanvas.instance.SetInfo(0, 0, "", false, this.nextIconKey);
			}
			base.DeleteCameras();
			yield break;
		}

		// Token: 0x04000D41 RID: 3393
		[Header("Items")]
		public GameObject[] itemsToShot;

		// Token: 0x04000D42 RID: 3394
		public Transform itemPosition;

		// Token: 0x04000D43 RID: 3395
		private GameObject instantiatedItem;
	}
}
