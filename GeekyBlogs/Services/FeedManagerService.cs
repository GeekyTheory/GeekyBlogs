using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.WebUI;
using Windows.Web;
using Windows.Web.Syndication;
using GeekyBlogs.Common;
using GeekyBlogs.Models;
using GeekyTool;
using GeekyTool.Models;
using HtmlAgilityPack;

namespace GeekyBlogs.Services
{
    public interface IFeedManagerService
    {
        Task<List<FeedItem>> GetFeedFromMenuItemAsync(MenuItem menuItem);
        Task<List<FeedItem>> GetFeedAsync(string rssUri);
        Task<string> RemoveUnusedElementsAsync(string url);
    }

    public class FeedManagerService : IFeedManagerService
    {
        public async Task<List<FeedItem>> GetFeedFromMenuItemAsync(MenuItem menuItem)
        {
            var feeds = await GetFeedAsync(menuItem.Url);
            foreach (var feedItem in feeds)
            {
                feedItem.BlogItem = menuItem;
            }
            return feeds;
        }

        /// <summary>
        /// Get the feeds with the SyndicationClient
        /// </summary>
        /// <param name="feedUri"></param>
        /// <returns></returns>
        public async Task<List<FeedItem>> GetFeedAsync(string url)
        {
            Uri uri;
            if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out uri))
            {
                await ApiHelper.ShowMessageDialog("Error", "Invalid URI");
                return null;
            }

            var client = new SyndicationClient();
            client.BypassCacheOnRetrieve = true;

            client.SetRequestHeader("User-Agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            try
            {
                var feed = await client.RetrieveFeedAsync(uri);

                return feed.Items.Select((item, i) => CreateFeedItem(item, feed.SourceFormat, i)).ToList();
            }
            catch (Exception ex)
            {
                var status = SyndicationError.GetStatus(ex.HResult);
                if (status == SyndicationErrorStatus.InvalidXml)
                {
                    await ApiHelper.ShowMessageDialog("Error", 
                        "An invalid XML exception was thrown. Please make sure to use a URI that points to a RSS or Atom feed."
                        + "\n\n\n" + ex.Message);
                }

                if (status == SyndicationErrorStatus.Unknown)
                {
                    WebErrorStatus webError = WebError.GetStatus(ex.HResult);

                    if (webError == WebErrorStatus.Unknown)
                    {
                        await ApiHelper.ShowMessageDialog("Error", ex.Message + "\n" + ex.InnerException?.Message);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Removing unused elements on html content
        /// </summary>
        /// <param name="url"></param>
        /// <param name="web"></param>
        /// <param name="feedItem"></param>
        /// <returns></returns>
        public async Task<string> RemoveUnusedElementsAsync(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(url);

            var test = document.DocumentNode
                .Descendants(
                    "div")
                .FirstOrDefault(
                    d =>
                        d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("content-wrapper"));

            document.DocumentNode.Descendants("body").ToList()[0].InnerHtml = test.OuterHtml;

            return document.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// Create each item from the SyndicationItem. Necesary to format the Content property with RemoveUnusedElementsAsync
        /// </summary>
        /// <param name="item"></param>
        /// <param name="format"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private FeedItem CreateFeedItem(SyndicationItem item, SyndicationFormat format, int i)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var feedItem = new FeedItem {Format = format};

            if (item.Title != null)
                feedItem.Title = item.Title.Text;

            feedItem.PubDate = item.PublishedDate.DateTime;
            feedItem.PubDateShow = ApiHelper.GetCustomFormattedDate(item.PublishedDate.DateTime);

            foreach (var author in item.Authors)
            {
                feedItem.Author = author.NodeValue;
            }

            if (format == SyndicationFormat.Atom10)
            {
                feedItem.Content = item.Content.Text;
            }
            else if (format == SyndicationFormat.Rss20)
            {
                feedItem.Content = item.Summary.Text;
            }

            feedItem.ImageUrl = GeekyHelper.ExtractFirstImageFromHtml(feedItem.Content);
            feedItem.Content = string.Empty;

            if (item.Links.Count > 0)
            {
                feedItem.Link = item.Links.FirstOrDefault().Uri;
            }
            
            if (i == 0 || i == 1)
            {
                feedItem.ColSpan = 2;
                feedItem.RowSpan = 2;
            }
            else
            {
                feedItem.ColSpan = 1;
                feedItem.RowSpan = 1;
            }
            
            return feedItem;
        }
    }
}
