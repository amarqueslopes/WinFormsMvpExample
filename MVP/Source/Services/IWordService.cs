using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Source.Services
{
    public interface IWordService
    {
        bool FindNext(string text);
    }
}
