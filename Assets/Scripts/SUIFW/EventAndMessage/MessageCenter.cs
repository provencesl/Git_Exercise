/**
 *	Title: "SOAP" UI框架
 *  消息传递中心
 *  Description:负责UI框架中，所有UI窗体中间的数据传值。
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class MessageCenter {
        /// <summary>
        /// 委托 ：消息传递
        /// </summary>
        public delegate void DelMessageDelivery(KeyValueUpdate kv);
        /// <summary>
        /// 消息中心缓存集合
        /// string：监听类型，DelMessageDelivery：数据执行委托
        /// </summary>
        public static Dictionary<string, DelMessageDelivery> _dicMessages = new Dictionary<string, DelMessageDelivery>();

        /// <summary>
        /// 添加消息的监听
        /// </summary>
        /// <param name="messageType">消息的分类</param>
        /// <param name="handler">消息的委托</param>
        public static void AddMsgListener(string messageType, DelMessageDelivery handler)
        {
            if (!_dicMessages.ContainsKey(messageType))
            {
                _dicMessages.Add(messageType, null);
            }
            _dicMessages[messageType] += handler;
        }
        /// <summary>
        /// 移除消息的监听
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="handler">Handler.</param>
        public static void RemoveMsgListener(string messageType, DelMessageDelivery handler)
        {
            if (_dicMessages.ContainsKey(messageType))
            {
                _dicMessages.Remove(messageType);
            }
        }
        /// <summary>
        /// 取消所有消息监听
        /// </summary>
        public static void ClearAllMsgListener()
        {
            if(_dicMessages != null)
            {
                _dicMessages.Clear();
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageType">消息的分类</param>
        /// <param name="kv">键值对对象</param>
        public static void SendMessage(string messageType,KeyValueUpdate kv)
        {
            DelMessageDelivery del; //委托
            if(_dicMessages.TryGetValue(messageType, out del))
            {
                if(del != null)
                {
                    del(kv);    //调用委托
                }
            }
        }

    }
    /// <summary>
    /// 键值更新对
    /// 配合委托，实现委托数据传递
    /// </summary>
    public class KeyValueUpdate
    {
        //键
        public string _Key = null;
        //值
        public object _Values = null;
        //只读属性
        public string Key
        {
            get
            {
                return _Key;
            }
        }

        public object Values
        {
            get
            {
                return _Values;
            }
        }
        public KeyValueUpdate(string key, object valueObj)
        {
            _Key = key;
            _Values = valueObj;
        }
    }
}
