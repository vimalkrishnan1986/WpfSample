using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.Validation;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using XInsure.Workflow.Designer.Commands;
using XInsure.Workflow.Designer.Helpers;
using XInsure.Workflow.Designer.Dto;
using XInsure.Workflow.Designer.Transport;
using System.Runtime.Versioning;
using XInsure.Workflow.Designer.CSharpSupport;
using Microsoft.CSharp.Activities;
using XInsure.Workflow.Helpers;
using System.Activities.XamlIntegration;
using System.Windows.Markup;
namespace XInsure.Workflow.Designer.ViewModel
{
    public sealed class DesignerViewModel : ViewModelBase
    {
        #region Private variables
        private string _name;
        private string _description;
        private int? Id;
        private ICommand _submit;
        private RoslynExpressionEditorService _expressionEditorService;
        DesignerMetadata designermetdata;
        #endregion

        #region Properties
        public bool IsNew { get; private set; }
        public WorkflowDesigner Designer { get; set; }
        public ICommand Submit
        {
            get
            {
                if (_submit == null)
                {
                    _submit = new ActionCommand(this.Save);
                }
                return _submit;
            }
            set
            {
                _submit = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                base.NotifyPropertyChanged("Name");
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                base.NotifyPropertyChanged("Description");
            }
        }
        #endregion

        public DesignerViewModel()
            : base()
        {
            Submit = new ActionCommand(this.Save);
            Id = null;
            IsNew = true;
            this.CreateDesigner();
            this.RegisterMetadata();
            this.InitializeDesigner();
            this.SetDefaultContext();
        }

        public DesignerViewModel(int id)
            : base()
        {
            Id = id;
            CreateDesigner();
            this.RegisterMetadata();
            LoadDesigner();
        }

        #region Public or internal
        internal async void Save()
        {

            try
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    ShowMessage("Name can't be empty");
                    return;
                }
                var validationService = this.Designer.Context.Services.GetService<IValidationErrorService>();
                var errorService = validationService as DesignerValidationService;
                if (errorService.ErrorCount > 0)
                {
                    string errorMessage = string.Join(Environment.NewLine, errorService.Errors);
                    base.ShowMessage(string.Format("Please fix the errors before saving {0} {1}", errorMessage, Environment.NewLine));
                    return;
                }
                string tempFileName = string.Empty;
                tempFileName = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
                this.Designer.Flush();
                const int defaultsize = 1326;

                FileHelper.WriteTextToFile(tempFileName, Designer.Text);
                var fileContents = FileHelper.ReadFromFile(tempFileName);
                if (fileContents.Length <= defaultsize)
                {
                    ShowMessage("There should me minimum values for workflow");
                    return;
                }

                try
                {
                    ActivityXamlServicesSettings settings = new ActivityXamlServicesSettings()
                    {
                        CompileExpressions = true,

                    };

                    XamlReader reader = new XamlReader();
                    var CompiliedActivity = ActivityXamlServices.Load(tempFileName, settings);
                }
                catch (Exception cex)
                {
                    base.ShowMessage(cex.Message);

                }

                var dto = new DefinitionDto()
  {
      Name = this.Name,
      Description = this.Description,
      Id = this.Id,
      Xaml = fileContents
  };
                FileHelper.Delete(tempFileName);
                using (IWorkflowDesignerProxy proxy = new DesignerProxy())
                {
                    var res = await proxy.Save(dto);
                    this.Id = res;
                    if (Id > 0)
                    {
                        ShowMessage("Saved Successfully");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowMessage(ex.Message);
            }
        }
        public ToolboxControl GetToolboxControl()
        {
            var assembly = Assembly.Load("XInsure.Workflow.Activities");
            return ToolboxHelper.GetToolboxControl(assembly);
        }

        #endregion

        #region Private methods
        private void RegisterMetadata()
        {
            designermetdata = new DesignerMetadata();
            designermetdata.Register();

        }
        private void InitializeDesigner()
        {
            var designerView = this.Designer.Context.Services.GetService<DesignerView>();
            if (designerView != null)
            {
                designerView.WorkflowShellBarItemVisibility =
                    ShellBarItemVisibility.Imports |
                    ShellBarItemVisibility.MiniMap |
                    ShellBarItemVisibility.Variables |
                     ShellBarItemVisibility.Arguments |
                    ShellBarItemVisibility.Zoom;
            }
        }
        private void SetDefaultContext()
        {
            if (IsNew)
            {
                ActivityBuilder builder = new ActivityBuilder()
                {
                    Name = "XInsurance",
                    Implementation = new Flowchart()
                };
                this.Designer.Load(builder);
            }
        }
        private void CreateDesigner()
        {
            this.Designer = new WorkflowDesigner();
            //this._expressionEditorService = new RoslynExpressionEditorService();
            //ExpressionTextBox.RegisterExpressionActivityEditor(new CSharpValue<string>().Language, typeof(RoslynExpressionEditor), CSharpExpressionHelper.CreateExpressionFromString);
            //this.Designer.Context.Services.Publish<IExpressionEditorService>(this._expressionEditorService);
            this.Designer.Context.Services.Publish<IValidationErrorService>(new DesignerValidationService());
            DesignerConfigurationService configurationService = Designer.Context.Services.GetService<DesignerConfigurationService>();
            configurationService.TargetFrameworkName = new FrameworkName(".NETFramework", new Version(4, 5));
        }
        private async void LoadDesigner()
        {
            using (IWorkflowDesignerProxy proxy = new DesignerProxy())
            {
                var definition = await proxy.GetById(Id.Value);
                if (definition == null)
                {
                    base.ShowMessage("Could not pick the values");
                }
                var xaml = definition.Xaml;
                Designer.Text = xaml;
                this.Name = definition.Name;
                this.Description = definition.Description;
                this.Designer.Text = xaml;
                this.Designer.Load();
            }
        }
        #endregion

    }
}
