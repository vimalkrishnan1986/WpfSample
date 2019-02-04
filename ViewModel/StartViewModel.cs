using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XInsure.Workflow.Designer.Commands;
namespace XInsure.Workflow.Designer.ViewModel
{
    public sealed class StartViewModel : ViewModelBase
    {
        public ICommand ChangeMenu { get; set; }

        public StartViewModel(Action<object> open)
        {
            //this.ChangeMenu = new EditCommand(open);
        }

    }
}
