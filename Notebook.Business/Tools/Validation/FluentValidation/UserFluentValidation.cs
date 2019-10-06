using FluentValidation;
using Notebook.Core.CrossCuttingConcerns.Validation.Abstract;
using Notebook.Core.CrossCuttingConcerns.Validaton;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;

namespace Notebook.Business.Tools.Validation.FluentValidation
{
    public class UserFluentValidation : AbstractValidator<User>, IValidation
    {
        public UserFluentValidation()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Password).NotEmpty();
        }

        public void Validator(object entity)
        {
            var result = base.Validate((User)entity);

            if (result.Errors.Count > 0)
            {
                string exceptionMessage = "";

                foreach (var error in result.Errors)
                {
                    exceptionMessage += string.Format("{0} <br/>",error.ErrorMessage);
                }
                //var exception = new ValidateException();

                //foreach (var error in result.Errors)
                //{

                //    exception.propertyExceptions.Add(new PropertyException
                //    {
                //        ErrorCode = error.ErrorCode,
                //        ErrorMessage = error.ErrorMessage,
                //        PropertyName = error.PropertyName,
                //        ResourceName = error.ResourceName
                //    });
                //}

                throw new System.Exception(exceptionMessage);
            }
        }
    }
}
