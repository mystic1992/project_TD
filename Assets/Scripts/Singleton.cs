namespace Game {

    using UnityEngine;
    using System.Collections;

    public class Singleton<T> where T : new() {

        private static T _g_instance;

        protected Singleton() {

        }

        public static T instance {
            get {
                if (_g_instance == null) {
                    _g_instance = new T();
                }
                return _g_instance;
            }
        }

    }

}