using FluentValidation;

public class CarValidator : AbstractValidator<AddCar>
{
    public CarValidator()
    {
        RuleFor(c => c.Make)
            .NotEmpty().WithMessage("Make is required.")
            .Matches("^[a-zA-Z]+$").WithMessage("Make must contain only alphabetic characters.")
            .Length(2, 50).WithMessage("Make must be between 2 and 50 characters.");

        RuleFor(c => c.Model)
            .NotEmpty().WithMessage("Model is required.")
            .Length(2, 50).WithMessage("Model must be between 2 and 50 characters.");

        RuleFor(c => c.Year)
            .InclusiveBetween(DateTime.Now.Year - 100, DateTime.Now.Year).WithMessage($"Year must be between {DateTime.Now.Year - 100} and {DateTime.Now.Year}.");

        RuleFor(c => c.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock level must be a non-negative integer.");
    }
}

public class RegisterDealerValidator : AbstractValidator<RegisterDealer>
{
    private readonly DealerContext _context;

    public RegisterDealerValidator(DealerContext context)
    {
        _context = context;

        RuleFor(x => x.DealerName)
            .NotEmpty().WithMessage("Dealer name is required.")
            .Length(2, 50).WithMessage("Dealer name must be between 2 and 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
    }
}

public class LoginDealerValidator : AbstractValidator<LoginDealer>
{
    public LoginDealerValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
    }
}