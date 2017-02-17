using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Enterprise.Component.Infrastructure
{
    public class ConvertToJson
    {
        public string JsonData
        { get; set; }
        public ConvertToJson()
        { }
        public ConvertToJson(DataTable dt, int page, int pageSize)
        {
            ExcuteResultToJson(dt, page, pageSize);
        }
        private void ExcuteResultToJson(DataTable dt, int page, int pageSize)
        {
            int rowbegin = (page - 1) * pageSize;
            int rowend = page * pageSize;
            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            if (rowbegin <= dt.Rows.Count)
            {
                JsonData = "{\"total\":" + dt.Rows.Count + ",\"rows\":[";
                for (int i = rowbegin; i <= rowend - 1; i++)
                {
                    JsonData += "{";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        JsonData += "\"" + dc.ColumnName + "\":\"" + dt.Rows[i][dc.ColumnName] + "\",";
                    }
                    JsonData = JsonData.Remove(JsonData.Length - 1);
                    JsonData += "},";

                }
                JsonData = JsonData.Remove(JsonData.Length - 1);
                JsonData += "]}";
            }
        }
    }
}