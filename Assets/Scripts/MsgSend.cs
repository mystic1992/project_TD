/********************************************************************
  filename: MsgSend
  author: Mario Chen
  purpose:  消息发送模块
  Tips:
*********************************************************************/



using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

    public delegate void MsgCallBack(object _objs);
    /// <summary>
    /// 在List的Contains()、Dictionary的ContainsKey()或[]操作时，会调用Equals()和GetHasCode()，从而对Enum或Struct类型进行装箱操作，产生GC问题
    /// 实现IEqualityComparer 防止装箱操作,解决GC问题
    /// </summary>
    public class MsgTypeEnumComparer : IEqualityComparer<MsgType> {
        public bool Equals(MsgType x, MsgType y) {
            return (int)x == (int)y;
        }

        public int GetHashCode(MsgType obj) {
            return (int)obj;
        }
    }

    /// <summary>
    /// 窗口消息
    /// </summary>
    public class MsgSend {
        private static MsgSend _gInstance;

        private static MsgSend Instance {
            get {
                if (_gInstance == null) {
                    _gInstance = new MsgSend();
                }

                return _gInstance;
            }
        }

        private readonly Dictionary<MsgType, List<MsgCallBack>> _m_broadcastTable;

        MsgSend() {
            var equalityComparer = new MsgTypeEnumComparer();
            _m_broadcastTable = new Dictionary<MsgType, List<MsgCallBack>>(equalityComparer);
        }

        /// <summary>
        /// 广播
        /// </summary>
        public static void SendMsg(MsgType _type, object _data) {
            Instance.BaseSendMsg(_type, _data);
        }

        /// <summary>
        /// 注册广播事件
        /// </summary>
        public static void RegisterMsg(MsgType _type, MsgCallBack _callback) {
            Instance.BaseRegisterMsg(_type, _callback);
        }

        /// <summary>
        /// 反注册广播事件
        /// </summary>
        public static void UnregisterMsg(MsgType _type, MsgCallBack _callback) {
            Instance.BaseUnregisterMsg(_type, _callback);
        }

        /// <summary>
        /// 注册广播事件
        /// </summary>
        /// <param name="_type">广播类型</param>
        /// <param name="_callback">广播回调</param>
        void BaseRegisterMsg(MsgType _type, MsgCallBack _callback) {
            List<MsgCallBack> broadcast;
            if (!_m_broadcastTable.TryGetValue(_type, out broadcast)) {
                broadcast = new List<MsgCallBack>();
                _m_broadcastTable[_type] = broadcast;
            }

            if (!broadcast.Contains(_callback)) {
                broadcast.Add(_callback);
            }
        }

        /// <summary>
        /// 反注册广播事件
        /// </summary>
        /// <param name="_type">反注册类型</param>
        /// <param name="_callback">反注册广播事件</param>
        void BaseUnregisterMsg(MsgType _type, MsgCallBack _callback) {
            List<MsgCallBack> broadcast;
            if (!_m_broadcastTable.TryGetValue(_type, out broadcast)) {
                return;
            }

            if (broadcast.Contains(_callback)) {
                broadcast.Remove(_callback);
            }
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="_type">广播类型</param>
        /// <param name="_data">广播不定参</param>
        void BaseSendMsg(MsgType _type, object _data) {
            List<MsgCallBack> broadcast;
            if (_m_broadcastTable.TryGetValue(_type, out broadcast)) {
                for (int i = broadcast.Count - 1; i >= 0; i--) {
                    try {
                        if (i < broadcast.Count) {
                            broadcast[i](_data);
                        }

                    }
                    catch (Exception e) {
                        Debug.LogError("处理业务消息时出错!" + _type.ToString());
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }

    public enum MsgType {
        None,
    }
}