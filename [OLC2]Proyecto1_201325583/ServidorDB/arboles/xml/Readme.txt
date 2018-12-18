==========================================
		SISTEMA DE ARCHIVOS
==========================================

Arbol que simula el sistema de archivos
y se maneja el dbms


			Lo voy a realizar hago una llamada

								Constante.global.deleteSimbolDB();
                                //como ya se eliminaron proc en el entorno global entonces debo agregarlo otra vez
                                //como estos procedimientos son unicos por motivos de que la base de datos
                                //no permite valores repetidos entonces solo agregar
                                Constante.global.agregar(new tabla_simbolos.Simbolo(tabla_simbolos.Simbolo.PROCEDIMIENTO, Constante.VOID, procs[i].Nombre ,procs[i]));