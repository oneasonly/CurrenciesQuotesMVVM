using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ViewVMWPF.ExtensionsXaml
{
    public static class AttachedProperties
    {
        #region AttachedProperties.SelectedItems Attached Property
        public static IList GetSelectedItems(ListBox obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(ListBox obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        public static readonly DependencyProperty
            SelectedItemsProperty =
                DependencyProperty.RegisterAttached(
                    "SelectedItems",
                    typeof(IList),
                    typeof(AttachedProperties),
                    new PropertyMetadata(null,
                        SelectedItems_PropertyChanged));

        private static void SelectedItems_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var listBox = obj as ListBox;
            IList selectedItems = e.NewValue as IList;

            if (selectedItems is INotifyCollectionChanged)
            {
                (selectedItems as INotifyCollectionChanged).CollectionChanged += (s, e3) =>
                {
                    if (null != e3.OldItems)
                        foreach (var item in e3.OldItems)
                            listBox.SelectedItems.Remove(item);
                    if (null != e3.NewItems)
                        foreach (var item in e3.NewItems)
                            listBox.SelectedItems.Add(item);
                };
            }

            if (null != selectedItems)
            {
                if (selectedItems.Count > 0)
                {
                    listBox.SelectedItems.Clear();
                    foreach (var item in selectedItems)
                        listBox.SelectedItems.Add(item);
                }

                listBox.SelectionChanged += (s, e2) =>
                {
                    if (null != e2.RemovedItems)
                        foreach (var item in e2.RemovedItems)
                            selectedItems.Remove(item);
                    if (null != e2.AddedItems)
                        foreach (var item in e2.AddedItems)
                            selectedItems.Add(item);
                };
            }
        }
        #endregion AttachedProperties.SelectedItems Attached Property
    }
}
