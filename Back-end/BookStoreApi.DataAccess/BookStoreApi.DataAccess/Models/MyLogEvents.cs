namespace BookStoreApi.Models
{
    public class MyLogEvents
    {
        public const int GenerateItems = 1000;
        public const int ListItems = 1001;
        public const int GetItem = 1002;
        public const int InsertItem = 1003;
        public const int UpdateItem = 1004;
        public const int DeleteItem = 1005;

        public const int Error = 3000;

        public static string ShowObject(object val)
        {
           return Newtonsoft.Json.JsonConvert.SerializeObject(val);
        }
    }
    public class MyLogEventTitle
    {
        public const string ListItems = "GET (ListItem)";
        public const string GetItem = "GET (ItemById)";
        public const string InsertItem = "POST";
        public const string UpdateItem = "PUT";
        public const string DeleteItem = "DELETE";
        public const string Error = "ERROR";
    }
}
