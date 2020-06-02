using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTotal.Master.Model
{
    public class sysMenu
    {
        public string name { get; set; }
        public string directory { get; set; }
        public string category { set; get; }
        public string type { get; set; }
        public List<sysMenuItem> samples { get; set; }
        public int order { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string uid { get; set; }
    }


    public class sysMenuItem
    {
        public string url { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string uid { get; set; }
        public string type { get; set; }
        public int order { get; set; }
        public string component { get; set; }
        public string dir { get; set; }
        public bool hidden { get; set; }
        public string parentId { get; set; }
    }
  
}
