using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XInsure.Workflow.Designer.Commands
{
    public sealed class ActionCommand : BaseCommand
    {
        private Action _saveMethod;
        public ActionCommand(Action saveMethod):base(null)
        {
            _saveMethod = saveMethod;
        }
        protected override void Do(object paramter)
        {
            this._saveMethod();
        }

        protected override bool Can(object parameter)
        {
            return true;
        }
    }
}
