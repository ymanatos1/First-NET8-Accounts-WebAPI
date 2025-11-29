//using FluentValidation;
//using WebAPI.Data.Models;

//public class AccountCategoryValidator : AbstractValidator<AccountCategory>
//{
//    public AccountCategoryValidator()
//    {
//        // -------------------------
//        // NAME RULES (single source)
//        // -------------------------
//        RuleFor(x => x.Name)
//            .NotEmpty()
//            .MaximumLength(20)
//            .Matches(@"^[A-Za-z][A-Za-z0-9_-]*$")
//            .WithMessage(
//              "Name must start with a letter and contain only letters, digits, - or _. Max 20 characters.");

//        // -------------------------
//        // DESCRIPTION (UI required)
//        // -------------------------
//        RuleFor(x => x.Description)
//            .NotEmpty()                     // ✅ Now required in UI only
//            .MaximumLength(200);

//    }
//}
