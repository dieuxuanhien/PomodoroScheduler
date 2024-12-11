using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace PomodoroScheduler.Behaviors
{
    public class ListViewDragDropBehavior
    {
        private static object _draggedItem;

        // Attach the PreviewMouseLeftButtonDown event to the ListView
        public static void AttachDragDropBehavior(ListView listView)
        {
            listView.PreviewMouseLeftButtonDown += ListView_PreviewMouseLeftButtonDown;
            listView.Drop += ListView_Drop;
            listView.DragOver += ListView_DragOver;
        }

        private static void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView)
            {
                // Get the item being dragged
                var position = e.GetPosition(listView);
                _draggedItem = GetItemUnderMouse(listView, position);
                if (_draggedItem != null)
                {
                    DragDrop.DoDragDrop(listView, _draggedItem, DragDropEffects.Move);
                }
            }
        }

        private static void ListView_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private static void ListView_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListView listView && _draggedItem != null)
            {
                var itemsSource = listView.ItemsSource as ObservableCollection<ViewModels.Task>;
                if (itemsSource == null) return;

                var targetItem = GetItemUnderMouse(listView, e.GetPosition(listView));
                if (targetItem == null || targetItem == _draggedItem) return;

                int oldIndex = itemsSource.IndexOf((ViewModels.Task)_draggedItem);
                int newIndex = itemsSource.IndexOf((ViewModels.Task)targetItem);

                if (oldIndex >= 0 && newIndex >= 0 && oldIndex != newIndex)
                {
                    itemsSource.Move(oldIndex, newIndex);
                }

                _draggedItem = null; // Reset the dragged item
            }
        }

        private static object GetItemUnderMouse(ListView listView, Point position)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(listView, position);
            if (hitTestResult?.VisualHit == null)
                return null;

            ListViewItem container = ItemsControl.ContainerFromElement(listView, hitTestResult.VisualHit) as ListViewItem;
            return container?.Content;
        }
    }
}
