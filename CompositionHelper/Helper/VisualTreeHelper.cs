﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace CompositionHelper.Helper
{
    /// <summary>
    /// 提供对可视化树的各种方法的简单封装
    /// </summary>
    public static class VisualTreeHelper
    {
        /// <summary>
        /// 在父控件中查找指定类型的子控件
        /// </summary>
        /// <typeparam name="T">要查找的子控件类型</typeparam>
        /// <param name="container">父控件</param>
        /// <returns>找到的控件</returns>
        public static T FindVisualElement<T>(this DependencyObject container) where T : DependencyObject
        {
            var childQueue = new Queue<DependencyObject>();

            childQueue.Enqueue(container);

            while (childQueue.Count > 0)
            {
                var current = childQueue.Dequeue();
                T result = current as T;
                if (result != null && result != container)
                {
                    return result;
                }

                int childCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(current);

                for (int childIndex = 0; childIndex < childCount; childIndex++)
                {
                    childQueue.Enqueue(Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(current, childIndex));
                }
            }

            return null;
        }

        /// <summary>
        /// 在父控件中查找所有指定类型的子控件
        /// </summary>
        /// <typeparam name="T">要查找的子控件类型</typeparam>
        /// <param name="container">父控件</param>
        /// <returns>找到的控件</returns>
        public static List<T> FindVisualElements<T>(this DependencyObject container) where T : DependencyObject
        {
            var childQueue = new Queue<DependencyObject>();
            List<T> list = new List<T>();

            childQueue.Enqueue(container);

            while (childQueue.Count > 0)
            {
                var current = childQueue.Dequeue();
                T result = current as T;
                if (result != null && result != container)
                {
                    list.Add(result);
                }

                int childCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(current);

                for (int childIndex = 0; childIndex < childCount; childIndex++)
                {
                    childQueue.Enqueue(Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(current, childIndex));
                }
            }

            return list;
        }

        /// <summary>
        /// 在父控件中使用指定的比较器查找子控件
        /// </summary>
        /// <param name="container">父控件</param>
        /// <param name="comparer">比较器</param>
        /// <returns>找到的控件</returns>
        public static DependencyObject FindVisualElement(this DependencyObject container,
            Func<DependencyObject, bool> comparer)
        {
            var childQueue = new Queue<DependencyObject>();

            childQueue.Enqueue(container);

            while (childQueue.Count > 0)
            {
                var current = childQueue.Dequeue();

                if (current != null && current != container && comparer(current))
                {
                    return current;
                }

                int childCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(current);

                for (int childIndex = 0; childIndex < childCount; childIndex++)
                {
                    childQueue.Enqueue(Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(current, childIndex));
                }
            }

            return null;
        }

        /// <summary>
        /// 在父控件中使用指定的比较器查找所有子控件
        /// </summary>
        /// <param name="container">父控件</param>
        /// <param name="comparer">比较器</param>
        /// <returns>找到的控件</returns>
        public static List<DependencyObject> FindVisualElements(this DependencyObject container,
            Func<DependencyObject, bool> comparer)
        {
            var childQueue = new Queue<DependencyObject>();
            List<DependencyObject> list = new List<DependencyObject>();

            childQueue.Enqueue(container);

            while (childQueue.Count > 0)
            {
                var current = childQueue.Dequeue();
                if (current != null && current != container && comparer(current))
                {
                    list.Add(current);
                }

                int childCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(current);

                for (int childIndex = 0; childIndex < childCount; childIndex++)
                {
                    childQueue.Enqueue(Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(current, childIndex));
                }
            }

            return list;
        }

        /// <summary>
        /// 在父控件中查找指定名字的子控件
        /// </summary>
        /// <param name="container">父控件</param>
        /// <param name="name">子控件名字</param>
        /// <returns>找到的控件</returns>
        public static FrameworkElement FindVisualElementFromName(this FrameworkElement container, String name)
        {
            if (name == null) return null;

            var childQueue = new Queue<FrameworkElement>();

            childQueue.Enqueue(container);

            while (childQueue.Count > 0)
            {
                var current = childQueue.Dequeue();
                FrameworkElement result = current;
                if (result != null && result != container && result.Name == name)
                {
                    return result;
                }

                int childCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(current);

                for (int childIndex = 0; childIndex < childCount; childIndex++)
                {
                    childQueue.Enqueue(
                        Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(current, childIndex) as FrameworkElement);
                }
            }

            return null;
        }

        /// <summary>
        /// 在子控件中查找指定类型的父控件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static DependencyObject FindParent<T>(this DependencyObject child)
        {
            DependencyObject parent = null;

            do
            {
                parent = Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(child);

                if (parent is T)
                {
                    break;
                }
            } while (parent != null);

            return parent;
        }

        /// <summary>
        /// 在子控件中查找指定名称的父控件。
        /// </summary>
        /// <param name="child"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DependencyObject FindParentFormName(this DependencyObject child, String name)
        {
            if (name == null) return null;

            DependencyObject parent = null;

            do
            {
                parent = Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(child);

                if ((parent as FrameworkElement)?.Name == name)
                {
                    break;
                }
            } while (parent != null);

            return parent;
        }

        /// <summary>
        /// 获取窗口所有打开的 <see cref="Popup"/>
        /// </summary>
        /// <param name="window">窗口</param>
        /// <returns>Popup 列表</returns>
        public static IReadOnlyList<Popup> GetOpenPopups(this Window window)
        {
            return Windows.UI.Xaml.Media.VisualTreeHelper.GetOpenPopups(window);
        }
    }
}