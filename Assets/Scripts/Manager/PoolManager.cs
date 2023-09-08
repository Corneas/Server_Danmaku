using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class PoolManager
{
    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        Application.quitting += () => { OnExiting?.Invoke(); };

        SceneManager.sceneUnloaded += _ => { OnSceneUnloaded?.Invoke(); };
    }

    private static event Action OnExiting;
    private static event Action OnSceneUnloaded;

    /// <summary>
    ///     ������Ʈ�� Ǯ���� �����ɴϴ�.
    /// </summary>
    /// <param name="prefab">������</param>
    /// <param name="parent">�θ�</param>
    /// <typeparam name="T">������Ʈ Ÿ��</typeparam>
    /// <returns>Ǯ���� ������Ʈ</returns>
    public static T Get<T>(T prefab, Transform parent = null) where T : Object
    {
        var pool = Pool<T>.Instance;
        var obj = pool.Get(prefab);
        if (obj is Component component)
            component.transform.SetParent(parent, false);
        if (obj is GameObject gameObject)
            gameObject.transform.SetParent(parent, false);
        return obj;
    }

    /// <summary>
    ///     ������Ʈ�� Ǯ�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="obj">��ȯ�� ������Ʈ</param>
    /// <typeparam name="T">������Ʈ Ÿ��</typeparam>
    public static void Release<T>(T obj) where T : Object
    {
        var pool = Pool<T>.Instance;
        pool.Release(obj);
    }

    private class Pool<T> where T : Object
    {
        private readonly Dictionary<T, T> _prefabs = new();
        private readonly Dictionary<T, Stack<T>> _stacks = new();

        static Pool()
        {
            OnExiting += Instance.Clear;
            OnSceneUnloaded += Instance.Clear;
            Debug.Log("PoolManager Initialized" + typeof(T));
        }

        public static Pool<T> Instance { get; } = new();

        public T Get(T prefab)
        {
            if (!_stacks.TryGetValue(prefab, out var stack))
            {
                stack = new Stack<T>();
                _stacks.Add(prefab, stack);
                _prefabs.Add(prefab, prefab);
            }

            if (stack.Count > 0)
            {
                var obj = stack.Pop();
                if (obj is Component component)
                    component.gameObject.SetActive(true);
                if (obj is GameObject gameObject)
                    gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var obj = Object.Instantiate(prefab);
                _prefabs.Add(obj, prefab);
                return obj;
            }
        }

        public void Release(T obj)
        {
            GameObject gameObject = null;

            if (obj is GameObject @object)
                gameObject = @object;
            else if (obj is Component component)
                gameObject = component.gameObject;

            if (!_prefabs.TryGetValue(obj, out var prefab))
            {
                Object.Destroy(gameObject);
                return;
            }

            gameObject.SetActive(false);
            _stacks[prefab].Push(obj);
        }

        private void Clear()
        {
            foreach (var obj in _stacks.Values.SelectMany(stack => stack))
            {
                GameObject gameObject = null;

                if (obj is GameObject @object)
                    gameObject = @object;
                else if (obj is Component component)
                    gameObject = component.gameObject;

                Object.Destroy(gameObject);
            };

            _stacks.Clear();
            _prefabs.Clear();
        }
    }
}
