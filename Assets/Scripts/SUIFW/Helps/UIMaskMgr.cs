/**
 *	Title: "SOAP" UI框架
 *  UI遮罩管理器
 *  Description:负责弹出窗体模态显示
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUIFW
{
    public class UIMaskMgr : MonoBehaviour {
        /* 字段 */

        //本脚本私有单例
        private static UIMaskMgr _Instance = null;

        //UI根节点对象
        private GameObject _GoCanvasRoot = null;
        //UI脚本节点对象
        private Transform _TraUIScriptsNode = null;
        //顶层面板
        private GameObject _GoTopPanel;
        //遮罩面板
        private GameObject _GoMaskPanel;
        //UI摄像机
        private Camera _UICamera;
        //UI摄像机 “层深”
        private float _OriginalUICameraDepth;

        /// <summary>
        /// 得到实例
        /// </summary>
        /// <returns>The instance.</returns>
        public static UIMaskMgr GetInstance()
        {
            if(_Instance == null)
            {
                _Instance = new GameObject("_UIMaskMgr").AddComponent<UIMaskMgr>();
            }
            return _Instance;
        }
        private void Awake()
        {
            //得到UI根节点对象、脚本节点对象
            _GoCanvasRoot = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS);
            _TraUIScriptsNode = UnityHelper.FindTheChildNode(_GoCanvasRoot, SysDefine.SYS_SCRIPTMANAGER_NODE);
            //把本脚本实例，作为“脚本节点对象”的子节点
            UnityHelper.AddChildNodeToParentNode(_TraUIScriptsNode,this.gameObject.transform);

            _GoTopPanel = _GoCanvasRoot;
            _GoMaskPanel = UnityHelper.FindTheChildNode(_GoCanvasRoot, "_UIMaskPanel").gameObject;
            _UICamera = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_UICAMERA).GetComponent<Camera>();
            if(_UICamera != null)
            {
                _OriginalUICameraDepth = _UICamera.depth;
            }
            else
            {
                Debug.Log(GetType() + "/start()/UICamera is null");
            }
        }
        /// <summary>
        /// 设置遮罩状态
        /// </summary>
        /// <param name="goDisplayUIForms">显示窗体</param>
        /// <param name="lucencyType">显示透明度属性</param>
        public void SetMaskWindow(GameObject goDisplayUIForms, UIFormLucencyType lucencyType = UIFormLucencyType.Lucency)
        {
            //顶层窗体下移
            _GoTopPanel.transform.SetAsLastSibling();
            //启用遮罩窗体以及设置透明度
            switch (lucencyType)
            {
                //完全透明，不能穿透
                case UIFormLucencyType.Lucency:
                    _GoMaskPanel.SetActive(true);
                    Color color = new Color(255 / 255F, 255 / 255F, 255 / 255F, 0 / 255F);  //遮罩窗体颜色 可以自行调整
                    _GoMaskPanel.GetComponent<Image>().color = color;
                    break;
                //半透明 不能穿透
                case UIFormLucencyType.Translucency:
                    _GoMaskPanel.SetActive(true);
                    Color color2 = new Color(220 / 255F, 220 / 255F, 220 / 255F, 50 / 255F);
                    _GoMaskPanel.GetComponent<Image>().color = color2;
                    break;
                //低透明 不能穿透
                case UIFormLucencyType.ImPenetrable:
                    _GoMaskPanel.SetActive(true);
                    Color color3 = new Color(50 / 255F, 50 / 255F, 50 / 255F, 200 / 255F);
                    _GoMaskPanel.GetComponent<Image>().color = color3;
                    break;
                //可以穿透
                case UIFormLucencyType.Pentrate:
                    _GoMaskPanel.SetActive(false);
                    break;
                default:
                    break;
            }
            //遮罩窗体下移
            _GoMaskPanel.transform.SetAsLastSibling();
            //显示窗体下移
            goDisplayUIForms.transform.SetAsLastSibling();
            //增加当前UI摄像机的层深（保证当前摄像机为最前显示）
            if (_UICamera != null)
            {
                _UICamera.depth += 100;
            }

        }
        /// <summary>
        /// 取消遮罩状态
        /// </summary>
        public void CancelMaskWindow()
        {
            //顶层窗体上移
            _GoTopPanel.transform.SetAsFirstSibling();
            //禁用遮罩窗体
            if (_GoMaskPanel.activeInHierarchy)
            {
                _GoMaskPanel.SetActive(false);
            }
            //恢复当前UI摄像机的层深
            if (_UICamera != null)
            {
                _UICamera.depth = _OriginalUICameraDepth; //恢复层深
            }
        }

    }
}
