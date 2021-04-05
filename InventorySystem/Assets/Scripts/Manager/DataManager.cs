// author:KIPKIPS
// time:2021.04.05 21:24:35
// describe:数据管理类
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class DataManager : BaseSingleton<DataManager> {
    public T LoadJsonByPath<T>(string path) {
        string filePath = Application.dataPath + "/" + path;
        //print(filePath);
        //读取文件
        StreamReader reader = new StreamReader(filePath);
        string jsonStr = reader.ReadToEnd();
        reader.Close();
        //Debug.Log(jsonStr);
        //字符串转换为DataSave对象
        T data = JsonConvert.DeserializeObject<T>(jsonStr);
        return data;
    }
}
