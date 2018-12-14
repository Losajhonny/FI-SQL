using Irony.Parsing;
using ServidorDB.arboles.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.analizadores.xml
{
    class RowAst
    {
        public static List<List<Atributo>> ROWS(ParseTreeNode padre)
        {
            List<List<Atributo>> l = new List<List<Atributo>>();
            for(int i = 0; i < padre.ChildNodes.Count; i++)
            {
                List<Atributo> atrs = ProcAst.PARAMS(padre.ChildNodes[i]);
                foreach (Atributo atr in atrs)
                {
                    atr.Nombre = atr.Nombre.Trim('~');
                }
                if(atrs.Count > 0)
                {
                    l.Add(atrs);
                }
            }
            return l;
        }
    }
}
