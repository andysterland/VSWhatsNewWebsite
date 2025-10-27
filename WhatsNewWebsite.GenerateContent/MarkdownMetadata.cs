using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WhatsNewWebsite.GenerateContent
{
    public class MarkdownMetadata
    {
        [DefaultValue("")]
        public string title { get; set; } = string.Empty;
        [DefaultValue("")]
        public string description { get; set; } = string.Empty;
        [DefaultValue("")]
        public string thumbnailImage { get; set; } = string.Empty;
        [DefaultValue("")]
        public string featureId { get; set; } = string.Empty;
        [DefaultValue("")]
        public string author { get; set; } = string.Empty;
    }
}
