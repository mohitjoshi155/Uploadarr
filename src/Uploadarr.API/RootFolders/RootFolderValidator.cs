using FluentValidation;
using Uploadarr.Data;

namespace Uploadarr.API
{
    public class RootFolderValidator : AbstractValidator<RootFolderDTO>
    {
        public RootFolderValidator()
        {
            RuleFor(x => x.Path).NotEmpty();
        }
    }
}
