using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using XInsure.Workflow.Designer.Dto;
namespace XInsure.Workflow.Designer.Transport
{
    internal interface IWorkflowDesignerProxy : IDisposable
    {
        Task<int> Save(DefinitionDto dto);
        Task<List<DefinitionDto>> Get();
        Task<DefinitionDto> GetById(int Id);
        Task<DefinitionDto> GetDefault();
    }
}
