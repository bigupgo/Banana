using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;

namespace Banana.Core.Db
{
    public class ERTools
    {
        public static List<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T t = GetEntity<T>(row);
                list.Add(t);
            }
            return list;
        }

        public static T GetEntity<T>(DataRow row)
        {
            T t = Activator.CreateInstance<T>();
            PropertyInfo[] propertypes = t.GetType().GetProperties();
            foreach (PropertyInfo pro in propertypes)
            {
                string tempName = pro.Name;
                if (row.Table.Columns.Contains(tempName))
                {
                    object value = row[tempName];
                    if (!value.ToString().Equals(""))
                    {
                        pro.SetValue(t, ConvertValue(pro.PropertyType, value), null);
                    }
                    if (pro.PropertyType.Name.Equals("String") && pro.GetValue(t, null) == null && value.ToString().Equals(""))
                    {
                        if (pro.GetValue(t, null) == null)
                        {
                            pro.SetValue(t, ConvertValue(pro.PropertyType, ""), null);
                        }
                    }
                }
                else
                {
                    if (pro.PropertyType.Name.Equals("String") && pro.GetValue(t, null) == null)
                    {
                        pro.SetValue(t, ConvertValue(pro.PropertyType, ""), null);
                    }
                }
            }
            return t;
        }

        public static object ConvertValue(Type type, object value)
        {
            if (type.Equals(typeof(Int16)))
            {
                return Convert.ToInt16(value);
            }
            else if (type.Equals(typeof(Int32)))
            {
                return Convert.ToInt32(value);
            }
            else if (type.Equals(typeof(Int64)))
            {
                return Convert.ToInt64(value);
            }
            else if (type.Equals(typeof(UInt16)))
            {
                return Convert.ToUInt16(value);
            }
            else if (type.Equals(typeof(UInt32)))
            {
                return Convert.ToUInt32(value);
            }
            else if (type.Equals(typeof(UInt64)))
            {
                return Convert.ToUInt64(value);
            }
            else if (type.Equals(typeof(Boolean)))
            {
                return Convert.ToBoolean(value);
            }
            else if (type.Equals(typeof(Byte)))
            {
                return Convert.ToByte(value);
            }
            else if (type.Equals(typeof(Char)))
            {
                return Convert.ToChar(value);
            }
            else if (type.Equals(typeof(DateTime)))
            {
                return Convert.ToDateTime(value);
            }
            else if (type.Equals(typeof(Decimal)))
            {
                return Convert.ToDecimal(value);
            }
            else if (type.Equals(typeof(Double)))
            {
                return Convert.ToDouble(value);
            }
            else if (type.Equals(typeof(SByte)))
            {
                return Convert.ToSByte(value);
            }
            else if (type.Equals(typeof(Single)))
            {
                return Convert.ToSingle(value);
            }
            else if (type.Equals(typeof(String)))
            {
                return Convert.ToString(value);
            }
            return value;
        }

        public static T GetFirst<T>(DataTable table)
        {
            T t = default(T);
            if (table == null)
            {
                return t;
            }
            if (table.Rows.Count == 0)
            {
                return t;
            }
            DataRow row = table.Rows[0];
            return GetEntity<T>(row);
        }
    }
}