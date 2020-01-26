using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Contracts
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        private const string Base = Root + "/" + Version;
   
        public static class Categories
        {
            public const string Create = Base + "/Categories";

            public const string Get = Base + "/Categories/{categoryTitle}";

            public const string GetAll = Base + "/Categories";

            public const string Update = Base + "/Categories/{CategoryId}";

            public const string Delete = Base + "/Categories/{CategoryId}";
        }

        public static class Authors
        {
            public const string Create = Base + "/Authors";

            public const string Get = Base + "/Authors/{AuthorId}";

            public const string GetAll = Base + "/Authors";

            public const string Update = Base + "/Authors/{AuthorId}";

            public const string Delete = Base + "/Authors/{AuthorId}";
        }

        public static class Book
        {
            public const string Create = Base + "/Books";

            public const string Get = Base + "/Books/{BookId}";
        }
    }
}