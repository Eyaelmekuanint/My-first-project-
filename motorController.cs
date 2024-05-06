

    // Clamp the steering input within a reasonable range
    float steerDelta = 10 * Time.fixedDeltaTime;
    input.steer = Mathf.Clamp(input.steer, prevSteer - steerDelta, prevSteer + steerDelta);
    prevSteer = input.steer;
    return input;
  }

  private void motoMove(MotoInput input) {
    WColForward.steerAngle = Mathf.Clamp(input.steer, -1, 1) * maxSteerAngle;

    WColForward.brakeTorque = maxForwardBrake * input.brakeForward;
    WColBack.brakeTorque = maxBackBrake * input.brakeBack;

    WColBack.motorTorque = maxMotorTorque * input.acceleration;
  }

  private void updateWheels() {
    float delta = Time.fixedDeltaTime;

    foreach (WheelData w in wheels) {
      WheelHit hit;

      Vector3 localPos = w.wheelTransform.localPosition;
      if (w.wheelCollider.GetGroundHit(out hit)) {
        localPos.y -= Vector3.Dot(w.wheelTransform.position - hit.point, transform.up) - wheelRadius;
        w.wheelTransform.localPosition = localPos;
      } else {
        localPos.y = w.wheelStartPos.y - wheelOffset;
      }
      // w.wheelTransform.localPosition = localPos;

      w.rotation = Mathf.Repeat(w.rotation + delta * w.wheelCollider.rpm * 360.0f / 60.0f, 360f);
      w.wheelTransform.localRotation = Quaternion.Euler(w.rotation, w.wheelCollider.steerAngle, 0.0f);
      
    }
  }

  void OnGUI() {
    GUI.color = Color.black;
    var area = new Rect(0, 0, 100, 50);
    GUI.Label(area, speedVal.ToString("f1") + " m/s" + "\nangle = " + prevAngle.ToString("f3") + "\nangle' = " + prevOmega.ToString("f3"));
  }
}
