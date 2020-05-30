/**
 *	Title: "SOAP" UI框架
 *
 *  Description:
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;
namespace DemoProject
{
    public class MarketUIForm : BaseUIForm
    {

        private void Awake()
        {
            //定义本窗体的性质(默认数值)
            base.CurrentUIType.UIForms_Type = UIFormType.Popup; 
            base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange;
            base.CurrentUIType.UIForms_LuccencyType = UIFormLucencyType.Translucency;
            //注册按钮事件
            //RegisterButtonObjectEvent("Btn_close", (go) => CloseUIForm());//使用lamda表达式写法
            RegisterButtonObjectEvent("Btn_close",
                p =>
                {
                    CloseUIForm();
                }
            );
            RegisterButtonObjectEvent("BtnTicket",
                p =>
                {
                    //打开子窗体
                    OpenUIForm(ProConsts.PROP_DETAIL_FORMS);
                    //传递数据
                    string[] strArr = new string[] { "11", "2222" }
;                    SendMessage("Props", "ticket", strArr);//传递多个数据
                }
            );
            RegisterButtonObjectEvent("BtnShoe",
                p =>
                {
                    //打开子窗体
                    OpenUIForm(ProConsts.PROP_DETAIL_FORMS);
                    //传递数据
                    string[] strArr = new string[] { "3333", "4444" };
                    SendMessage("Props", "shoe", strArr);
                }
            );

        }
    }
}
