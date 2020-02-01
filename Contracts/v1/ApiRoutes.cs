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

        public static class Languages
        {
            public const string Create = Base + "/Languages";

            public const string Get = Base + "/Languages/{languageTitle}";

            public const string GetAll = Base + "/Languages";

            public const string Update = Base + "/Languages/{LanguagesId}";

            public const string Delete = Base + "/Languages/{LanguagesId}";
        }

        public static class Authors
        {
            public const string Create = Base + "/Authors";

            public const string Get = Base + "/Authors/{AuthorId}";

            public const string GetAll = Base + "/Authors";

            public const string Update = Base + "/Authors/{AuthorId}";

            public const string Delete = Base + "/Authors/{AuthorId}";
        }

        public static class Books
        {
            public const string Create = Base + "/Books";

            public const string Get = Base + "/Books/{BookId}";

            public const string GetAll = Base + "/Books";

            public const string Update = Base + "/Books/{BookId}";

            public const string Delete = Base + "/Books/{BookId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/Identity/Login";

            public const string Register = Base + "/Identity/Register";

            public const string GetProfile = Base + "/Identity/UserProfile";

            public const string GetAllUsers = Base + "/Identity/Users";

            public const string DeleteUserByEmail = Base + "/Identity/Users/Delete/{userEmail}";
        }
    }
}