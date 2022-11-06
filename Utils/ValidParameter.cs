using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Utils
{
    public static class ValidParameter
    {
        public static SqlParameter CreateParameter(string paramName, object value, DbType? paramType)
        {
            
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = paramName;


            //value.GetType() == typeof(DateTime)
            if (value == null)
            {
                value = DBNull.Value;
                parameter.Value = value;

            }
            else if (value.GetType() == typeof(DateTime))
            {
                string[] dateChain = ((DateTime)value).GetDateTimeFormats();

                for (int i = 0; i < dateChain.Length; i++)
                {
                    //System.Diagnostics.Debug.WriteLine(dateChain[i] + " :");
                    if (dateChain.ElementAt(20).Equals( "01/01/01 00:00"))
                    {
                        value = new DateTime(1900, 01,01);
                        parameter.DbType = DbType.DateTime;   
                        break;
                    }
                    
                    break;
                }

                parameter.Value = value;
            }

            else
            {
                parameter.Value = value;
            }
            
            if (paramType.HasValue)
            {
                parameter.DbType = paramType.Value;
            }
            

            return parameter;
        }
    }
}