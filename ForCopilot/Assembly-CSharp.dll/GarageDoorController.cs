using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class GarageDoorController : MonoBehaviour
{
	// Token: 0x0600015D RID: 349 RVA: 0x00007B3C File Offset: 0x00005D3C
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("MainCamera"))
		{
			if (Input.GetKeyUp(KeyCode.E) && !this.doorStatus.doorIsOpen && this.doorStatus.canRotate)
			{
				this.doorStatus.canRotate = false;
				base.StartCoroutine(this.Rotate(Vector3.right, -80f, 1f));
			}
			if (Input.GetKeyUp(KeyCode.E) && this.doorStatus.doorIsOpen && this.doorStatus.canRotate)
			{
				this.doorStatus.canRotate = false;
				base.StartCoroutine(this.Rotate(Vector3.right, 80f, 1f));
			}
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00007BF4 File Offset: 0x00005DF4
	private IEnumerator Rotate(Vector3 axis, float angle, float duration = 1f)
	{
		Quaternion from = this.garageDoor.rotation;
		Quaternion to = this.garageDoor.rotation;
		to *= Quaternion.Euler(axis * angle);
		float elapsed = 0f;
		while (elapsed < duration)
		{
			this.garageDoor.rotation = Quaternion.Slerp(from, to, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}
		this.garageDoor.rotation = to;
		this.doorStatus.doorIsOpen = !this.doorStatus.doorIsOpen;
		this.doorStatus.canRotate = true;
		yield break;
	}

	// Token: 0x04000126 RID: 294
	public GarageDoorStatus doorStatus;

	// Token: 0x04000127 RID: 295
	public Transform garageDoor;

	// Token: 0x04000128 RID: 296
	public Quaternion targetRotation = new Quaternion(80f, 0f, 0f, 0f);
}
