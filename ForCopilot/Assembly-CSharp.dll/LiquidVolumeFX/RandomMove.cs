using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000182 RID: 386
	public class RandomMove : MonoBehaviour
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x000213CC File Offset: 0x0001F5CC
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				this.flaskType++;
				if (this.flaskType >= 3)
				{
					this.flaskType = 0;
				}
				base.transform.Find("SphereFlask").gameObject.SetActive(this.flaskType == 0);
				base.transform.Find("CylinderFlask").gameObject.SetActive(this.flaskType == 1);
				base.transform.Find("CubeFlask").gameObject.SetActive(this.flaskType == 2);
			}
			Vector3 a = Vector3.zero;
			if (this.automatic)
			{
				if (UnityEngine.Random.value > 0.99f)
				{
					a = Vector3.right * (this.speed + (UnityEngine.Random.value - 0.5f) * this.randomSpeed);
				}
			}
			else
			{
				if (Input.GetKey(KeyCode.RightArrow))
				{
					a += Vector3.right * this.speed;
				}
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					a += Vector3.left * this.speed;
				}
				if (Input.GetKey(KeyCode.UpArrow))
				{
					a += Vector3.forward * this.speed;
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					a += Vector3.back * this.speed;
				}
			}
			float num = 60f * Time.deltaTime;
			this.velocity += a * num;
			float num2 = 0.005f * num;
			if (this.velocity.magnitude > num2)
			{
				this.velocity -= this.velocity.normalized * num2;
			}
			else
			{
				this.velocity = Vector3.zero;
			}
			base.transform.localPosition += this.velocity * num;
			if (Input.GetKey(KeyCode.W))
			{
				base.transform.Rotate(0f, 0f, this.rotationSpeed * num);
			}
			else if (Input.GetKey(KeyCode.S))
			{
				base.transform.Rotate(0f, 0f, -this.rotationSpeed * num);
			}
			if (base.transform.localPosition.x > this.right)
			{
				base.transform.localPosition = new Vector3(this.right, base.transform.localPosition.y, base.transform.localPosition.z);
				this.velocity.Set(0f, 0f, 0f);
			}
			if (base.transform.localPosition.x < this.left)
			{
				base.transform.localPosition = new Vector3(this.left, base.transform.localPosition.y, base.transform.localPosition.z);
				this.velocity.Set(0f, 0f, 0f);
			}
			if (base.transform.localPosition.z > this.back)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, this.back);
				this.velocity.Set(0f, 0f, 0f);
			}
			if (base.transform.localPosition.z < this.front)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, this.front);
				this.velocity.Set(0f, 0f, 0f);
			}
		}

		// Token: 0x0400082C RID: 2092
		[Range(-10f, 10f)]
		public float right = 2f;

		// Token: 0x0400082D RID: 2093
		[Range(-10f, 10f)]
		public float left = -2f;

		// Token: 0x0400082E RID: 2094
		[Range(-10f, 10f)]
		public float back = 2f;

		// Token: 0x0400082F RID: 2095
		[Range(-10f, 10f)]
		public float front = -1f;

		// Token: 0x04000830 RID: 2096
		[Range(0f, 0.2f)]
		public float speed = 0.5f;

		// Token: 0x04000831 RID: 2097
		[Range(0f, 2f)]
		public float rotationSpeed = 1f;

		// Token: 0x04000832 RID: 2098
		[Range(0.1f, 2f)]
		public float randomSpeed;

		// Token: 0x04000833 RID: 2099
		public bool automatic;

		// Token: 0x04000834 RID: 2100
		private Vector3 velocity = Vector3.zero;

		// Token: 0x04000835 RID: 2101
		private int flaskType;
	}
}
