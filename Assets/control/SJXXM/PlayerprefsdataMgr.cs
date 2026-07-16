using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// PlayerPrefs 数据持久化管理器（单例）
/// 使用反射自动将任意对象的所有 public 字段序列化到 PlayerPrefs 或反序列化读取
/// 支持基础类型（int/float/string/bool）、List、Dictionary 和嵌套对象
/// </summary>
public class PlayerprefsdataMgr
{
    /// <summary>单例实例</summary>
    static PlayerprefsdataMgr instance = new PlayerprefsdataMgr();

    /// <summary>获取单例入口</summary>
    public static PlayerprefsdataMgr Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>私有构造函数</summary>
    private PlayerprefsdataMgr()
    {

    }

    /// <summary>
    /// 将对象的所有 public 字段保存到 PlayerPrefs
    /// 字段键名格式: key_类型名_字段类型名_字段名
    /// </summary>
    /// <param name="data">要保存的数据对象</param>
    /// <param name="key">存储键名前缀</param>
    public void SaveData(object data, string key)
    {
        Type dataType = data.GetType();
        FieldInfo[] infos = dataType.GetFields();
        string saveKeyName = "";
        FieldInfo info;
        for(int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            saveKeyName = key + "_" + dataType.Name + "_" +
            info.FieldType.Name + "_" + info.Name;
            SaveValue(info.GetValue(data), saveKeyName);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// 递归保存单个值到 PlayerPrefs
    /// 根据值类型分别调用对应的 PlayerPrefs.SetXxx 方法
    /// </summary>
    /// <param name="value">要存储的值</param>
    /// <param name="keyname">存储键名</param>
    private void SaveValue(object value, string keyname)
    {
        Type valueType = value.GetType();
        if(valueType == typeof(int))
        {
            PlayerPrefs.SetInt(keyname, (int)value);
        }
        else if(valueType == typeof(float))
        {
            PlayerPrefs.SetFloat(keyname, (float)value);
        }
        else if(valueType == typeof(string))
        {
            PlayerPrefs.SetString(keyname, (string)value);
        }
        else if(valueType == typeof(bool))
        {
            // bool 转换为 int 存储（PlayerPrefs 不支持直接存 bool）
            PlayerPrefs.SetInt(keyname, (bool)value ? 1 : 0);
        }
        else if(typeof(IList).IsAssignableFrom(valueType))
        {
            // 列表：先存数量，再逐项递归存储
            IList list = (IList)value;
            PlayerPrefs.SetInt(keyname + "_count", list.Count);
            int index = 0;
            foreach(object item in list)
            {
                SaveValue(item, keyname + "_" + index);
                index++;
            }
        }
        else if(typeof(IDictionary).IsAssignableFrom(valueType))
        {
            // 字典：先存数量，再存 key-value 对
            IDictionary dic = (IDictionary)value;
            PlayerPrefs.SetInt(keyname + "_count", dic.Count);
            int index = 0;
            foreach(object item in dic.Keys)
            {
                SaveValue(item, keyname + "_key_" + index);
                SaveValue(dic[item], keyname + "_" + index);
                index++;
            }
        }
        else
        {
            // 嵌套对象：递归调用 SaveData
            SaveData(value, keyname);
        }
    }

    /// <summary>
    /// 从 PlayerPrefs 加载数据并填充到新对象中
    /// </summary>
    /// <param name="type">目标类型</param>
    /// <param name="KeyName">存储键名前缀</param>
    /// <returns>加载并填充好的对象</returns>
    public object LoadData(Type type, string KeyName)
    {
        object dataType = Activator.CreateInstance(type);
        FieldInfo[] infos = type.GetFields();
        FieldInfo info;
        string loadKeyName = "";
        for(int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            loadKeyName = KeyName + "_" + type.Name + "_" +
            info.FieldType.Name + "_" + info.Name;
            info.SetValue(dataType, LoadValue(info.FieldType, loadKeyName));
        }
        return dataType;
    }

    /// <summary>
    /// 递归从 PlayerPrefs 读取单个值
    /// 根据字段类型分别调用对应的 PlayerPrefs.GetXxx 方法
    /// </summary>
    /// <param name="fieldType">字段类型</param>
    /// <param name="keyname">存储键名</param>
    /// <returns>读取到的值</returns>
    private object LoadValue(Type fieldType, string keyname)
    {
        if(fieldType == typeof(int))
        {
            return PlayerPrefs.GetInt(keyname);
        }
        else if(fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyname);
        }
        else if(fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(keyname);
        }
        else if(fieldType == typeof(bool))
        {
            // 从 int 还原 bool
            return PlayerPrefs.GetInt(keyname) == 1 ? true : false;
        }
        else if(typeof(IList).IsAssignableFrom(fieldType))
        {
            // 列表：读取数量，逐项递归加载
            int count = PlayerPrefs.GetInt(keyname + "_count");
            IList list = (IList)Activator.CreateInstance(fieldType);
            Type itemType = fieldType.GetGenericArguments()[0];
            for(int i = 0; i < count; i++)
            {
                list.Add(LoadValue(itemType, keyname + "_" + i));
            }
            return list;
        }
        else if(typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            // 字典：读取数量，逐项加载 key-value
            int count = PlayerPrefs.GetInt(keyname + "_count");
            IDictionary dic = (IDictionary)Activator.CreateInstance(fieldType);
            Type[] types = fieldType.GetGenericArguments();
            Type keyType = types[0];
            Type valueType = types[1];
            for(int i = 0; i < count; i++)
            {
                object key = LoadValue(keyType, keyname + "_key_" + i);
                object value = LoadValue(valueType, keyname + "_" + i);
                dic.Add(key, value);
            }
            return dic;
        }
        else
        {
            // 嵌套对象：递归调用 LoadData
            return LoadData(fieldType, keyname);
        }
    }
}
