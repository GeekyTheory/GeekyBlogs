using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekyBlogs.Services
{
    public interface IResizable
    {
        int ColSpan { get; set; }
        int RowSpan { get; set; }
    }
}
