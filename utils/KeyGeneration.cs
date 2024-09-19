public static class KeyGeneration
{
    private static IConfiguration _configuration;

    // This method can be called once to set the IConfiguration object
    public static void SetConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetSecureKey()
    {
        var signingKey = _configuration["JwtSettings:SigningKey"];
        if (string.IsNullOrEmpty(signingKey))
        {
            throw new InvalidOperationException("JWT signing key is not set.");
        }
        return signingKey;
    }
}



