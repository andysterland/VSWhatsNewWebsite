using System.Linq;
using WhatsNewWebsite.DataModel;

namespace WhatsNewWebsite.ApiService
{
    public class ArticleApi
    {
        public Dictionary<string, WhatsNewItemArticle> _articles = new Dictionary<string, WhatsNewItemArticle>();

        public ArticleApi(List<WhatsNewItemArticle> Articles)
        {
            foreach(var article in Articles)
            {
                if(_articles.ContainsKey(article.Id))
                {
                    continue;
                }
                _articles.Add(article.Id, article);
            }
        }

        public WhatsNewItemArticle Get(string Id)
        {
            if (_articles.ContainsKey(Id))
            {
                return _articles[Id];
            }
            return new WhatsNewItemArticle();
        }
    }
}
