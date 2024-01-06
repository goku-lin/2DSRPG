using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : SingletonMono<CameraCtrl>
{
    [HideInInspector]
    public Vector3 center;
    public Vector3 limtXY;
    int moveSpeed = 30;
    private Coroutine c_shake;

    void Start()
    {
        center = this.transform.position;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            var x = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            var y = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            if (x + y < -moveSpeed)
            {
                x = 0;
                y = 0;
            }

            var eulerAngles = Camera.main.transform.localEulerAngles;

            eulerAngles.x = 0;

            var ro = Quaternion.Euler(eulerAngles);
            Camera.main.transform.Translate(ro * new Vector3(-x, -y, 0), Space.World);
        }

        var maxX = center.x + limtXY.x;
        var minX = center.x - limtXY.x;

        var minY = center.y - limtXY.y;
        var maxY = center.y + limtXY.y;
        var pos = this.transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        this.transform.position = pos;
    }

    public void Shake(float duration, float power = 1)
    {
        if (c_shake != null) StopCoroutine(c_shake);

        c_shake = StartCoroutine(c_Shake(duration, power));
    }

    /// <summary>
    /// 晃动镜头 晃动力量衰减
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator c_Shake(float duration, float power = 1)
    {
        var startTime = Time.time;

        var y = this.transform.position.y;
        var org_pos = this.transform.position;
        var effectPos = this.transform.position;
        var floatValue = 0;

        while (true)
        {

            var t = (Time.time - startTime) / duration;
            //晃动力量衰减
            effectPos.y = y + Mathf.Sin(floatValue) * (1 - t) * power;
            floatValue += 1;
            this.transform.position = effectPos;

            if (t >= 1)
            {
                this.transform.position = org_pos;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
