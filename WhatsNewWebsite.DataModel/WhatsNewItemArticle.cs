using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsNewWebsite.DataModel
{
    public class WhatsNewItemArticle
    {
        public string FileName { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        public WhatsNewItemArticle()
        {
            this.Id = GetId(FileName);
        }

        private string GetId(string FileName)
        {
            var lower = FileName.ToLower();
            return lower.Replace(".md", "");
        }
    }
}
