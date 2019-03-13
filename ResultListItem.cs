using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omlenet
{
    public class ResultListItem
    {
        public string Name;
        public int Id;
        public int Mass;

        public override string ToString()
        {
            return Name + " (" + Mass + "g)";
        }

        public override bool Equals(object obj)
        {
            //Note: I expect you won't be trying to compare ResultListItems to other types of objects
            return obj is ResultListItem && ((ResultListItem)obj).Id == Id && ((ResultListItem)obj).Mass == Mass;
        }

        public override int GetHashCode()
        {
            return Id * 10000 + Mass;
        }
    }
}
