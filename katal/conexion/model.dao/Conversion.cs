using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.dao
{
    public class Conversion
    {
        public static  decimal ParseDecimal(string data)
        {
            if (data == "")
            {
                return 0;
            }
            else
            {
                return Decimal.Parse(data);
            }
        }
        public static DateTime ParseDateTime(string data)
        {
            if (data == "")
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.Parse(data);
            }
        }
        public static int Parseint(string data)
        {
            if (data == "")
            {
                return 0;
            }
            else
            {
                return int.Parse(data);
            }
        }
        public static bool ParseBool(string data)
        {
            if (data == "")
            {
                return false;
            }
            else
            {
                return bool.Parse(data);
            }
            
        }
        public static bool ParseBool(bool data)
        {
            if (data)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}