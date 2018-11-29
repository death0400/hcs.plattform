using System;
using Hcs.Serialize.Xlsx;
using Hcs.Serialize.Xlsx.Template;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform.File
{
    class WebApplicationXlsxTemplateFactory : IXlsxTEmplateFactory
    {
        private string root;
        public Func<string, string, string> CombinePath { get; set; }
        public WebApplicationXlsxTemplateFactory(IHostingEnvironment env)
        {
            CombinePath = (root, key) => System.IO.Path.Combine(root, "ExcelTemplate", key);
            root = env.ContentRootPath;
        }
        public IXlsxTemplate Get(string key)
        {
            var path = CombinePath(root, key);
            if (!System.IO.File.Exists(path))
            {
                throw new Exception($"template {key} not exists");
            }

            return new FileXlsxTemplate(path);
        }
    }
}