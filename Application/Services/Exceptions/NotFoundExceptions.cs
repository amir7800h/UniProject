using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Exceptions
{
    public class NotFoundExceptions:Exception
    {
        public NotFoundExceptions(string entityName, object key)
            : base($"Entity  {entityName} with key {key} was not found.")
        {

        }
    }
}
