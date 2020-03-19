using FluentValidation;
using Uploadarr.Data;

namespace Uploadarr.API
{
    public class RootFolderValidator : AbstractValidator<RootFolder>
    {
        public RootFolderValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
