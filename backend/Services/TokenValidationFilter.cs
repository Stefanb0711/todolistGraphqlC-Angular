using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace todListBackend.Services
{
    public class TokenValidationFilter: ActionFilterAttribute
    {
        private readonly JwtTokenService _jwtTokenService;


        public TokenValidationFilter(JwtTokenService jwtTokenService) {
            _jwtTokenService = jwtTokenService;
        }

       
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();


            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // Hier kannst du den Token validieren oder weiterverarbeiten

                var claimsPrincipal = _jwtTokenService.ValidateToken(token);

                if (claimsPrincipal != null)
                {
                    Console.WriteLine("Token ist gültig.");

                    var userId = _jwtTokenService.GetUserIdFromJwt(token);

                    context.HttpContext.Items["UserId"] = userId;

                    // Setze ClaimsPrincipal in HttpContext.User (optional)
                    context.HttpContext.User = claimsPrincipal;
                }
                else
                {
                    Console.WriteLine("Token abgelaufen oder ungültig.");
                    context.Result = new UnauthorizedObjectResult(new
                    {
                        Message = "Ungültiger oder abgelaufener Token. Zugriff verweigert."
                    });
                    return; // Anfrage abbrechen
                }
            }
            else
            { 
                // Keine gültige Autorisierung
                context.Result = new UnauthorizedResult();
            }

            // Versuche, den Authorization-Header zu lesen

            /*
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                var jwtToken = token.ToString().Replace("Bearer", "").Trim();
                Console.WriteLine("JWT-Token von Frontend im Actionfilter:\n\n" + jwtToken);

                // Validieren des Tokens
                var claimsPrincipal = _jwtTokenService.ValidateToken(jwtToken);

                if (claimsPrincipal != null)
                {
                    Console.WriteLine("Token ist gültig.");
                    // Setze ClaimsPrincipal in HttpContext.User (optional)
                    context.HttpContext.User = claimsPrincipal;
                }
                else
                {
                    Console.WriteLine("Token abgelaufen oder ungültig.");
                    context.Result = new UnauthorizedObjectResult(new
                    {
                        Message = "Ungültiger oder abgelaufener Token. Zugriff verweigert."
                    });
                    return; // Anfrage abbrechen
                }
            }
            else
            {
                Console.WriteLine("Authorization-Header fehlt.");
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "Authorization-Header fehlt. Zugriff verweigert."
                });
                return; // Anfrage abbrechen
            }
            */
            base.OnActionExecuting(context);
        }

    }


    
}
