using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace Common.Base
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            ValidationObjects = new List<ValidationObject>();    
        }

        public List<ValidationObject> ValidationObjects { get; set; }

        public static object Lock = new object();

        public void AddError(string key, string value)
        {
            lock (Lock)
            {
                var obj = ValidationObjects.FirstOrDefault(x => x.Key == key);
                if (obj == null)
                {
                    obj = new ValidationObject { Key = key };
                    ValidationObjects.Add(obj);
                }
                obj.Lines.Add(value);
            }
        }

        public bool IsValid
        {
            get { return ValidationObjects.IsEmpty(); }
        }

      
    }
}