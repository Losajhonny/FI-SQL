using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServidorDB.analizadores.usql;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;

namespace ServidorDB.arboles.usql.Expresiones.Aritmetica
{
    class NodoDiv : NodoExp
    {
        public NodoDiv(NodoExp izq, NodoExp der, int line, int colm) : base(izq, der, line, colm) { }

        public override object ejecutar(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.ejecutar(ent) : null;
            Resultado r2 = (der != null) ? (Resultado)der.ejecutar(ent) : null;
            int tipo = Constante.resta[r1.Tipo, r2.Tipo];

            if (r1.Tipo == Constante.INTEGER && r2.Tipo == Constante.INTEGER)
            {
                double v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                double v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);
                string res = (v2 != 0) ? (v1 / v2).ToString() : "0";

                bool ban = true;
                int cont = 0;
                for (int i = 0; i < res.Length; i++)
                {
                    if (ban)
                    {
                        if (res[i] == '.')
                        {
                            ban = false;
                        }
                    }
                    else
                    {
                        cont += 1;
                    }
                }

                if (cont >= 1)
                {
                    v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                    v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);

                    if (v2 != 0)
                    {
                        tipo = Constante.DOUBLE;
                        return new Resultado(tipo, (v1 / v2).ToString());
                    }
                    else
                    {
                        string descripcion = "La division entre '0' no es posible";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                        return new Resultado(Constante.ERROR, "");
                    }
                }
                else
                {
                    int va1 = Convert.ToInt32(r1.Valor);
                    int va2 = Convert.ToInt32(r2.Valor);

                    if (v2 != 0)
                    {
                        return new Resultado(tipo, (va1 / va2).ToString());
                    }
                    else
                    {
                        string descripcion = "La division entre '0' no es posible";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                        return new Resultado(Constante.ERROR, "");
                    }
                }
            }
            else if ((r1.Tipo == Constante.INTEGER || r1.Tipo == Constante.DOUBLE)
                && (r2.Tipo == Constante.INTEGER || r2.Tipo == Constante.DOUBLE))
            {
                double v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                double v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);

                if (v2 != 0)
                {
                    return new Resultado(tipo, (v1 / v2).ToString());
                }
                else
                {
                    string descripcion = "La division entre '0' no es posible";
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                    return new Resultado(Constante.ERROR, "");
                }
            }
            else if ((r1.Tipo == Constante.DATE || r1.Tipo == Constante.DATETIME)
                && r2.Tipo == Constante.TEXT)
            {
                return new Resultado(tipo, r1.Valor + r2.Valor);
            }
            else
            {
                if (!(r1.Tipo == Constante.ERROR || r2.Tipo == Constante.ERROR))
                {
                    string descripcion = "Tipos de operandos incorrectos para el operador '/'\nprimer tipo: "
                        + Constante.getTipo(r1.Tipo) + "\nsegundo tipo: " + Constante.getTipo(r2.Tipo);
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
                return new Resultado(Constante.ERROR, "");
            }
        }

        public override object generar_booleano(Entorno ent)
        {
            Resultado r1 = (izq != null) ? (Resultado)izq.generar_booleano(ent) : null;
            Resultado r2 = (der != null) ? (Resultado)der.generar_booleano(ent) : null;
            int tipo = Constante.resta[r1.Tipo, r2.Tipo];

            if (r1.Tipo == Constante.INTEGER && r2.Tipo == Constante.INTEGER)
            {
                double v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                double v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);
                string res = (v2 != 0) ? (v1 / v2).ToString() : "0";

                bool ban = true;
                int cont = 0;
                for (int i = 0; i < res.Length; i++)
                {
                    if (ban)
                    {
                        if (res[i] == '.')
                        {
                            ban = false;
                        }
                    }
                    else
                    {
                        cont += 1;
                    }
                }

                if (cont >= 1)
                {
                    v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                    v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);

                    if (v2 != 0)
                    {
                        tipo = Constante.DOUBLE;
                        return new Resultado(tipo, (v1 / v2).ToString());
                    }
                    else
                    {
                        string descripcion = "La division entre '0' no es posible";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                        return new Resultado(Constante.ERROR, "");
                    }
                }
                else
                {
                    int va1 = Convert.ToInt32(r1.Valor);
                    int va2 = Convert.ToInt32(r2.Valor);

                    if (v2 != 0)
                    {
                        return new Resultado(tipo, (va1 / va2).ToString());
                    }
                    else
                    {
                        string descripcion = "La division entre '0' no es posible";
                        uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                        return new Resultado(Constante.ERROR, "");
                    }
                }
            }
            else if ((r1.Tipo == Constante.INTEGER || r1.Tipo == Constante.DOUBLE)
                && (r2.Tipo == Constante.INTEGER || r2.Tipo == Constante.DOUBLE))
            {
                double v1 = Convert.ToDouble(r1.Valor, System.Globalization.CultureInfo.InvariantCulture);
                double v2 = Convert.ToDouble(r2.Valor, System.Globalization.CultureInfo.InvariantCulture);

                if (v2 != 0)
                {
                    return new Resultado(tipo, (v1 / v2).ToString());
                }
                else
                {
                    string descripcion = "La division entre '0' no es posible";
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                    return new Resultado(Constante.ERROR, "");
                }
            }
            else if ((r1.Tipo == Constante.DATE || r1.Tipo == Constante.DATETIME)
                && r2.Tipo == Constante.TEXT)
            {
                return new Resultado(tipo, r1.Valor + r2.Valor);
            }
            else if (r1.Tipo == Constante.ID || r2.Tipo == Constante.ID)
            {
                return new Resultado(Constante.ID, r1.Valor + r2.Valor);
            }
            else
            {
                if (!(r1.Tipo == Constante.ERROR || r2.Tipo == Constante.ERROR))
                {
                    string descripcion = "Tipos de operandos incorrectos para el operador '/'\nprimer tipo: "
                        + Constante.getTipo(r1.Tipo) + "\nsegundo tipo: " + Constante.getTipo(r2.Tipo);
                    uSintactico.uerrores.Add(new uError(Constante.SEMANTICO, descripcion, null, line, colm));
                }
                return new Resultado(Constante.ERROR, "");
            }
        }

    }
}
