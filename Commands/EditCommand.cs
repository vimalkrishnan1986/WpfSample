using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XInsure.Workflow.Designer.Commands
{
    public sealed class EditCommand : BaseCommand
    {
        private UserControl contentControl;
        private ResizeMode ResizeMode;
        private Func<object, UserControl> GetControl;
        public EditCommand(Func<object, UserControl> getcontrolmethod, DependencyObject obj, ResizeMode? mode = null)
            : base(obj)
        {
            this.GetControl = getcontrolmethod;
            this.ResizeMode = mode.HasValue ? mode.Value : ResizeMode.CanResize;
        }

        protected override bool Can(object parameter)
        {
            return true;
        }

        protected override void Do(object parameter)
        {
            contentControl = this.GetControl(parameter);
            base.Open(contentControl, ResizeMode);
        }
    }
}
