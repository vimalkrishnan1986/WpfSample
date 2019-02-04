using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XInsure.Workflow.Designer.Helpers
{
    internal static class ToolboxHelper
    {
        internal static ToolboxControl GetToolboxControl(Assembly extended = null)
        {
            ToolboxControl control = new ToolboxControl();
            var assemblies = new List<Assembly>();
            assemblies.Add(typeof(Assign).Assembly);
            ToolboxCategory category = new ToolboxCategory("Standared Activities");
            var query = from asm in assemblies
                        from type in asm.GetTypes()
                        where type.IsPublic &&
                        !type.IsNested &&
                        !type.IsAbstract &&
                        !type.ContainsGenericParameters &&
                        (typeof(Activity).IsAssignableFrom(type) ||
                        typeof(IActivityTemplateFactory).IsAssignableFrom(type))
                        orderby type.Name
                        select new ToolboxItemWrapper(type, type.Name);
            query.ToList().ForEach(p =>
                {
                    if (!category.Tools.Contains(p))
                    {
                        category.Add(p);
                    }
                }
                );
            control.Categories.Add(category);
            if (extended != null)
            {
                var tools = extended.GetTypes().Where(p => p.IsPublic && !p.IsAbstract && (typeof(Activity).IsAssignableFrom(p))).Select(p => new ToolboxItemWrapper(p, p.Name));
                category = new ToolboxCategory("Xceedance Activities");
                tools.ToList().ForEach(p => category.Add(p));
                control.Categories.Add(category);
            }
            return control;
        }
    }
}
