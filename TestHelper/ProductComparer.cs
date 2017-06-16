using BusinessEntities;
using DataModel.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsHelper
{
    public class ProductComparer : IComparer, IComparer<Product>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as Product;
            var rhs = actual as Product;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(Product expected, Product actual)
        {
            int temp;
            return (temp = expected.Id.CompareTo(actual.Id)) != 0 ? temp
                : (temp = expected.Name.CompareTo(actual.Name)) != 0 ? temp
                : (temp = Nullable.Compare(expected.CreateDate, actual.CreateDate)) != 0 ? temp
                : expected.isActive.CompareTo(actual.isActive);
        }
    }
}
