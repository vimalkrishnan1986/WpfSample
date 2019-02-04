using System;
using System.Activities.Presentation.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XInsure.Workflow.Designer.Helpers
{
    public class DesignerValidationService : IValidationErrorService
    {
        public int ErrorCount { get; set; }
        public IList<ValidationErrorInfo> Errors { get; private set; }

        public void ShowValidationErrors(IList<ValidationErrorInfo> errors)
        {
            this.Errors = errors;
            this.ErrorCount = Errors.Count();
        }
    }
}
