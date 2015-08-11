using System;
using System.Collections.ObjectModel;
using Windows.Web.Syndication;
using GeekyBlogs.Services;

namespace GeekyBlogs.Models
{
    //public class FeedData
    //{
    //    public string Title { get; set; }

    //    private ObservableCollection<FeedItem> items = new ObservableCollection<FeedItem>();
    //    public ObservableCollection<FeedItem> Items
    //    {
    //        get { return items; }
    //        set { items = value; }
    //    }
    //}

    public class FeedItem : IResizable
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PubDate { get; set; }
        public string PubDateShow { get; set; }
        public string ImageUrl { get; set; }
        public Uri Link { get; set; }
        public MenuItem BlogName { get; set; }
        public SyndicationFormat Format { get; set; }

        // IResizable
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
    }
}
