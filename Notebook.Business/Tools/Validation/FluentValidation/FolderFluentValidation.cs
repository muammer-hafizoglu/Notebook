﻿using FluentValidation;
using Notebook.Core.CrossCuttingConcerns.Validation.Abstract;
using Notebook.Core.CrossCuttingConcerns.Validaton;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;

namespace Notebook.Business.Tools.Validation.FluentValidation
{
    public class FolderFluentValidation : AbstractValidator<Folder>, IValidation
    {
        public FolderFluentValidation()
        {
            RuleFor(p => p.Name).NotEmpty();
        }

        public void Validator(object entity)
        {
            var result = base.Validate((Folder)entity);

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
