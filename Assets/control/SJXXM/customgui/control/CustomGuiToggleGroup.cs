using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义 Toggle 组控件（互斥开关组）
/// 确保一组 Toggle 中只有一个被选中，实现类似 RadioButton 的效果
/// </summary>
public class CustomGuiToggleGroup : MonoBehaviour
{
    /// <summary>组内所有 Toggle 控件</summary>
    public CustomGuiToggle[] toggles;

    /// <summary>当前选中的 Toggle</summary>
    private CustomGuiToggle frontTurTog;

    /// <summary>
    /// 启动时为每个 Toggle 订阅事件，实现互斥逻辑：
    /// - 选中一个时，取消其他所有
    /// - 点击已选中的不会取消（至少保持一个选中）
    /// </summary>
    void Start()
    {
        if(toggles.Length == 0)
        return;

        for(int i = 0; i < toggles.Length; i++)
        {
            CustomGuiToggle toggle = toggles[i];
            toggle.changevalue += (value) =>
            {
                if(value)
                {
                    // 选中当前，取消其他所有
                    for(int j = 0; j < toggles.Length; j++)
                    {
                        if(toggles[j] != toggle)
                        {
                            toggles[j].issel = false;
                        }
                    }
                    frontTurTog = toggle;
                }
                else if (frontTurTog == toggle)
                {
                    // 不允许取消已选中的（至少保持一个选中）
                    toggle.issel = true;
                }
            };
        }
    }
}
