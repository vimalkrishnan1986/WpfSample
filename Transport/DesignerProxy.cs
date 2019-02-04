using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XInsure.Workflow.Designer.Dto;
using XInsure.Workflow.Helpers;
namespace XInsure.Workflow.Designer.Transport
{
    internal sealed class DesignerProxy : IWorkflowDesignerProxy
    {
        const string apiUrlKey = "DesignerApiUrl";
        private string BaseUrl
        {
            get
            {
                return ConfigHelper.GetConfigurationValue<string>(apiUrlKey);
            }
        }
        public async Task<int> Save(DefinitionDto dto)
        {
            const string url = "Workflows";
            return await Post<int, DefinitionDto>(url, dto);
        }
        public async Task<List<DefinitionDto>> Get()
        {
            const string url = "Workflows";
            return await this.Get<List<DefinitionDto>, object>(url, null);
        }
        public async Task<DefinitionDto> GetById(int Id)
        {
            const string url = "Workflows";
            return await this.Get<DefinitionDto, int>(url, Id);
        }
        private async Task<TOut> Get<TOut, TIn>(string api, TIn input)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    string url = string.Format("{0}/{1}", BaseUrl, api);
                    if (input != null)
                    {
                        url = string.Format("{0}/{1}", url, input);
                    }
                    var status = await client.GetAsync(url);
                    var result = status.EnsureSuccessStatusCode();
                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(result.ReasonPhrase);
                    }
                    return await result.Content.ReadAsAsync<TOut>();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("There was execption while  communicting to url {0} and excpetion is {1}", BaseUrl, ex.Message));
                }
            }
        }
        private async Task<TOut> Post<TOut, TIn>(string api, TIn dto)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var status = await client.PostAsJsonAsync(string.Format("{0}/{1}", BaseUrl, api), dto);
                    var result = status.EnsureSuccessStatusCode();
                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(result.ReasonPhrase);
                    }
                    return await result.Content.ReadAsAsync<TOut>();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("There was execption while  communicting to url {0} and excpetion is {1}", BaseUrl, ex.Message));
                }
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public async Task<DefinitionDto> GetDefault()
        {
            const string url = "Workflows/default";
            return await this.Get<DefinitionDto, object>(url, null);
        }
    }
}
