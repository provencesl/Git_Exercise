/**
 *	Title: "SOAP" UI框架
 *      定义所有UI窗体的父类
 *      定义4个生命周期状态
 *      1.Display   显示
 *      2.Hiding    隐藏
 *      3.ReDisplay 再显示
 *      4.Freeze    冻结
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class BaseUIForm : MonoBehaviour
    {
        //字段
        private UIType _CurrentUIType = new UIType();
        //属性，当前UI窗体类型
        public UIType CurrentUIType
        {
            get
            {
                return _CurrentUIType;
            }

            set
            {
                _CurrentUIType = value;
            }
        }

        #region 窗体的四种（生命周期）状态
        public virtual void Display()
        {
            gameObject.SetActive(true);
            //设置模态窗体调用必须是弹出窗体
            if (_CurrentUIType.UIForms_Type == UIFormType.Popup)
            {
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _CurrentUIType.UIForms_LuccencyType);
            }
        }
        public virtual void Hiding()
        {
            gameObject.SetActive(false);
            //取消模态窗体调用必须是弹出窗体
            if (_CurrentUIType.UIForms_Type == UIFormType.Popup)
            {
                UIMaskMgr.GetInstance().CancelMaskWindow();
            }
        }
        public virtual void ReDisplay()
        {
            gameObject.SetActive(true);
            //设置模态窗体调用必须是弹出窗体
            if (_CurrentUIType.UIForms_Type == UIFormType.Popup)
            {
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _CurrentUIType.UIForms_LuccencyType);
            }
        }
        public virtual void Freeze()
        {
            gameObject.SetActive(true);
        }
        #endregion

        #region 封装子类常用的方法
        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="delHandle">方法</param>
        protected void RegisterButtonObjectEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            //查找节点
            GameObject traLogonSysButton = UnityHelper.FindTheChildNode(this.gameObject, buttonName).gameObject;
            //给按钮注册事件
            if (traLogonSysButton != null)
            {
                EventTriggerListener.Get(traLogonSysButton).onClick = delHandle;
            }
        }
        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormName">User interface form name.</param>
        protected void OpenUIForm(string uiFormName)
        {
            UIManager.GetInstance().ShowUIForms(uiFormName);
        }
        /// <summary>
        /// 关闭当前UI窗体(不需要传递参数)
        /// </summary>
        protected void CloseUIForm()
        {
            string strUIFormName = string.Empty;
            int intPosition = -1;
            strUIFormName = GetType().ToString();   //当前命名空间+类名
            intPosition = strUIFormName.IndexOf('.');
            if(intPosition != -1)
            {
                strUIFormName = strUIFormName.Substring(intPosition + 1);
            }
            UIManager.GetInstance().CloseUIForms(strUIFormName);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType">消息的分类</param>
        /// <param name="msgName">消息的名称</param>
        /// <param name="msgContent">消息的内容</param>
        protected void SendMessage(string msgType, string msgName,object msgContent)
        {
            //传递数据
            KeyValueUpdate kvs = new KeyValueUpdate(msgName, msgContent);
            MessageCenter.SendMessage(msgType, kvs);
        }
        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="msgType">消息的分类</param>
        /// <param name="handler">消息委托</param>
        protected void ReceiveMessage(string msgType,MessageCenter.DelMessageDelivery handler)
        {
            MessageCenter.AddMsgListener(msgType, handler);
        }
        /// <summary>
        /// 显示语言，//通过配置文件读取对应版本的语言文字
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="id">Identifier.</param>
        protected string Show(string id)
        {
            string strResult = string.Empty;
            strResult = LauguageMgr.GetInstance().ShowText(id); 
            return strResult;
        }

        #endregion
    }
}

