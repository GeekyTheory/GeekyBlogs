using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.Web.Syndication;

namespace GeekyBlogs.Common
{
    public class ApiHelper
    {
        public static string ExtractFirstImageFromHtml(string content)
        {
            string matchString = Regex.Match(content, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
            return matchString;
        }
        
        public static string GetCustomFormattedDate(DateTime date)
        {
            var now = DateTime.Now;
            var dateNow = new DateTime(now.Year, now.Month, now.Day);
            var feedDate = new DateTime(date.Year, date.Month, date.Day);

            if (dateNow == feedDate)
            {
                return string.Format("Hoy, {0}", date.ToString("HH:mm"));
            }
            else if (dateNow == feedDate.AddDays(1))
            {
                return string.Format("Ayer, {0}", date.ToString("HH:mm"));
            }
            else
            {
                return date.ToString("f");
            }
        }

	    public static async Task ShowMessageDialog(string title, string content)
	    {
		    MessageDialog msg = new MessageDialog(content, title);
		    await msg.ShowAsync();
	    }

        public static bool ValidFeedUri(string feedUri)
        {
            if (string.IsNullOrEmpty(feedUri))
                return false;

            Uri uri;
            if (!Uri.TryCreate(feedUri.Trim(), UriKind.Absolute, out uri))
                return false;
            else
                return true;
        }
    }
}
