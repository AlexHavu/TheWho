using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tipalti.TheWho.Enums;

namespace Tipalti.TheWho.Models
{
    public class IBaseSearchResult
    {
    }
    public class BaseSearchResult: IBaseSearchResult
    {
        public int Id { get; set; }
        public eDocumentType DocumentType { get;set;}
        public virtual void SetPrevew() { }


    }
}
