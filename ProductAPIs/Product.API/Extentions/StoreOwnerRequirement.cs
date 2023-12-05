using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Product.API.Extentions
{
    public record StoreOwnerRequirement : IAuthorizationRequirement;

    public class StoreOwnerHandler : AuthorizationHandler<StoreOwnerRequirement>
    {
        public StoreOwnerHandler()
        {
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StoreOwnerRequirement requirement)
        {
            var httpContext = (HttpContext)context.Resource!;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            // Read the original request body
            var originalBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();

            // Create a new MemoryStream with the original body content
            var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(originalBody));

            // Replace the original request body with the new MemoryStream
            httpContext.Request.Body = bodyStream;

            try
            {
                // Extract ownerId using a simple regex (adjust as needed)
                var match = Regex.Match(originalBody, "\"ownerId\"\\s*:\\s*\"(.*?)\"");
                if (match.Success && Guid.TryParse(match.Groups[1].Value, out var ownerId))
                {
                    var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
                    if (Guid.TryParse(userId, out var parsedUserId) && ownerId == parsedUserId)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
            finally
            {
                // Restore the original request body
                httpContext.Request.Body = originalBody.ToStream();
            }

            context.Fail();
        }
    }
}
