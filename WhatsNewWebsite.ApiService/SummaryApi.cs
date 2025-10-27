using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WhatsNewWebsite.DataModel;

namespace WhatsNewWebsite.ApiService
{
    public class SummaryApi
    {
        private Dictionary<string, WhatsNewItemSummary> _itemSummaries;

        public SummaryApi (List<WhatsNewItemSummary> ItemSummaries)
        {
            _itemSummaries = new Dictionary<string, WhatsNewItemSummary>();

            foreach(var item in ItemSummaries)
            {
                if(!_itemSummaries.ContainsKey(item.Id))
                {
                    _itemSummaries.Add(item.Id, item);
                }
            }
        }

        public WhatsNewItemSummary Get(string Id)
        {
            return _itemSummaries.GetValueOrDefault(Id);
        }

        public List<WhatsNewItemSummary> GetAll()
        {
            return _itemSummaries.Values.ToList();
        }

        public List<WhatsNewItemSummary> GetRandom(int Count)
        {
            if (Count == 0)
            {
                Count = 60;
            }
            else if (Count >= _itemSummaries.Values.Count)
            {
                return _itemSummaries.Values.ToList();
            }

            var rnd = new Random();
            return _itemSummaries.Values.ToList().OrderBy(x => rnd.Next()).Take(Count).ToList();
        }

        public List<WhatsNewItemSummary> GetWithTerm(string Term)
        {
            var results = new List<WhatsNewItemSummary>();
            foreach (var item in _itemSummaries.Values)
            {
                if (item.Title.Contains(Term, StringComparison.InvariantCultureIgnoreCase) || item.Description.Contains(Term, StringComparison.InvariantCultureIgnoreCase))
                {
                    results.Add(item);
                }
            }
            return results;
        }

        // return items with the specified version
        public List<WhatsNewItemSummary>GetWithVersion(string Version)

        {
            var results = new List<WhatsNewItemSummary>();
            foreach (var item in _itemSummaries.Values)
            {
                if (item.Version == Version)
                {
                    results.Add(item);
                }
            }
            return results;
        }
    }
}
