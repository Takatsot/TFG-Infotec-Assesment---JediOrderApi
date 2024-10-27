using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class ProductSpecParams
    {

        private const int MaxPageSize = 20;

        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6; 

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        private  List<string> _types = [];
        public List<string> Type 
        { 
            get => _types; 
            set 
            {
                _types = value.SelectMany(x => x.Split(',',
                    StringSplitOptions.RemoveEmptyEntries)).ToList();
            } 
        }

        private List<string> _name = [];
        public List<string> Name
        {
            get => _name;
            set
            {
                _name = value.SelectMany(x => x.Split(',',
                    StringSplitOptions.RemoveEmptyEntries)).ToList();
            }
        }
        public string? Sort { get; set; }

        private string? _search;
        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();   
        }
    }
}
