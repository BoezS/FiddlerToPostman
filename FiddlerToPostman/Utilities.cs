using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FiddlerToPostman
{
    public class Utilities
    {
        public static string ToJSONString(object obj)
        {
            string jsonString = "";
            List<string> stringSegments = new List<string>();

            PropertyInfo[] propertyInfoList = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (propertyInfoList != null)
            {
                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    if (propertyInfo.DeclaringType != typeof(object))
                    {
                        var propertyValue = propertyInfo.GetValue(obj, null);

                        if (propertyValue != null)
                        {
                            if (propertyInfo.PropertyType.GetInterface("IEnumerable") != null)
                            {
                                if (propertyInfo.PropertyType.Name.ToLower() != "string")
                                {
                                    List<string> listItems = new List<string>();

                                    foreach (object item in (Array)propertyValue)
                                    {
                                        if (item.GetType().Name.ToLower() != "string")
                                        {
                                            if (item.GetType().IsClass)
                                            {
                                                listItems.Add(ToJSONString(item));
                                            }
                                            else
                                            {
                                                listItems.Add(item.ToString());
                                            }
                                        }
                                        else
                                        {
                                            listItems.Add(string.Format("\"{0}\"", item));
                                        }
                                    }

                                    if (listItems.Count > 0)
                                    {
                                        stringSegments.Add(string.Format("\"{0}\":[{1}]", propertyInfo.Name, string.Join(",", listItems.ToArray())));
                                    }
                                }
                                else
                                {
                                    stringSegments.Add(string.Format("\"{0}\":\"{1}\"", propertyInfo.Name, propertyValue));
                                }
                            }
                            else
                            {
                                if (propertyInfo.PropertyType.IsClass)
                                {
                                    stringSegments.Add(ToJSONString(propertyValue));
                                }
                                else
                                {
                                    stringSegments.Add(string.Format("\"{0}\":{1}", propertyInfo.Name, propertyValue));
                                }
                            }
                        }
                    }
                }

                if (stringSegments.Count > 0) 
                { 
                    jsonString = "{" + string.Join(",", stringSegments.ToArray()) + "}"; 
                }
            }

            return jsonString;
        }
    }
}
