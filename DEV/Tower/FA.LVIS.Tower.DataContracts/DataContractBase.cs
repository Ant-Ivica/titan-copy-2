using System;
using System.Text;
using System.Reflection;

namespace FA.LVIS.Tower.DataContracts
{
    public abstract class DataContractBase
    {
        public DCState State { get; set; }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();

            Type thisType = this.GetType();

            strBuilder.Append("[ " + thisType.FullName + " {");

            PropertyInfo[] properties = thisType.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                strBuilder.Append(" " + pi.Name + ":" + (pi.GetValue(this) ?? "null").ToString() + "; ");
            }

            strBuilder.Append("} ]");

            return strBuilder.ToString();
        }
    }

    public enum DCState : int
    {
        Unknown = 0,
        Unchanged = 1,
        Modified = 2,
        New = 3,
        Deleted = 4
    }
}
