using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
namespace XInsure.Workflow.Designer.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        protected abstract void Do(object paramter);
        protected abstract bool Can(object parameter);
        private DependencyObject obj;

        protected BaseCommand(DependencyObject obj)
            : base()
        {
            this.obj = obj;

        }
        public bool CanExecute(object parameter)
        {
            return Can(parameter);
        }

        protected void Open(UserControl control, ResizeMode mode)
        {
            if (obj == null)
            {
                throw new Exception("Page has not been set");
            }

            var service = NavigationService.GetNavigationService(obj);
            OnStart designerPage = new OnStart();
            designerPage.WorkflowEdit.Content = control;
            designerPage.TabContainer.SelectedIndex = 1;
            service.Navigate(designerPage);
        }
        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                this.Log("There is paramteter passed");
            }
            this.Do(parameter);
        }

        protected void Log(string message)
        {

        }
    }
}
