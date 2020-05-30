/**
 *	Title: "SOAP" UI框架
 *  通用配置管理器接口
 *  Description: 基于“键值对”配置文件的通用解析
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SUIFW
{
    public interface IConfigManager {
        /// <summary>
        /// 只读属性，应用设置
        /// 得到键值对集合数据
        /// </summary>
        /// <value>The app setting.</value>
        Dictionary<string, string> AppSetting { get; }
        /// <summary>
        /// </summary>
        /// <returns>The app setting max number.</returns>
        int GetAppSettingMaxNumber();
    }
    [Serializable]
    internal class KeyValueInfo
    {
        //配置信息
        public List<KeyValueNode> ConfigInfo = null;
    }
    [Serializable]
    internal class KeyValueNode
    {
        //键
        public string Key = null;
        //值
        public string Value = null;
    }
}
