using FluentValidation;
using Notebook.Core.CrossCuttingConcerns.Validation.Abstract;
using Notebook.Core.CrossCuttingConcerns.Validaton;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;

namespace Notebook.Business.Tools.Validation.FluentValidation
{
    public class NoteFluentValidation : AbstractValidator<Note>, IValidation
    {
        public NoteFluentValidation()
        {
            RuleFor(p => p.Title).NotEmpty();
            RuleFor(p => p.Content).NotEmpty();
        }

        public void Validator(object entity)
        {
            var result = base.Validate((Note)entity);

            if (result.Errors.Count > 0)
            {
                string exceptionMessage = "";

                foreach (var error in result.Errors)
                {
                    exceptionMessage += string.Format("Property Name: {0} | Message: {1} <br/>", error.PropertyName,error.ErrorMessage);
                }

                throw new System.Exception(exceptionMessage);
            }
        }
    }
}
