using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物体持续旋转组件
/// 挂载后物体会绕 Y 轴持续旋转（用于展示性动画，如炮塔展示台）
/// </summary>
public class TOWER_ROTATO : MonoBehaviour
{
    /// <summary>旋转速度（度/秒）</summary>
    public float rotateSpeed = 5;

    void Start()
    {

    }

    /// <summary>每帧绕 Y 轴旋转指定的角度</summary>
    void Update()
    {
        this.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
