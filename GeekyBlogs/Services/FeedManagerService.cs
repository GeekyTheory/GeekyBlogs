using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web;
using Windows.Web.Syndication;
using GeekyBlogs.Common;
using GeekyBlogs.Models;
using GeekyTool;

namespace GeekyBlogs.Services
{
    public interface IFeedManagerService
    {
        Task<List<FeedItem>> GetFeedAsync(string rssUri);
    }

    public class FeedManagerService : IFeedManagerService
    {
        public async Task<List<FeedItem>> GetFeedAsync(string feedUri)
        {
            Uri uri;
            if (!Uri.TryCreate(feedUri.Trim(), UriKind.Absolute, out uri))
            {
                //rootPage.NotifyUser("Error: Invalid URI.", NotifyType.ErrorMessage);
                return null;
            }

            SyndicationClient client = new SyndicationClient();
            client.BypassCacheOnRetrieve = true;

            client.SetRequestHeader("User-Agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            try
            {
                List<FeedItem> feedItems = new List<FeedItem>();
                SyndicationFeed feed = await client.RetrieveFeedAsync(uri);

                //rootPage.NotifyUser("Feed download complete.", NotifyType.StatusMessage);

                //ISyndicationText title = feed.Title;
                //feedData.Title = title != null ? title.Text : "(no title)";
                
                foreach (FeedItem feedItem in feed.Items.Select((item, i) => CreateFeedItem(item, feed.SourceFormat, i)))
                {
                    feedItems.Add(feedItem);
                }

                //foreach (SyndicationItem item in feed.Items)
                //{
                //    FeedItem feedItem = CreateFeedItem(item, feed.SourceFormat);
                //    feedData.Items.Add(feedItem);
                //}

                return feedItems;
            }
            catch (Exception ex)
            {
                SyndicationErrorStatus status = SyndicationError.GetStatus(ex.HResult);
                if (status == SyndicationErrorStatus.InvalidXml)
                {
                    //OutputField.Text += "An invalid XML exception was thrown. " +
                    //    "Please make sure to use a URI that points to a RSS or Atom feed.";
                }

                if (status == SyndicationErrorStatus.Unknown)
                {
                    WebErrorStatus webError = WebError.GetStatus(ex.HResult);

                    if (webError == WebErrorStatus.Unknown)
                    {
                        // Neither a syndication nor a web error. Rethrow.
                        throw;
                    }
                }

                //rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
            }

            return null;
        }

        private FeedItem CreateFeedItem(SyndicationItem item, SyndicationFormat format, int i)
        {
            var feedItem = new FeedItem();
            feedItem.Format = format;

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
