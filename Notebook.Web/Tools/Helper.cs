using Microsoft.AspNetCore.Http;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Helper
{
    
    //public static string Pagination(string Url, int ActivePage, int TotalPage, int Interval = 4)
    //{
    //    if (TotalPage < 2) return "";

    //    string symbol = (Url.Contains("?")) ? "&" : "?";
    //    string _url = Url + symbol + "Page=" + 1;
    //    string result = "<ul class='pagination pagination-sm no-margin pull-right'><li><a href=" + _url + ">&laquo;</a></li>";

    //    if (TotalPage <= (Interval * 2 + 1))
    //    {
    //        for (int i = 1; i <= TotalPage; i++)
    //        {
    //            _url = Url + symbol + "Page=" + i;
    //            result += (i == ActivePage) ? $"<li><a href={_url} class='active'>{i}</a></li>" : $"<li><a href={_url}>{i}</a></li>";
    //        }
    //    }
    //    else
    //    {
    //        int start = 1;
    //        int finish = TotalPage;

    //        if (ActivePage <= (Interval + 1))
    //        {
    //            finish = Interval * 2 + 1;
    //        }
    //        else if (ActivePage > (Interval + 1))
    //        {
    //            start = (TotalPage > (ActivePage + Interval)) ? ActivePage - Interval : TotalPage - Interval * 2;
    //            finish = (TotalPage > (ActivePage + Interval)) ? ActivePage + Interval : TotalPage;
    //        }

    //        for (int i = start; i <= finish; i++)
    //        {
    //            _url = Url + symbol + "Page=" + i;
    //            result += (i == ActivePage) ? $"<li><a href={_url} class='active'>{i}</a></li>" : $"<li><a href={_url}>{i}</a></li>";
    //        }
    //    }

    //    _url = Url + symbol + "Page=" + TotalPage;
    //    result += "<li><a href=" + _url + ">&raquo;</a></li></ul>";
    //    return result;
    //}
}
