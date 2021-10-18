using BookApi.ModelView;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using static BookApi.ModelView.ResponseModelView;

namespace BookServices
{
    /// <summary>
    /// Summary description for BookService
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
   
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BookService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld(string n)
        {
            return "Hello World"+n;
        }
        private DateTime AddDate(DateTime Date, double number)
        {
           DateTime d= Date.AddDays(number);
            return d;
        }
        public string converttodate(DateTime dateTime)
        {

            DateTime dt = DateTime.ParseExact(dateTime.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string s = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            return s;
        }
        public DateTime CalulateDate(DateTime start,int AfterIndex, int[] Days)
        {
            int NumToAdd = 0;
            if (AfterIndex == 0)
                NumToAdd = 7 - Days[Days.Length-1] + Days[0];
            else
                NumToAdd = Days[AfterIndex] - Days[AfterIndex-1];

            return AddDate(start, NumToAdd);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Get(DateTime StartDate, int[] Days, int num)
        {
            //int[] Days =new int[2] { 1,3};
            ResponseModelView response = new ResponseModelView();
            List<SessionsScheduled> list = new List<SessionsScheduled>();
            DateTime lastdate =StartDate;
            SessionsScheduled chapterDetails = new SessionsScheduled();
            List<string> ChapterDays = new List<string>();
            int count = 1;
            int DayOfNumber = 0;
            int index = 0;
            for (int item = 1; item <= 5*(num+1); item++)
            { 
                if (count <= num)
                {
                    chapterDetails.ChapterNumber = item-index;
                    index++;
                    if (item == 1 && DayOfNumber == 0)
                    {
                        ChapterDays.Add((StartDate.ToShortDateString()));
                        lastdate = StartDate;
                    } 
                    else
                    {
                        DateTime Date;
                        Date = CalulateDate(lastdate, DayOfNumber, Days);
                        ChapterDays.Add(Date.ToShortDateString());
                        lastdate = Date;
                    }
                    count++;
                    DayOfNumber = (++DayOfNumber) %(Days.Length);
                }
                else
                {
                    chapterDetails.SessionsDate = ChapterDays;
                    list.Add(chapterDetails);
                    chapterDetails = new SessionsScheduled();
                    ChapterDays = new List<string>();
                    count = 1;
                }
        }
            response.Sessions = list;
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(response));
        }
    }
}
