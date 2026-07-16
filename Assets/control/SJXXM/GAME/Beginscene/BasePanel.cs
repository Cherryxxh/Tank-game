using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 面板泛型单例基类
/// 子类继承后自动拥有全局单例访问能力，提供统一的显示/隐藏功能
/// T 必须为引用类型
/// </summary>
public class BasePanel<T> : MonoBehaviour where T : class
{
    /// <summary>全局单例实例（每个子类 T 各有独立的一份）</summary>
    private static T instance;

    /// <summary>获取面板单例（通过类名直接访问，无需实例）</summary>
    public static T Instance => instance;

    /// <summary>对象激活时将自身注册为单例实例</summary>
    void Awake()
    {
        instance = this as T;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>显示面板（激活 GameObject）</summary>
    public virtual void ShowMe()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>隐藏面板（禁用 GameObject）</summary>
    public virtual void HideMe()
    {
        this.gameObject.SetActive(false);
    }
}
