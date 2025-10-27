using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using WhatsNewWebsite.DataModel;
using Markdig.Syntax;
using System.Text.Json;
using SkiaSharp;
using MimeMapping;
using System.Reflection.Metadata.Ecma335;

namespace WhatsNewWebsite.GenerateContent
{
    internal class Program
    {
        const string markdownFilePath = @"Q:\src\vs-copilot-bingo-card\Content";
        const string summaryOutputFile = @"Q:\src\VSWhatsNewWebsite\WhatsNewWebsite.ApiService\whatsnewitems.json";
        const string articleOutputFile = @"Q:\src\VSWhatsNewWebsite\WhatsNewWebsite.ApiService\whatsnewarticles.json";
        const string mediaOutputFolder = @"Q:\src\VSWhatsNewWebsite\WhatsNewWebsite.Web\wwwroot\Media\";
        const string placeholderThumbnailImage = @"Q:\src\VSWhatsNewWebsite\WhatsNewWebsite.Web\wwwroot\Media\Placeholder_Thumbnail.webp";
        
        static void Main(string[] args)
        {

            Console.WriteLine($"Generating content from {markdownFilePath}");

            var summaries = new List<WhatsNewItemSummary>();
            var articles = new List<WhatsNewItemArticle>();
            var releaseFolders = Directory.GetDirectories(markdownFilePath);

            foreach (var folder in releaseFolders)
            {
                var minorRelease = int.Parse(folder.Split(".").Last());
                var markdownFiles = Directory.GetFiles(Path.Combine(folder, "content"), "*.md");
                foreach (var markdownFile in markdownFiles)
                {
                    var results = ParseMarkdownFile(markdownFile);

                    WhatsNewItemSummary summary = results.Item1;
                    summary.MinorRelease = minorRelease;
                    summary.FileName = Path.GetFileName(markdownFile);
                    summary.Id = summary.FileName.Replace(".md", string.Empty);
                    summaries.Add(summary);

                    WhatsNewItemArticle article = results.Item2;
                    article.Id = summary.FileName.Replace(".md", string.Empty);
                    articles.Add(article);

                    Console.WriteLine($"Added: {summary.Title}");
                }

                var mediaPath = Path.Combine(folder, "Media");

                if (Path.Exists(mediaPath))
                {
                    var files = Directory.GetFiles(mediaPath, "*.*");
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);
                        var fileExt = Path.GetExtension(file);
                        var fileMime = MimeUtility.GetMimeMapping(fileExt);
                        var destPath = Path.Combine(mediaOutputFolder, fileName);
                        
                        if (fileMime.StartsWith("image"))
                        { 
                            var srcBytes = File.ReadAllBytes(file);
                            var outBytes = ScaleImage(srcBytes, 300, int.MaxValue);
                            File.WriteAllBytes(destPath, outBytes);
                        }
                        else if(fileMime.StartsWith("video"))
                        {
                            Console.WriteLine($"Converting {file} to gif");
                            destPath = Path.ChangeExtension(destPath, ".gif");
                            FFmpeg.ConvertToGif(file, destPath);
                        }
                        else
                        {
                            File.Copy(file, destPath, true);
                        }
                    }
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(summaryOutputFile, JsonSerializer.Serialize(summaries, options));
            File.WriteAllText(articleOutputFile, JsonSerializer.Serialize(articles, options));
        }

        private static Tuple<WhatsNewItemSummary, WhatsNewItemArticle> ParseMarkdownFile(string MarkdownFilePath)
        {
            var markdown = File.ReadAllText(MarkdownFilePath);
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .Build();

            var writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            pipeline.Setup(renderer);

            var document = Markdown.Parse(markdown, pipeline);

            // extract the front matter from markdown document
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            var yaml = yamlBlock.Lines.ToString();

            // deserialize the yaml block into a custom type
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            var metadata = deserializer.Deserialize<MarkdownMetadata>(yaml);

            var imageBase64 = GetImageBase64(MarkdownFilePath, metadata.thumbnailImage);

            var summary = new WhatsNewItemSummary()
            {
                Title = metadata.title,
                Description = metadata.description,
                ImageBase64 = imageBase64,
            };

            renderer.Render(document);
            writer.Flush();
            var html = writer.ToString();

            var article = new WhatsNewItemArticle()
            {
                FileName = Path.GetFileName(MarkdownFilePath),
                HtmlContent = html
            };

            return  Tuple.Create(summary, article);
        }

        private static string GetImageBase64(string MarkdownFilePath, string ThumbnailImage)
        {
            var markdownFolderPath = Path.GetDirectoryName(MarkdownFilePath);
            var imageFilePath = Path.Combine(markdownFolderPath, ThumbnailImage);

            if( !File.Exists(imageFilePath) )
            {
                imageFilePath = placeholderThumbnailImage;
            }

            byte[] imageArray = System.IO.File.ReadAllBytes(imageFilePath);
            byte[] scaledImageArray = ScaleImage(imageArray, 384, 220);
            string base64ImageRepresentation = Convert.ToBase64String(scaledImageArray);
            var dataUri = $"data:image/png;base64,{base64ImageRepresentation}";

            return dataUri;
        }

        public static byte[] ScaleImage(byte[] imageBytes, int maxWidth, int maxHeight)
        {
            SKBitmap image = SKBitmap.Decode(imageBytes);

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var info = new SKImageInfo(newWidth, newHeight);
            image = image.Resize(info, SKFilterQuality.High);

            using var ms = new MemoryStream();
            image.Encode(ms, SKEncodedImageFormat.Png, 100);
            return ms.ToArray();
        }
    }
}
