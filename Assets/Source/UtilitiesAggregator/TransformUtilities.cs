using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;

namespace Omega.Package.Internal
{
    public class TransformUtilities
    {
        internal TransformUtilities()
        {
        }

        /// <summary>
        /// Уничтожает всех потомков переданного трансформа
        /// </summary>
        /// <param name="root">Трансформ, потомки которого будут удалены</param>
        /// <exception cref="ArgumentNullException">Параметр <param name="root"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
        public void DestroyChildren([NotNull] Transform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            ClearChildrenWithoutChecks(root);
        }

        public bool IsChildOf([NotNull] Transform transform, [CanBeNull] Transform parent)
        {
            if (transform is null)
                throw new ArgumentNullException(nameof(transform));
            if (!transform)
                throw new MissingReferenceException(nameof(transform));

            return IsChildOfWithoutChecks(transform, parent);
        }

        /// <summary>
        /// Возвращает всех потомков переданного трансформа 
        /// </summary>
        /// <param name="root">Трансформ, относительно которого будет осуществляться поиск потомков </param>
        /// <returns>Массив потомков</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="root"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
        [NotNull]
        public Transform[] GetChildren([NotNull] Transform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetChildrenWithoutChecks(root);
        }

        public void GetChildren(Transform root, List<Transform> result)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            GetChildrenWithoutChecks(root, result);
        }

        public void GetAllChildren(Transform root, List<Transform> result)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            GetAllChildrenWithoutChecks(root, result);
        }

        public int GetAllChildrenCount(Transform root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
            if (!root)
                throw new MissingReferenceException(nameof(root));

            return GetAllChildrenCountWithoutChecks(root);
        }

        [NotNull]
        internal static Transform[] GetChildrenWithoutChecks([NotNull] Transform root)
        {
            var childrenCount = root.childCount;
            if (childrenCount == 0)
                return Array.Empty<Transform>();

            var children = new Transform[childrenCount];
            for (int i = 0; i < children.Length; i++)
                children[i] = root.GetChild(i);

            return children;
        }

        internal static bool IsChildOfWithoutChecks([CanBeNull] Transform transform, [CanBeNull] Transform parent)
        {
            var temp = transform;
            while (temp)
            {
                // ReSharper disable once PossibleNullReferenceException
                var tempParent = temp.parent;
                if (tempParent == parent)
                    return true;

                temp = tempParent;
            }

            return false;
        }

        internal static void GetChildrenWithoutChecks([NotNull] Transform root, [NotNull] List<Transform> result)
        {
            var childrenCount = root.childCount;
            for (int i = 0; i < childrenCount; i++)
                result.Add(root.GetChild(i));
        }

        internal static void ClearChildrenWithoutChecks([NotNull] Transform root)
        {
            var childesCount = root.childCount;
            if (childesCount == 0)
                return;

            var children = ListPool<Transform>.Rent(childesCount);
            GetChildrenWithoutChecks(root, children);

            foreach (var child in children)
                ObjectUtilities.AutoDestroyWithoutChecks(child.gameObject);

            ListPool<Transform>.ReturnInternal(children);
        }

        internal static void GetAllChildrenWithoutChecks([NotNull] Transform root, [NotNull] List<Transform> children)
        {
            var i = children.Count;
            GetChildrenWithoutChecks(root, children);
            var count = children.Count;
            for (; i < count; i++)
                GetAllChildrenWithoutChecks(children[i], children);
        }

        internal static int GetAllChildrenCountWithoutChecks([NotNull] Transform root)
        {
            var children = ListPool<Transform>.Rent();
            GetAllChildrenWithoutChecks(root, children);
            var result = children.Count;
            ListPool<Transform>.Return(children);

            return result;
        }

        [Obsolete("Use DestroyChildren")]
        public void ClearChilds([NotNull] Transform root) => DestroyChildren(root);

        [NotNull, Obsolete("Use GetChildren")]
        public Transform[] GetChilds([NotNull] Transform root) => GetChildren(root);

        [Obsolete("User GetChildren")]
        public void GetChilds(Transform root, List<Transform> result) => GetChildren(root, result);

        [Obsolete("Use GetAllChildren")]
        public void GetAllChilds(Transform root, List<Transform> result) => GetAllChildren(root, result);

        [Obsolete("Use GetAllChildrenCount")]
        public int GetAllChildsCount(Transform root) => GetAllChildrenCount(root);

        [NotNull, Obsolete("Use GetChildrenWithoutChecks")]
        internal static Transform[] GetChildsWithoutChecks([NotNull] Transform root) => GetChildrenWithoutChecks(root);

        [Obsolete("Use GetChildrenWithoutChecks")]
        internal static void GetChildsWithoutChecks([NotNull] Transform root, [NotNull] List<Transform> result) =>
            GetChildrenWithoutChecks(root, result);

        [Obsolete("Use ClearChildrenWithoutChecks")]
        internal static void ClearChildsWithoutChecks([NotNull] Transform root) => ClearChildrenWithoutChecks(root);

        [Obsolete("Use GetAllChildrenWithoutChecks")]
        internal static void GetAllChildsWithoutChecks([NotNull] Transform root, [NotNull] List<Transform> childs) =>
            GetAllChildrenWithoutChecks(root, childs);

        [Obsolete("Use GetAllChildrenCountWithoutChecks")]
        internal static int GetAllChildsCountWithoutChecks([NotNull] Transform root) =>
            GetAllChildrenCountWithoutChecks(root);
    }
}