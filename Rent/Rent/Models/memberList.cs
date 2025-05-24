using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent.Models
{
    public class memberList
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public List<Member> Data { get; set; }
    }
}