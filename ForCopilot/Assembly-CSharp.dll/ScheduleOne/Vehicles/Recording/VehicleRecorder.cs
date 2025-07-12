using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles.Recording
{
	// Token: 0x02000820 RID: 2080
	public class VehicleRecorder : MonoBehaviour
	{
		// Token: 0x06003897 RID: 14487 RVA: 0x000EE8F8 File Offset: 0x000ECAF8
		protected virtual void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				this.IS_RECORDING = !this.IS_RECORDING;
				if (this.IS_RECORDING)
				{
					this.keyFrames.Clear();
					this.vehicleToRecord = PlayerSingleton<PlayerMovement>.Instance.currentVehicle;
				}
			}
			if (this.vehicleToRecord && this.IS_RECORDING)
			{
				if (this.timeSinceKeyFrame >= 1f / (float)VehicleRecorder.frameRate)
				{
					this.timeSinceKeyFrame = 0f;
					VehicleKeyFrame item = this.Capture();
					this.keyFrames.Add(item);
				}
				Console.Log(this.vehicleToRecord.speed_Kmh, null);
				this.timeSinceKeyFrame += Time.deltaTime;
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x000EE9B0 File Offset: 0x000ECBB0
		private VehicleKeyFrame Capture()
		{
			VehicleKeyFrame vehicleKeyFrame = new VehicleKeyFrame();
			vehicleKeyFrame.position = this.vehicleToRecord.transform.position;
			vehicleKeyFrame.rotation = this.vehicleToRecord.transform.rotation;
			vehicleKeyFrame.brakesApplied = this.vehicleToRecord.brakesApplied;
			vehicleKeyFrame.reversing = this.vehicleToRecord.isReversing;
			if (this.vehicleToRecord.GetComponent<VehicleLights>())
			{
				vehicleKeyFrame.headlightsOn = this.vehicleToRecord.GetComponent<VehicleLights>().headLightsOn;
			}
			foreach (Wheel wheel in this.vehicleToRecord.wheels)
			{
				vehicleKeyFrame.wheels.Add(this.CaptureWheel(wheel));
			}
			return vehicleKeyFrame;
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x000EEA90 File Offset: 0x000ECC90
		private VehicleKeyFrame.WheelTransform CaptureWheel(Wheel wheel)
		{
			return new VehicleKeyFrame.WheelTransform
			{
				yPos = wheel.transform.Find("Model").transform.localPosition.y,
				rotation = wheel.transform.Find("Model").transform.localRotation
			};
		}

		// Token: 0x04002873 RID: 10355
		public static int frameRate = 24;

		// Token: 0x04002874 RID: 10356
		public bool IS_RECORDING;

		// Token: 0x04002875 RID: 10357
		public List<VehicleKeyFrame> keyFrames = new List<VehicleKeyFrame>();

		// Token: 0x04002876 RID: 10358
		private LandVehicle vehicleToRecord;

		// Token: 0x04002877 RID: 10359
		private float timeSinceKeyFrame;
	}
}
