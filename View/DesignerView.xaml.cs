using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XInsure.Workflow.Designer.ViewModel;

namespace XInsure.Workflow.Designer.View
{
    /// <summary>
    /// Interaction logic for DesignerView.xaml
    /// </summary>
    public partial class DesignerView : UserControl
    {
        WorkflowDesigner Designer;
        public DesignerView()
        {
            InitializeComponent();
            this.DataContext = new DesignerViewModel();
            AddToolBox();
            LoadDesigner();
            AddPropertyInspector();
        }
        public DesignerView(int definitionId)
        {
            InitializeComponent();
            this.DataContext = new DesignerViewModel(definitionId);
            AddToolBox();
            LoadDesigner();
            AddPropertyInspector();
        }
        private void AddToolBox()
        {
            ToolboxControl tc = (this.DataContext as DesignerViewModel).GetToolboxControl();
            Grid.SetColumn(tc, 0);
            Grid.SetRow(tc, 0);
            designerGrid.Children.Add(tc);
        }
        private void AddPropertyInspector()
        {
            if (Designer.PropertyInspectorView != null)
            {
                Grid.SetColumn(Designer.PropertyInspectorView, 2);
                Grid.SetRow(Designer.PropertyInspectorView, 0);
                designerGrid.Children.Add(Designer.PropertyInspectorView);
            }
        }
        private void LoadDesigner()
        {
            Designer = (this.DataContext as DesignerViewModel).Designer;
            if (this.Designer == null)
            {
                throw new Exception("Designer is not initialized");
            }
            Grid.SetColumn(this.Designer.View, 1);
            Grid.SetRow(Designer.View, 0);
            designerGrid.Children.Add(this.Designer.View);
        }
    }
}
