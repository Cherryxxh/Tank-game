using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 自定义 GUI 系统的绘制入口
/// 挂载在场景根节点上，每帧 OnGUI 遍历并绘制所有子控件
/// [ExecuteAlways] 使其在编辑模式下也能预览
/// </summary>
[ExecuteAlways]
public class CustomGuiRoot : MonoBehaviour
{
    /// <summary>所有子控件的缓存数组</summary>
    private CustomGuicontrol[] allcontrols;

    /// <summary>
    /// 启动时获取所有子控件
    /// </summary>
    void Start()
    {
        allcontrols = this.GetComponentsInChildren<CustomGuicontrol>();
    }

    /// <summary>
    /// Unity 的即时模式 GUI 绘制回调
    /// 每帧重新获取控件列表（支持编辑模式下动态增删），然后逐个调用 DrawGUI
    /// </summary>
    private void OnGUI()
    {
        allcontrols = this.GetComponentsInChildren<CustomGuicontrol>();

        for(int i = 0; i < allcontrols.Length; i++)
        {
            allcontrols[i].DrawGUI();
        }
    }
}
