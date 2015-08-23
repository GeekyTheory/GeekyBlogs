using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace GeekyBlogs.Common
{
    public class ApiHelper
    {
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
    }
}
