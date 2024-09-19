using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class DealerService : IDealerService
{
    private readonly DealerContext _context;

    public DealerService(DealerContext context)
    {
        _context = context;
    }

    public async Task<Dealer> RegisterDealer(RegisterDealer dealer)
    {

        // Check if the email is already registered
        if (await _context.Dealers.AnyAsync(d => d.Email == dealer.Email))
        {
            throw new ArgumentException("Email is already registered.");
        }

        // Hash the password
        dealer.Password = BCrypt.Net.BCrypt.HashPassword(dealer.Password);

        var newDealer = new Dealer()
        {
            DealerName = dealer.DealerName,
            Email = dealer.Email,
            Password = dealer.Password
        };

        _context.Dealers.Add(newDealer);
        await _context.SaveChangesAsync();

        return newDealer;
    }

    public async Task<string> Login(LoginDealer loginDetails)
    {
        // Verify dealer's email and password
        var dealer = await _context.Dealers.FirstOrDefaultAsync(d => d.Email == loginDetails.Email);

        if (dealer == null || !BCrypt.Net.BCrypt.Verify(loginDetails.Password, dealer.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(KeyGeneration.GetSecureKey());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, dealer.DealerId.ToString()),
                new Claim(ClaimTypes.Name, dealer.DealerName)
            }),
            Expires = DateTime.UtcNow.AddMinutes(60), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
