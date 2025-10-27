namespace WhatsNewWebsite.DataModel
{
    public class WhatsNewItemSummary
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public int MinorRelease { get; set; }
        public string Version { get; set; }
        public string FileName { get; set; }
        public string Id { get; set; }

        public WhatsNewItemSummary() : this("", "", "", 0, "")
        {
        }

        public WhatsNewItemSummary(string Title, string Description, string ImageBase64, int MinorRelease, string FileName)
        {
            this.Title = Title;
            this.Description = Description;
            this.ImageBase64 = ImageBase64;
            this.MinorRelease = MinorRelease;
            this.Version =  GetMinorRelease(MinorRelease);
            this.FileName = FileName;
            this.Id = GetId(FileName);
        }

        private string GetMinorRelease(int MinorRelease)
        {
            return $"17.{MinorRelease}";
        }

        private string GetId(string FileName)
        {
            var lower = FileName.ToLower();
            return lower.Replace(".md", "");
        }
    }
}