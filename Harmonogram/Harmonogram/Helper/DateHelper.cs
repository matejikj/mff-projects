using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Helper
{
    static class DateHelper
    {
        private static List<DateTime> statniSvatky = new List<DateTime>()
        {
            new DateTime(2019,1,1),
            new DateTime(2019,4,19),
            new DateTime(2019,4,22),
            new DateTime(2019,5,1),
            new DateTime(2019,5,8),
            new DateTime(2019,7,5),
            new DateTime(2019,7,6),
            new DateTime(2019,9,28),
            new DateTime(2019,10,28),
            new DateTime(2019,11,17),
            new DateTime(2019,12,24),
            new DateTime(2019,12,25),
            new DateTime(2019,12,26),
            new DateTime(2020,1,1),
            new DateTime(2020,4,10),
            new DateTime(2020,4,13),
            new DateTime(2020,5,1),
            new DateTime(2020,5,8),
            new DateTime(2020,7,5),
            new DateTime(2020,7,6),
            new DateTime(2020,9,28),
            new DateTime(2020,10,28),
            new DateTime(2020,11,17),
            new DateTime(2020,12,24),
            new DateTime(2020,12,25),
            new DateTime(2020,12,26),
            new DateTime(2021,1,1),
            new DateTime(2021,4,2),
            new DateTime(2021,4,5),
            new DateTime(2021,5,1),
            new DateTime(2021,5,8),
            new DateTime(2021,7,5),
            new DateTime(2021,7,6),
            new DateTime(2021,9,28),
            new DateTime(2021,10,28),
            new DateTime(2021,11,17),
            new DateTime(2021,12,24),
            new DateTime(2021,12,25),
            new DateTime(2021,12,26),
            new DateTime(2022,1,1),
            new DateTime(2022,4,15),
            new DateTime(2022,4,18),
            new DateTime(2022,5,1),
            new DateTime(2022,5,8),
            new DateTime(2022,7,5),
            new DateTime(2022,7,6),
            new DateTime(2022,9,28),
            new DateTime(2022,10,28),
            new DateTime(2022,11,17),
            new DateTime(2022,12,24),
            new DateTime(2022,12,25),
            new DateTime(2022,12,26),
        };

        public static DateTime add30WorkDays(DateTime startDate)
        {
            var date = startDate.Date;
            DayOfWeek dayInWeek;

            int days = 30;

            while (days != 0)
            {
                dayInWeek = date.DayOfWeek;
                if (dayInWeek == DayOfWeek.Sunday || dayInWeek == DayOfWeek.Saturday)
                {
                    date = date.AddDays(1);
                }
                else
                {
                    if (statniSvatky.Contains(date))
                    {
                        date = date.AddDays(1);
                    }
                    else
                    {
                        date = date.AddDays(1);
                        days--;

                    }
                }
            }
            return date;
        }
    }
}
