using FluentValidation;
using Notebook.Core.CrossCuttingConcerns.Validation.Abstract;
using Notebook.Core.CrossCuttingConcerns.Validaton;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;

namespace Notebook.Business.Tools.Validation.FluentValidation
{
    public class GroupFluentValidation : AbstractValidator<Group>, IValidation
    {
        public GroupFluentValidation()
        {
            RuleFor(p => p.Name).NotEmpty();
        }

        public void Validator(object entity)
        {
            var result = base.Validate((Group)entity);

            if (result.Errors.Count > 0)
            {
                string exceptionMessage = "";

                foreach (var error in result.Errors)
                {
                    exceptionMessage += string.Format("{0} <br/>", error.ErrorMessage);
                }

                throw new System.Exception(exceptionMessage);
            }
        }
    }
}
