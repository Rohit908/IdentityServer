using System.Collections.Generic;

namespace ApiOne.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Permission.{module}.Create",
            $"Permission.{module}.View",
            $"Permission.{module}.Edit",
            $"Permission.{module}.Delete",
        };
        }

        public static class Products
        {
            public const string View = "Permission.Products.View";
            public const string Create = "Permission.Products.Create";
            public const string Edit = "Permission.Products.Edit";
            public const string Delete = "Permission.Products.Delete";
        }
    }
}
