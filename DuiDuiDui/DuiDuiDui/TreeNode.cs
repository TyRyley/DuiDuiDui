using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuiDuiDui
{
    public class TreeNode <T>
    {
        // TreeNode var
        public T Data { get; set; }
        public T Parent { get; set; }
        public List<TreeNode<T>> Children { get; set; }
    }
}
