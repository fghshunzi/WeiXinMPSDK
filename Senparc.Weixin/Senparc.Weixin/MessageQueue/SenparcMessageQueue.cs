﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Weixin.MessageQueue
{
    /// <summary>
    /// 消息列队
    /// </summary>
    public class SenparcMessageQueue
    {
        private static Dictionary<string, SenparcMessageQueueItem> MessageQueueDictionary = new Dictionary<string, SenparcMessageQueueItem>(StringComparer.OrdinalIgnoreCase);
        private static List<string> MessageQueueList = new List<string>();

        //static SenparcMessageQueue()
        //{

        //}

        /// <summary>
        /// 同步执行锁
        /// </summary>
        public static object MessageQueueSyncLock = new object();

        /// <summary>
        /// 获取当前等待执行的Key
        /// </summary>
        /// <returns></returns>
        public string GetCurrentKey()
        {
            lock (MessageQueueSyncLock)
            {
                return MessageQueueList.FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取SenparcMessageQueueItem
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SenparcMessageQueueItem GetItem(string key)
        {
            lock (MessageQueueSyncLock)
            {
                if (MessageQueueDictionary.ContainsKey(key))
                {
                    return MessageQueueDictionary[key];
                }
                return null;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public SenparcMessageQueueItem Add(string key, Action action)
        {
            lock (MessageQueueSyncLock)
            {
                if (!MessageQueueDictionary.ContainsKey(key))
                {
                    MessageQueueList.Add(key);
                }
                //else
                //{
                //    MessageQueueList.Remove(key);
                //    MessageQueueList.Add(key);//移动到末尾
                //}

                var mqItem =new  SenparcMessageQueueItem(key,action);
                MessageQueueDictionary[key] = mqItem;
                return mqItem;
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            lock (MessageQueueSyncLock)
            {
                if (MessageQueueDictionary.ContainsKey(key))
                {
                    MessageQueueDictionary.Remove(key);
                    MessageQueueList.Remove(key);
                }
            }
        }

        /// <summary>
        /// 获得当前列队数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            lock (MessageQueueSyncLock)
            {
                return MessageQueueList.Count;
            }
        }
    }
}
