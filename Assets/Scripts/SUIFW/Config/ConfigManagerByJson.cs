/**
 *	Title: "SOAP" UI框架
 *
 *  Description:基于Json配置文件的“配置管理器”
 *  
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class ConfigManagerByJson : IConfigManager {
        //保存（键值对）集合
        private static Dictionary<string, string> _AppSetting;

        /// <summary>
        /// 得到应用设置（键值组合），只读属性
        /// </summary>
        /// <value>The app setting.</value>
        public Dictionary<string, string> AppSetting
        {
            get
            {
                return _AppSetting;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <returns>The app setting max number.</returns>
        /// <param name="JsonPath">JJson配置文件路径</param>
        public ConfigManagerByJson(string JsonPath)
        {
            _AppSetting = new Dictionary<string, string>();
            InitAndAnalysisJson(JsonPath);
            //初始化解析json数据，加载到_AppSetting集合中
        }
        /// <summary>
        /// 得到AppSetting的最大数值
        /// </summary>
        /// <returns>The app setting max number.</returns>
        public int GetAppSettingMaxNumber()
        {
            if(_AppSetting != null && _AppSetting.Count >= 1)
            {
                return _AppSetting.Count;
            }
            return 0;
        }
        /// <summary>
        /// 初始化解析Json数据，加载到集合中
        /// </summary>
        /// <param name="JsonPath">Json path.</param>
        private void InitAndAnalysisJson(string JsonPath)
        {
            TextAsset configInfo = null;
            KeyValueInfo keyValueInfo = null;
            if (string.IsNullOrEmpty(JsonPath)) return;
            //解析
            try
            {
                configInfo = Resources.Load<TextAsset>(JsonPath);
                keyValueInfo = JsonUtility.FromJson<KeyValueInfo>(configInfo.text);
            }
            catch (Exception)
            {
                //抛出自定义异常
                throw new JsonAnalysisException(GetType() + "InitAndAnalysisJson()/Json Analysis Exception !Parameter JsonPath=" + JsonPath);
            }
            foreach (KeyValueNode nodeInfo in keyValueInfo.ConfigInfo)
            {
                _AppSetting.Add(nodeInfo.Key, nodeInfo.Value);
            }
        }
    }
}
