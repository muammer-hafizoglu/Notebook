using Notebook.Core.CrossCuttingConcerns.Validaton;

namespace Notebook.Core.CrossCuttingConcerns.Validation.Abstract
{
    public interface IValidation
    {
        void Validator(object entity);
    }
}
