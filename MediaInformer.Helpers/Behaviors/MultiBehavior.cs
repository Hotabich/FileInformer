namespace MediaInformer.Helpers.Behaviors
{
    using GalaSoft.MvvmLight.Command;
    using MediaInformer.Models;
    using Microsoft.Xaml.Interactivity;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class ItemClickBehavior : Behavior<ListViewBase>
    {
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
           "ClickCommand", typeof(RelayCommand<MediaToolItem>), typeof(ItemClickBehavior), new PropertyMetadata(null));

        public RelayCommand<MediaToolItem> ClickCommand
        {
            get { return (RelayCommand<MediaToolItem>)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ItemClick += OnItemClickExecute;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ItemClick -= OnItemClickExecute;
        }

        private void OnItemClickExecute(object sender, ItemClickEventArgs e)
        {
            var isSelectionMode = ListViewSelectionMode.None == AssociatedObject.SelectionMode;
            if (isSelectionMode)
            {
                this.ClickCommand.Execute(e.ClickedItem);
            }
        }
    }
}
