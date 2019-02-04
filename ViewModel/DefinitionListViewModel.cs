using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using XInsure.Workflow.Designer.Dto;
using XInsure.Workflow.Designer.Transport;
using XInsure.Workflow.Designer.Commands;
using System.Windows;
namespace XInsure.Workflow.Designer.ViewModel
{
    public sealed class DefinitionListViewModel : ViewModelBase
    {
        private List<DefinitionDto> _definitions;
        private ICommand _edit;
        private ICommand _refresh;
        public ICommand Refresh
        {
            get
            {
                if (_refresh == null)
                {
                    _refresh = new ActionCommand(this.Load);
                }
                return _refresh;
            }
            set
            {
                _refresh = value;
            }
        }
        public ICommand Edit
        {
            get
            {
                return _edit;
            }
            set
            {
                _edit = value;
                base.NotifyPropertyChanged("Edit");
            }
        }
        public List<DefinitionDto> Definitions
        {
            get
            {
                return _definitions;
            }
            set
            {
                _definitions = value;
                base.NotifyPropertyChanged("Definitions");
            }
        }

        public DefinitionListViewModel(Func<object, UserControl> getEditControl, DependencyObject obj)
        {
            this.Edit = new EditCommand(getEditControl, obj);
            this.Load();
        }
        private async void Load()
        {
            IWorkflowDesignerProxy proxy = new DesignerProxy();
            Definitions = await proxy.Get();
            if (Definitions == null)
            {
                ShowMessage("Error");
                return;
            }
            if (Definitions.Count == 0)
            {
                ShowMessage("No actieve definctions found");
            }
        }

    }
}
