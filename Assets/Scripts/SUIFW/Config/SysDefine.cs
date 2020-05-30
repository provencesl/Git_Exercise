/**
 *	Title: "SOAP" UI框架
 *  框架核心参数
 *  Description:
 *          1：  系统常量
 *          2：  全局性方法
 *          3：  系统枚举类型
 *          4：  委托定义
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SUIFW
{
    #region 系统枚举类型

    /// <summary>
    /// UI窗体（位置）类型
    /// </summary>
    public enum UIFormType
    {
        //普通窗体
        Normal,
        //固定窗体
        Fixed,
        //弹出窗体
        Popup
    }

    /// <summary>
    /// UI窗体的显示类型
    /// </summary>
    public enum UIFormShowMode
    {
        //普通
        Normal,
        //反向切换,父窗体冻结
        ReverseChange,
        //隐藏其他
        HideOther
    }

    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public enum UIFormLucencyType
    {
        //完全透明,不能穿透
        Lucency,
        //半透明，不能穿透
        Translucency,
        //低透明，不能穿透
        ImPenetrable,
        //可以穿透
        Pentrate
    }
    #endregion

    public class SysDefine : MonoBehaviour
    {
        /* 语言文件常量 */
        public const string SYS_LANGUAGE_JSON_CONFIG = "LauguageJSONConfig";

        /* 路径常量 */
        public const string SYS_PATH_CANVAS = "Canvas";
        public const string SYS_PATH_UIFORMS_CONFIG_INFO = "UIFormsConfigInfo";
        public const string SYS_PATH_SysConfigJson = "SysConfigInfo";
        /* 标签常量 */
        public const string SYS_TAG_CANVAS = "_TagCanvas";
        public const string SYS_TAG_UICAMERA = "_TagUICamera";

        /* 节点常量 */
        public const string SYS_NORMAL_NODE = "Normal";
        public const string SYS_FIXED_NODE = "Fixed";
        public const string SYS_POPUP_NODE = "Popup";
        public const string SYS_SCRIPTMANAGER_NODE = "_ScriptMgr";

        /* 摄像机层深的常量 */
        /* 全局性的方法 */

        /* 委托的定义 */

    }
}

