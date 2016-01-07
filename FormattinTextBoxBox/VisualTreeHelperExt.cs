using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace FormattinTextBoxBox
{
    internal static class VisualTreeHelperExt
    {
        internal static IEnumerable<DependencyObject> NestedChildren(this DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
                if (VisualTreeHelper.GetChildrenCount(child) != 0)
                {
                    foreach (var nestedChild in NestedChildren(child))
                    {
                        yield return nestedChild;
                    }
                }
            }
        }
    }
}
