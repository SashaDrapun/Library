using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static  class DateParser
    {
        public static string FromStringToDate(string date)
        {
            if(date == null)
            {
                return "";
            }
            else
            {
                var value = date.Split(new char[] { ' ' });
                return value[0];
            }
            
        }

        public static string FromNormalTimeToBdTime(string normalTime)
        {
            char[] values = normalTime.ToCharArray();

            int day = Convert.ToInt32(values[0].ToString() + values[1].ToString());

            int month = Convert.ToInt32(values[3].ToString() + values[4].ToString());

            int year = Convert.ToInt32(values[6].ToString() + values[7].ToString() + values[8].ToString() + values[9].ToString());


            string bdTime = year + "." + month + "." + day;

            return bdTime;
        }
    }
}
