using ServidorDB.analizadores.usql;
using ServidorDB.arboles.usql.Expresiones;
using ServidorDB.arboles.xml;
using ServidorDB.otros;
using ServidorDB.tabla_simbolos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorDB.arboles.usql.SSL
{
    class Asignacion : uInstruccion
    {
        /*
         * Realiza la asignacion de una variable
         * @var     variable de asignacion
         * @id      atributo de un objeto
         * @ne      expresion de asignacion 
         */

        private string var;
        private string id;
        private NodoExp exp;
        private int line;
        private int colm;

        public Asignacion(string var, NodoExp exp, int line, int colm)
        {
            this.var = var;
            this.exp = exp;
            this.line = line;
            this.colm = colm;
        }
        
        public Asignacion(string var, string id, NodoExp exp, int line, int colm)
        {
            this.var = var;
            this.id = id;
            this.exp = exp;
            this.line = line;
            this.colm = colm;
        }

        public string Var { get => var; set => var = value; }
        public string Id { get => id; set => id = value; }
        public NodoExp Exp { get => exp; set => exp = value; }
        public int Line { get => line; set => line = value; }
        public int Colm { get => colm; set => colm = value; }

        public object ejecutar(Entorno ent)
        {
            if (id == null)
            {//entonces solo es asignacion de variable
                Resultado res = (Resultado) exp.ejecutar(ent);
                //Simbolo s = ent.getSimbolo_Entorno(var);
                Simbolo s = ent.getSimbolo_Entorno(var, Simbolo.VARIABLE);

                if (s != null)
                {
                    if(res.Tipo == s.Tipo)
                    {
                        s.Valor = res.Valor;
                    }
                    else if (res.Tipo == Constante.INTEGER && s.Tipo == Constante.BOOL)
                    {
                        int v = Convert.ToInt32(res.Valor);
                        if (v == 1 || v == 0)
                        {
                            s.Valor = res.Valor;
                        }
                        else
                        {
                            string descripcion = "El tipo de dato bool no permite el valor " + res.Valor
                                + " unicamente acepta 0 o 1";
                            uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                            return new Resultado(Constante.ERROR, "");
                        }
                    }
                    else
                    {
                        if (res.Tipo != Constante.ERROR)
                        {
                            string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a " + Constante.getTipo(s.Tipo);
                            uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                        }
                    }
                }
                else
                {
                    string descripcion = "Variable " + var + " no existe";
                    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                }
            }
            else
            {//entonces es para asignacion de un atributo
                Resultado res = (Resultado)exp.ejecutar(ent);
                //Simbolo s = ent.getSimbolo_Entorno(var);

                Simbolo s = ent.getSimbolo_Entorno(var, Simbolo.VARIABLE);

                if (s != null)
                {

                    /*Devo buscar sus valores por el "id" de esta clase con la lista de parametros que tiene
                     el objeto
                     
                     si si existe devo verificar su tipo y aisgnar el valor
                     si no reportar el error de que no exise el atriubto del objeto
                     
                     si el tipo no es igual al del valor que se le quiere asignar reportar error de tipos
                     
                     
                     tambien devo ver como realizarlo en el NODO LVARIBLE DONDE TAMBIEN SE 
                     NECESITAN DATOS JAJAJ*/
                    Objeto obj = (Objeto)s.Valor;

                    bool atrEncontrado = false;
                    bool tipoCorrecto = false;
                    int type = -1;
                    for (int i = 0; i < obj.Parametros.Count; i++)
                    {
                        Atributo tmp = obj.Parametros[i];
                        if (tmp.Nombre.Equals(id))
                        {
                            atrEncontrado = true;
                            if(res.Tipo == tmp.Tipo)
                            {
                                //valor correcto
                                tipoCorrecto = true;
                                obj.Valores[i] = res.Valor;
                            }
                            type = tmp.Tipo;
                            break;
                        }
                    }

                    if (!atrEncontrado)
                    {
                        //reportar el error que no existe el atributo del objeto
                        string descripcion = "Atributo " + id + " no encontrado en el objeto: " + var;
                        uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                    else if (!tipoCorrecto)
                    {
                        //error de tipos
                        //aqui siempre entra si se encontro el atributo--entonces type siempre va hacer diferente a -1
                        string descripcion = "Tipos incompatibles: " + Constante.getTipo(res.Tipo) + " no puede ser convertido a " + Constante.getTipo(type);
                        uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                    }
                }
                else
                {
                    string descripcion = "Variable " + var + " no existe";
                    uSintactico.uerrores.Add(new uError(ent.Tent, Constante.SEMANTICO, descripcion, null, line, colm));
                }
            }
            return null;
        }

        object uInstruccion.generar_booleano(Entorno ent)
        {
            throw new NotImplementedException();
        }
    }
}
