using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authentication.Cookies;
using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TestGraphQLMiddleware.Graphql
{
    // this doesn't work
    public class SalesDepartmentAuthorizationHandler
    : AuthorizationHandler<SalesDepartmentRequirement, IResolverContext>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SalesDepartmentRequirement requirement,
            IResolverContext resource)
        {
            /*if (context.User.HasClaim(...))
            {
            }*/
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
    public class SalesDepartmentRequirement : IAuthorizationRequirement { }

    // this does work if I configured it into the startup instead
    public class BasicAuthorizationHandler
    : HotChocolate.AspNetCore.Authorization.IAuthorizationHandler
    {
        public ValueTask<AuthorizeResult> AuthorizeAsync(IMiddlewareContext context, AuthorizeDirective directive)
        {
            /*if (context.User.HasClaim(...))
            {
            }*/
            return new ValueTask<AuthorizeResult>(AuthorizeResult.Allowed);
        }
    }

}
