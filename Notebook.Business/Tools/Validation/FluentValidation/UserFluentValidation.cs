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
            RuleFor(p => p.ID).NotEmpty().MaximumLength(8);
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Email).NotEmpty();
            RuleFor(p => p.Email).EmailAddress();
            RuleFor(p => p.Username).NotEmpty();
            RuleFor(p => p.Username).Must(UsernameTagControl).WithMessage("Custom character unavailable");
        }

        public void Validator(object entity)
        {
            var result = base.Validate((User)entity);

            if (result.Errors.Count > 0)
            {
                string exceptionMessage = "";

                foreach (var error in result.Errors)
                {
                    exceptionMessage += string.Format("Property Name: {0} | Message: {1} <br/>", error.PropertyName,error.ErrorMessage);
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

        private bool UsernameTagControl(string username)
        {
            if (username.IsThereTurkishCharacter() || username.IsThereHtmlTag("_"))
            {
                return false;
            }

            return true;
        }
    }
}
