using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>
/// Registers users, authenticates credentials, and creates authentication results.
/// </summary>
public sealed class AuthService
{
    /// <summary>Provides storage for retrieving and saving user accounts.</summary>
    private readonly IUserRepository _userRepository;

    /// <summary>Issues access tokens for successfully authenticated users.</summary>
    private readonly IAuthenticationTokenService _tokenService;

    /// <summary>
    /// Creates the service with user storage and token-generation dependencies.
    /// </summary>
    /// <param name="userRepository">Storage used to find and save users.</param>
    /// <param name="tokenService">Service used to issue authentication tokens.</param>
    public AuthService(IUserRepository userRepository, IAuthenticationTokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Validates and registers a user, then returns the user and a sign-in token.
    /// </summary>
    /// <param name="request">The name, email, and password for the new learner account.</param>
    /// <param name="cancellationToken">Cancels the storage operations if requested.</param>
    /// <returns>The registered user, access token, and token expiration time.</returns>
    /// <exception cref="InvalidOperationException">The request is invalid or its email is already registered.</exception>
    public async Task<AuthenticationResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRegistration(request);

        var email = NormalizeEmail(request.Email);
        var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            Role = UserRole.Learner,
            PasswordHash = PasswordHasher.HashPassword(request.Password),
        };

        var savedUser = await _userRepository.SaveAsync(user, cancellationToken);
        var expiresAtUtc = DateTimeOffset.UtcNow.AddHours(6);

        return new AuthenticationResult(savedUser, _tokenService.CreateToken(savedUser), expiresAtUtc);
    }

    /// <summary>
    /// Checks a user's credentials, records the login time, and issues a token.
    /// </summary>
    /// <param name="request">The email and password to authenticate.</param>
    /// <param name="cancellationToken">Cancels the storage operations if requested.</param>
    /// <returns>The authenticated user, access token, and token expiration time.</returns>
    /// <exception cref="InvalidOperationException">Required credentials are missing or do not match a user.</exception>
    public async Task<AuthenticationResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidOperationException("Email and password are required.");
        }

        var email = NormalizeEmail(request.Email);
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        user.LastLoginAtUtc = DateTimeOffset.UtcNow;
        await _userRepository.SaveAsync(user, cancellationToken);

        var expiresAtUtc = DateTimeOffset.UtcNow.AddHours(6);
        return new AuthenticationResult(user, _tokenService.CreateToken(user), expiresAtUtc);
    }

    /// <summary>
    /// Converts a user to the public summary returned by authentication endpoints.
    /// </summary>
    /// <param name="user">The user whose public details are summarized.</param>
    /// <returns>The user's ID, name, email, and role without password data.</returns>
    public static UserSummary ToSummary(User user) => new(user.Id, user.Name, user.Email, user.Role);

    /// <summary>
    /// Trims surrounding whitespace from an email address.
    /// </summary>
    /// <param name="email">The email address to normalize.</param>
    /// <returns>The trimmed email address.</returns>
    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// Checks that registration details contain a name, plausible email, password, and supported role.
    /// </summary>
    /// <param name="request">The registration details to validate.</param>
    /// <exception cref="InvalidOperationException">A required value is missing or invalid.</exception>
    private static void ValidateRegistration(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidOperationException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        if (!request.Email.Contains('@', StringComparison.Ordinal) || !request.Email.Contains('.', StringComparison.Ordinal))
        {
            throw new InvalidOperationException("A valid email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            throw new InvalidOperationException("Password must be at least 8 characters long.");
        }

    }
}
