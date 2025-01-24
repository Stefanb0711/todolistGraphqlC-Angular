using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenService
{


    private readonly IConfiguration _configuration;
	private readonly byte[] _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _tokenValidityInMinutes;


    public JwtTokenService(IConfiguration configuration)
	{
		//_configuration = configuration;
        _key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]); // Geheimschlüssel für Token-Signatur
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _tokenValidityInMinutes = int.Parse(configuration["Jwt:TokenValidityInMinutes"]);
    }


    private const int TokenValidityInMinutes = 60; // Token-Gültigkeit (in Minuten)

	public string GenerateToken(string userId, string username, string email)
	{
		

		// Schlüssel generieren
		//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
		var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);

		// Claims definieren
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, username),
			new Claim(JwtRegisteredClaimNames.Email, email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Eindeutige ID
            new Claim("UserId", userId) // Benutzerdefinierte Claims
            };

		// Token erstellen
		var token = new JwtSecurityToken(
			issuer: _issuer, // Herausgeber
			audience: _audience, // Zielgruppe
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_tokenValidityInMinutes),
			signingCredentials: credentials);

		// Token als String zurückgeben
		return new JwtSecurityTokenHandler().WriteToken(token);
	}


    public string GetUserIdFromJwt(string token)
    {
        //Console.WriteLine("Der zu decodierende Token: " + token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token); // Token wird nicht validiert, nur gelesen.

        // Benutzerdefinierten Claim "UserId" auslesen
        var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("UserId konnte nicht gefunden werden.");
            return null;
        }

        //Console.WriteLine("Gefundene UserId: " + userId);
        return userId;
    }


	public ClaimsPrincipal ValidateToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_key),
            ValidateIssuer = !string.IsNullOrEmpty(_issuer),
            ValidIssuer = _issuer,
            ValidateAudience = !string.IsNullOrEmpty(_audience),
            ValidAudience = _audience,
            ClockSkew = TimeSpan.Zero
        };

		try
		{
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Überprüfen, ob das Token ein JWT-Token ist
            if (validatedToken is JwtSecurityToken jwtToken)
            {
                // Optional: Weitere Prüfungen, z.B. Algorithmen
                if (jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal; // Token ist gültig
                }
            }
        }
        catch (SecurityTokenException ex)
        {
            Console.WriteLine($"Token-Fehler: {ex.Message}");

            // Token ist ungültig
        }
        catch (Exception ex)
        {
            // Ein anderer Fehler ist aufgetreten
            Console.WriteLine($"Fehler bei der Token-Validierung: {ex.Message}");
        }

        return null; // Token ist ungültig oder konnte nicht validiert werden
    }



}