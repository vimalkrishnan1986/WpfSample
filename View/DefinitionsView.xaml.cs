using System;
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
using XInsure.Workflow.Designer.Dto;
namespace XInsure.Workflow.Designer.View
{
    /// <summary>
    /// Interaction logic for DefinitionsView.xaml
    /// </summary>
    public partial class DefinitionsView : UserControl
    {
        public DefinitionsView()
        {
            InitializeComponent();
            this.DataContext = new DefinitionListViewModel(this.GetEditControl, this);
        }

        public UserControl GetEditControl(object param)
        {
            if (param is DefinitionDto)
            {
                return new DesignerView((param as DefinitionDto).Id.Value);
            }
            return null;
        }

    }
}
