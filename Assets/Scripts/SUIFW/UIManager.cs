/**
 *	Title: "SOAP" UI框架
 *          UI管理器
 *          整个UI框架的核心，用户通过本脚本，来实现框架的绝大部分功能
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _Instance = null;
        //UI窗体预设路径 1.窗体预设名称，2.窗体预设路径
        private Dictionary<string, string> _DicFormsPaths;
        //缓存所有UI窗体
        private Dictionary<string, BaseUIForm> _DicAllUIForms;
        //当前显示UI窗体
        private Dictionary<string, BaseUIForm> _DicCurrentShowUIForms;
        //定义栈的集合,显示所有“反向切换”属性窗体
        private Stack<BaseUIForm> _StaCurrentUIForms;
        //UI根节点
        private Transform _TraCanvasTransform = null;
        //全屏幕显示的节点
        private Transform _TraNormal = null;
        //固定显示的节点
        private Transform _TraFixed = null;
        //弹出节点
        private Transform _TraPopup = null;
        //UI管理脚本的节点
        private Transform _TraUIScripts = null;

        /// <summary>
        /// 得到实例
        /// </summary>
        /// <returns>The UIM anager.</returns>
        public static UIManager GetInstance()
        {
            if(_Instance == null)
            {
                _Instance = new GameObject("_UIManager").AddComponent<UIManager>();
            }
            return _Instance;
        }
        //初始化核心数据 加载“UI窗体路径” 到 集合中。
        private void Awake()
        {
            //字段初始化
            _DicAllUIForms = new Dictionary<string, BaseUIForm>();
            _DicCurrentShowUIForms = new Dictionary<string, BaseUIForm>();
            _DicFormsPaths = new Dictionary<string, string>();
            _StaCurrentUIForms = new Stack<BaseUIForm>();
            //初始化加载（根UI窗体）Canvas预设
            InitRootCanvasLoading();
            //得到UI根节点、全屏节点、固定节点、弹出节点
            _TraCanvasTransform = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;
            _TraNormal = UnityHelper.FindTheChildNode(_TraCanvasTransform.gameObject, SysDefine.SYS_NORMAL_NODE);
            _TraFixed = UnityHelper.FindTheChildNode(_TraCanvasTransform.gameObject, SysDefine.SYS_FIXED_NODE);
            _TraPopup = UnityHelper.FindTheChildNode(_TraCanvasTransform.gameObject, SysDefine.SYS_POPUP_NODE);
            _TraUIScripts = UnityHelper.FindTheChildNode(_TraCanvasTransform.gameObject, SysDefine.SYS_SCRIPTMANAGER_NODE);

            //把本脚本作为“根UI窗体”的子节点。
            this.gameObject.transform.SetParent(_TraUIScripts, false);
            //this.gameObject.transform.SetParent(_TraUIScripts, false);
            //"根UI窗体"在场景转换的时候，不允许销毁
            DontDestroyOnLoad(_TraCanvasTransform);
            //初始化“UI窗体预设”路径数据
            InitUIFormsPathData();
        }
        /// <summary>
        /// 初始化“UI窗体预设”路径数据
        /// </summary>
        private void InitUIFormsPathData()
        {
            IConfigManager configManager = new ConfigManagerByJson(SysDefine.SYS_PATH_UIFORMS_CONFIG_INFO);
            if(configManager != null)
            {
                _DicFormsPaths = configManager.AppSetting;
            }
        }

        /// <summary>
        /// 显示（打开）UI窗体
        /// 1: 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        /// 2: 根据不同的UI窗体的“显示模式”，分别作不同的加载处理
        /// </summary>
        /// <param name="uiFormName">UI窗体预设的名称</param>
        public void ShowUIForms(string uiFormName)
        {
            BaseUIForm baseUIForm = null;  //UI窗体基类
            if (string.IsNullOrEmpty(uiFormName)) return;
            baseUIForm = LoadFormsToAllUIFormsCatch(uiFormName);
            if (baseUIForm == null) return;
            //是否清空栈集合中的数据
            if (baseUIForm.CurrentUIType.IsClearStack)
            {
                ClearStackArray();
            }
            switch (baseUIForm.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:
                    LoadUIToCurrentCache(uiFormName);
                    break;
                case UIFormShowMode.ReverseChange:
                    PushUIFormToStack(uiFormName);
                    break;
                case UIFormShowMode.HideOther:
                    EnterUIFormsAndHideOther(uiFormName);
                    break;
                default:
                    break;
            }
        }

        #region 显示"UI管理器"内部核心数据, 测试使用
        //显示所有UI窗体的数量
        public int ShowAllUIFormCount()
        {
            return _DicAllUIForms != null ? _DicAllUIForms.Count : 0;
        }
        //显示所有当前显示的窗体数量
        public int ShowCurrentUIFormsCount()
        {
            return _DicCurrentShowUIForms != null ? _DicCurrentShowUIForms.Count : 0;
        }
        //显示栈中窗体数量
        public int ShowCurrentStackUIFormsCount()
        {
            return _StaCurrentUIForms != null ? _StaCurrentUIForms.Count : 0;
        }
        #endregion

        /// <summary>
        /// 关闭（返回上一个）窗体
        /// </summary>
        /// <param name="uiFormName">User interface form name.</param>
        public void CloseUIForms(string uiFormName)
        {
            BaseUIForm baseUIForm;
            if (string.IsNullOrEmpty(uiFormName)) return;
            _DicAllUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm == null) return;
            switch (baseUIForm.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:
                    ExitUIForms(uiFormName);
                    //普通窗体的关闭
                    break;
                case UIFormShowMode.ReverseChange:
                    //反向切换窗体的关闭
                    PropUIForm();
                    break;
                    //隐藏其他窗体的关闭
                case UIFormShowMode.HideOther:
                    ExitUIFormsAndDisplayOther(uiFormName);
                    break;
                default:
                    break;
            }

        }

        #region 私有方法
        //初始化加载根UI窗体canvas预设
        private void InitRootCanvasLoading()
        {
            ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS, false);
        }

        /// <summary>
        /// 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        /// 功能：检查所有UI窗体集合中，是否加载过，否则才加载
        /// </summary>
        /// <returns>The forms to all UIF orms catch.</returns>
        /// <param name="uiFormsName">UI窗体预设的名称</param>
        private BaseUIForm LoadFormsToAllUIFormsCatch(string uiFormsName)
        {
            BaseUIForm baseUIResult = null;     //加载的返回UI窗体基类
            _DicAllUIForms.TryGetValue(uiFormsName, out baseUIResult);
            if(baseUIResult == null)
            {
                //加载指定路径“UI窗体”
                baseUIResult = LoadUIForm(uiFormsName);
            }
            return baseUIResult;
        }

        /// <summary>
        /// 加载指定名称的“UI窗体”
        /// 功能：
        ///    1：根据“UI窗体名称”，加载预设克隆体。
        ///    2：根据不同预设克隆体中带的脚本中不同的“位置信息”，加载到“根窗体”下不同的节点。
        ///    3：隐藏刚创建的UI克隆体。
        ///    4：把克隆体，加入到“所有UI窗体”（缓存）集合中。
        /// </summary>
        /// <returns>The UIF orm.</returns>
        /// <param name="uiFormName">User interface form name.</param>
        private BaseUIForm LoadUIForm(string uiFormName)
        {
            string strUIFormPaths = null;   //UI窗体路径
            GameObject goCloneUIPrefabs = null;     //创建的UI克隆体预设
            BaseUIForm baseUIForm = null;       //窗体基类

            //根据UI窗体名称，得到对应的加载路径
            _DicFormsPaths.TryGetValue(uiFormName, out strUIFormPaths);
            //根据UI窗体名称，加载预设克隆体
            if (!string.IsNullOrEmpty(strUIFormPaths))
            {
                goCloneUIPrefabs = ResourcesMgr.GetInstance().LoadAsset(strUIFormPaths, false);
            }
            //设置UI克隆体的父节点
            if(_TraCanvasTransform != null && goCloneUIPrefabs != null)
            {
                baseUIForm = goCloneUIPrefabs.GetComponent<BaseUIForm>();
                if(baseUIForm == null)
                {
                    Debug.Log("baseUIForm == null");
                    return null;
                }
                switch (baseUIForm.CurrentUIType.UIForms_Type)
                {
                    case UIFormType.Normal:                 //普通窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraNormal, false);
                        break;
                    case UIFormType.Fixed:                  //固定窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraFixed, false);
                        break;
                    case UIFormType.Popup:                  //弹出窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraPopup, false);
                        break;
                    default:
                        break;
                }
                //设置隐藏
                goCloneUIPrefabs.SetActive(false);
                //把克隆体，加入到“所有UI窗体”（缓存）集合中。
                _DicAllUIForms.Add(uiFormName, baseUIForm);
                return baseUIForm;
            }
            else
            {
                Debug.Log("_TraCanvasTransform == null || goCloneUIPrefabs == null");
                return null;
            }

        }

        /// <summary>
        /// 把当前窗体加载到“当前窗体”集合中
        /// </summary>
        /// <param name="uiFormName">窗体预设的名称</param>
        private void LoadUIToCurrentCache(string uiFormName)
        {
            BaseUIForm baseUIForm;  //窗体基类
            BaseUIForm baseUIFormFromAllCache;  //从所有窗体集合中得到的窗体
            //如果正在显示的集合中，存在UI窗体，则直接返回
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm != null) return;
            _DicAllUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);
            if(baseUIFormFromAllCache != null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, baseUIFormFromAllCache);
                baseUIFormFromAllCache.Display();   //显示当前窗体
            }
        }
        /// <summary>
        /// UI窗体入栈
        /// </summary>
        /// <param name="uiFormName">窗体名称</param>
        private void PushUIFormToStack(string uiFormName)
        {
            BaseUIForm baseUIForm;
            if(_StaCurrentUIForms.Count > 0)
            {
                BaseUIForm topUIForm = _StaCurrentUIForms.Peek();
                topUIForm.Freeze();
            }
            _DicAllUIForms.TryGetValue(uiFormName, out baseUIForm);
            if(baseUIForm != null)
            {
                baseUIForm.Display();
                _StaCurrentUIForms.Push(baseUIForm);
            }
            else
            {
                Debug.Log("baseUIForm == null");
            }

        }
        /// <summary>
        /// 退出指定UI窗体
        /// </summary>
        /// <param name="uiFormName">User interface form name.</param>
        private void ExitUIForms(string uiFormName)
        {
            BaseUIForm baseUIForm;
            //正在显示集合中如果没有记录， 直接退出
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm == null) return;
            //指定窗体，标记为隐藏状态，且从正在显示集合中移除
            baseUIForm.Hiding();
            _DicCurrentShowUIForms.Remove(uiFormName);
        }

        //反向切换属性窗体的出栈逻辑
        private void PropUIForm()
        {
            if(_StaCurrentUIForms.Count >= 2)
            {
                BaseUIForm topUIForm = _StaCurrentUIForms.Pop();
                topUIForm.Hiding();
                //出栈后，下一个窗体做重新显示
                BaseUIForm nextUIForm = _StaCurrentUIForms.Peek();
                nextUIForm.ReDisplay();

            }
            else if(_StaCurrentUIForms.Count == 1)
            {
                //出栈处理
                BaseUIForm topUIForm = _StaCurrentUIForms.Pop();
                topUIForm.Hiding();
            }
        }
        /// <summary>
        /// 打开窗体，且隐藏其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void EnterUIFormsAndHideOther(string strUIName)
        {
            BaseUIForm baseUIForm;  //UI窗体基类
            BaseUIForm baseUIFormFromAll;   //从集合中得到的窗体基类
            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;
            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm != null) return; //如果有 直接返回

            //正在显示集合 栈集合 所有的窗体隐藏
            foreach (BaseUIForm baseUI in _DicCurrentShowUIForms.Values)
            {
                baseUI.Hiding();
            }
            foreach (BaseUIForm staUI in _StaCurrentUIForms)
            {
                staUI.Hiding();
            }

            //把当前窗体加入到正在显示集合中，并做显示处理
            _DicAllUIForms.TryGetValue(strUIName, out baseUIFormFromAll);
            if(baseUIFormFromAll != null)
            {
                baseUIFormFromAll.Display();
                _DicCurrentShowUIForms.Add(strUIName, baseUIFormFromAll);
            }
        }

        /// <summary>
        /// 关闭窗体，且显示其他窗体
        /// </summary>
        /// <param name="strUIName">打开的指定窗体名称</param>
        private void ExitUIFormsAndDisplayOther(string strUIName)
        {
            BaseUIForm baseUIForm;  //UI窗体基类
            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;
            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm == null) return; //如果有 直接返回

            //当前窗体隐藏状态，且正在显示的集合中，移除本窗体
            baseUIForm.Hiding();
            _DicCurrentShowUIForms.Remove(strUIName);

            //把“正在显示集合”与“栈集合”中的所有窗体都定义重新显示状态
            foreach (BaseUIForm baseUI in _DicCurrentShowUIForms.Values)
            {
                baseUI.ReDisplay();
            }
            foreach (BaseUIForm staUI in _StaCurrentUIForms)
            {
                staUI.ReDisplay();
            }
        }
        /// <summary>
        /// 是否清空“栈集合”中的数据
        /// </summary>
        private bool ClearStackArray()
        {
            if(_StaCurrentUIForms != null && _StaCurrentUIForms.Count >= 1){
                _StaCurrentUIForms.Clear();
                return true;
            }
            return false;
        }

        #endregion

    }
}

