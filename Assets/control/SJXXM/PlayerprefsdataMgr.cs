using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerprefsdataMgr 
{
    
    static PlayerprefsdataMgr instance = new PlayerprefsdataMgr();

    public static PlayerprefsdataMgr Instance
    {
        get
        {
            return instance;  
        }
    }


    private PlayerprefsdataMgr()
    {

    }


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
            PlayerPrefs.SetInt(keyname, (bool)value ? 1 : 0);
        }
        else if(typeof(IList).IsAssignableFrom(valueType))
        {
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
            IDictionary dic = (IDictionary)value;
            PlayerPrefs.SetInt(keyname + "_count", dic.Count);
            int index = 0;
            foreach(object item in dic.Keys)
            {
                //keyname + "_key_" + index是再一次传入的keyname 进入上面四个类型进行存储
                SaveValue(item, keyname + "_key_" + index);
                SaveValue(dic[item], keyname + "_" + index);
                index++;
            }
        }
        else
        {
            SaveData(value, keyname);
        }
    }



    public object LoadData(Type type,string KeyName)
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
            return PlayerPrefs.GetInt(keyname) == 1 ? true : false;
        }
        else if(typeof(IList).IsAssignableFrom(fieldType))
        {
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
            return LoadData(fieldType, keyname);
        }
    }

  








}
