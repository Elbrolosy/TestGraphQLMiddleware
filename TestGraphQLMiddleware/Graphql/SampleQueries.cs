using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;

namespace TestGraphQLMiddleware.Graphql
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class SampleQueries
    {
        [Authorize(Policy = "SalesDepartment")]
        public string testQuery()
        {
            return "Done";
        }
    }
}