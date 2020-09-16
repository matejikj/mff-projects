using Harmonogram.Helper;
using Harmonogram.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Harmonogram
{
    public static class StaticResources
    {
        public static string UserName { get; set; }
        public static User User { get; set; }
        public static List<string> Materials { get; set; }
        public static List<string> Cars { get; set; }
        public static List<string> Departments { get; internal set; }
        public static Order lastAddedOrder { get; set; }
        public static string lastUrl { get; set; }
        public static string blankRowName { get; set; }
        public static string blankRowColor { get; set; }
    }
}
