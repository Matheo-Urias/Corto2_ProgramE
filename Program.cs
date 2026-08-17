using System;

class Program
{
    const int MAX = 100, MAX_EMP = 50;

    static string[] sedes = { "Campus Santa Ana (Central)", "Campus Ilobasco (CRI)" };
    static string[] direcciones = { "Bypass carretera a Metapan, Santa Ana", "Carretera a Ilobasco Km 56, Cabanas" };
    static string[] telefonos = { "(+503) 2484-0623", "(+503) 2378-1500" };

    static string[] carreras = {
        "Lic. Administracion de Empresas", "Lic. Contaduria Publica", "Lic. Mercadeo y Negocios Inter.",
        "Ing. Quimica", "Ing. Desarrollo de Software", "Ing. Civil", "Ing. Sistemas Informaticos",
        "Lic. Diseno Grafico", "Lic. Ciencias Juridicas", "Lic. Idioma Ingles", "Doc. Medicina", "Lic. Enfermeria", "Tec. Enfermeria",
        "Ing. Desarrollo de Software (IL)", "Ing. Sistemas Informaticos (IL)", "Lic. Educacion Ingles (IL)",
        "Lic. Idioma Ingles (IL)", "Lic. Mercadeo (IL)", "Lic. Enfermeria (IL)", "Tec. Enfermeria (IL)",
        "Lic. Administracion de Empresas (Semi)", "Ing. Procesamiento de Alimentos (Semi)"
    };
    static int[] relSede = { 0,0,0,0,0,0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,1 };

    static string[] empDui = new string[MAX_EMP], empNom = new string[MAX_EMP], empTip = new string[MAX_EMP], empCar = new string[MAX_EMP];
    static int[] empSede = new int[MAX_EMP];
    static int contEmp = 0;

    static string[] estCar = new string[MAX], estNom = new string[MAX];
    static int[] estEdad = new int[MAX], estCarr = new int[MAX], estSede = new int[MAX];
    static char[] estGen = new char[MAX];
    static double[] estAsis = new double[MAX];
    static double[,] estNot = new double[MAX, 4];
    static int contEst = 0;

    static void Main()
    {
        int op;
        do
        {
            Console.Clear();
            Console.WriteLine(" SISTEMA CENTRALIZADO UNICAES\n" + new string('=', 45));
            Console.WriteLine("1. Registrar Personal\n" +
                              "2. Registrar Estudiante\n" +
                              "3. Mostrar Listados Generales\n" +
                              "4. Buscar Persona\n" +
                              "5. Modificar Datos de Persona\n" +
                              "6. Eliminar Persona\n" +
                              "7. Reporte Gerencial\n" +
                              "8. Cargar Datos Oficiales UNICAES\n" +
                              "9. Salir");
            op = (int)LeerRango("\nOpcion: ", 1, 9, true);
            Console.Clear();

            switch (op)
            {
                case 1: RegistrarEmpleado(); break;
                case 2: RegistrarEstudiante(); break;
                case 3: MostrarListados(); break;
                case 4: Buscar(); break;
                case 5: ModificarPersona(); break;
                case 6: EliminarPersona(); break;
                case 7: GenerarReporte(); break;
                case 8: CargarDatosSemilla(); break;
            }
            if (op != 9) { Console.WriteLine("\nPresione ENTER para continuar..."); Console.ReadLine(); }
        } while (op != 9);
    }

    static void RegistrarEmpleado()
    {
        if (contEmp >= MAX_EMP) { Console.WriteLine("Limite de personal alcanzado."); return; }
        Console.WriteLine("--- REGISTRO DE PERSONAL ---");
        int s = SeleccionarSede();
        string dui = LeerId("DUI (9 digitos): ", 9, true, true, -1);
        string nom = LeerTexto("Nombre (solo letras): ", true);
        
        int t = (int)LeerRango("Tipo:\n1. Autoridad\n2. Docente\n3. Administrativo\n4. Servicios Generales\nOpcion: ", 1, 4, true);
        string[] tipos = { "Autoridad/Decano", "Docente", "Administrativo", "Servicios Generales" };
        string cargo = LeerTexto("Cargo especifico: ", false);

        empDui[contEmp] = dui; empNom[contEmp] = nom; empSede[contEmp] = s; empTip[contEmp] = tipos[t - 1]; empCar[contEmp] = cargo;
        contEmp++;
        Console.WriteLine("\nPersonal registrado correctamente!");
    }

    static void RegistrarEstudiante()
    {
        if (contEst >= MAX) { Console.WriteLine("Limite de estudiantes alcanzado."); return; }
        Console.WriteLine("--- REGISTRO DE ESTUDIANTE ---");
        int s = SeleccionarSede();
        
        int[] filt = new int[carreras.Length]; int totalFilt = 0;
        for (int i = 0; i < carreras.Length; i++)
            if (relSede[i] == s) { filt[totalFilt] = i; Console.WriteLine($"{totalFilt + 1}. {carreras[i]}"); totalFilt++; }

        int cSel = (int)LeerRango("Seleccione Carrera: ", 1, totalFilt, true) - 1;
        string carnet = LeerId("Carnet (9 caracteres)", 9, false, false, -1).ToUpper();
        string nom = LeerTexto("Nombre Alumno: ", true);
        int edad = (int)LeerRango("Edad (16-80): ", 16, 80, true);
        
        char g;
        do { Console.Write("Genero (M/F): "); g = char.ToUpper(Console.ReadLine()!.Trim()[0]); } while (g != 'M' && g != 'F');
        double asis = LeerRango("Asistencia (0-100%): ", 0, 100, false);

        estCar[contEst] = carnet; estNom[contEst] = nom; estEdad[contEst] = edad; estGen[contEst] = g;
        estCarr[contEst] = filt[cSel]; estSede[contEst] = s; estAsis[contEst] = asis;

        for (int i = 0; i < 3; i++) estNot[contEst, i] = LeerNota($"Nota Periodo {i + 1}: ");
        contEst++;
        Console.WriteLine("\nEstudiante registrado correctamente!");
    }

    static void MostrarListados()
    {
        Console.WriteLine("=== PERSONAL REGISTRADO ===");
        if (contEmp == 0) Console.WriteLine("No hay personal registrado.");
        else for (int i = 0; i < contEmp; i++) Console.WriteLine($"{empDui[i]} | {empNom[i],-35} | {empTip[i],-18} | {empCar[i],-35} | {sedes[empSede[i]]}");

        Console.WriteLine("\n=== ESTUDIANTES REGISTRADOS ===");
        if (contEst == 0) Console.WriteLine("No hay estudiantes registrados.");
        else for (int i = 0; i < contEst; i++)
        {
            double p = Prom(i);
            string est = (p >= 6.0 && estAsis[i] >= 75) ? "APROBADO" : "REPROBADO";
            Console.WriteLine($"{estCar[i]} | {estNom[i],-25} | {sedes[estSede[i]],-22} | Prom: {p:F2} | {est}");
        }
    }

    static void Buscar()
    {
        Console.Write("Ingrese Carnet, DUI o Nombre: "); string t = Console.ReadLine()!.Trim().ToLower();
        bool enc = false;

        for (int i = 0; i < contEmp; i++)
            if (empDui[i].ToLower() == t || empNom[i].ToLower().Contains(t))
            { Console.WriteLine($"[PERSONAL] {empDui[i]} - {empNom[i]} ({empCar[i]} - {sedes[empSede[i]]})"); enc = true; }

        for (int i = 0; i < contEst; i++)
            if (estCar[i].ToLower() == t || estNom[i].ToLower().Contains(t))
            { Console.WriteLine($"[ALUMNO] {estCar[i]} - {estNom[i]} (Prom: {Prom(i):F2} - {sedes[estSede[i]]})"); enc = true; }

        if (!enc) Console.WriteLine("No se encontraron registros.");
    }

    static void ModificarPersona()
    {
        Console.WriteLine("--- MODIFICAR DATOS DE PERSONA ---");
        Console.Write("Ingrese DUI o Carnet de la persona a modificar: ");
        string id = Console.ReadLine()!.Trim().ToLower();

        for (int i = 0; i < contEmp; i++)
        {
            if (empDui[i].ToLower() == id)
            {
                Console.WriteLine($"\n[PERSONAL ENCONTRADO] Nombre actual: {empNom[i]}");
                empSede[i] = SeleccionarSede();
                empDui[i] = LeerId("Nuevo DUI (9 digitos): ", 9, true, true, i);
                empNom[i] = LeerTexto("Nuevo Nombre Completo: ", true);
                
                int t = (int)LeerRango("Nuevo Tipo:\n1. Autoridad\n2. Docente\n3. Administrativo\n4. Servicios Generales\nOpcion: ", 1, 4, true);
                string[] tipos = { "Autoridad/Decano", "Docente", "Administrativo", "Servicios Generales" };
                empTip[i] = tipos[t - 1];
                empCar[i] = LeerTexto("Nuevo Cargo especifico: ", false);

                Console.WriteLine("\nDatos del empleado modificados con exito!");
                return;
            }
        }

        for (int i = 0; i < contEst; i++)
        {
            if (estCar[i].ToLower() == id)
            {
                Console.WriteLine($"\n[ESTUDIANTE ENCONTRADO] Nombre actual: {estNom[i]}");
                int s = SeleccionarSede();
                
                int[] filt = new int[carreras.Length]; int totalFilt = 0;
                for (int c = 0; c < carreras.Length; c++)
                    if (relSede[c] == s) { filt[totalFilt] = c; Console.WriteLine($"{totalFilt + 1}. {carreras[c]}"); totalFilt++; }

                int cSel = (int)LeerRango("Seleccione Nueva Carrera: ", 1, totalFilt, true) - 1;
                
                estSede[i] = s;
                estCarr[i] = filt[cSel];
                estCar[i] = LeerId("Nuevo Carnet (9 caracteres): ", 9, false, false, i);
                estNom[i] = LeerTexto("Nuevo Nombre del Alumno: ", true);
                estEdad[i] = (int)LeerRango("Nueva Edad (16-80): ", 16, 80, true);
                
                char g;
                do { Console.Write("Nuevo Genero (M/F): "); g = char.ToUpper(Console.ReadLine()!.Trim()[0]); } while (g != 'M' && g != 'F');
                estGen[i] = g;

                estAsis[i] = LeerRango("Nueva Asistencia (0-100%): ", 0, 100, false);

                Console.WriteLine("\n--- Modificar Notas ---");
                for (int n = 0; n < 3; n++) estNot[i, n] = LeerNota($"Nueva Nota Periodo {n + 1}: ");

                Console.WriteLine("\nDatos del estudiante modificados con exito!");
                return;
            }
        }

        Console.WriteLine("No se encontro ninguna persona registrada.");
    }

    static void EliminarPersona()
    {
        Console.WriteLine("--- ELIMINAR REGISTRO DE PERSONA ---");
        Console.Write("Ingrese DUI o Carnet de la persona a eliminar: ");
        string id = Console.ReadLine()!.Trim().ToLower();

        for (int i = 0; i < contEmp; i++)
        {
            if (empDui[i].ToLower() == id)
            {
                Console.Write($"Esta seguro de eliminar al empleado {empNom[i]}? (S/N): ");
                if (Console.ReadLine()!.Trim().ToUpper() == "S")
                {
                    for (int j = i; j < contEmp - 1; j++)
                    {
                        empDui[j] = empDui[j + 1];
                        empNom[j] = empNom[j + 1];
                        empTip[j] = empTip[j + 1];
                        empCar[j] = empCar[j + 1];
                        empSede[j] = empSede[j + 1];
                    }
                    contEmp--;
                    Console.WriteLine("\nEmpleado eliminado exitosamente!");
                }
                else Console.WriteLine("Operacion cancelada.");
                return;
            }
        }

        for (int i = 0; i < contEst; i++)
        {
            if (estCar[i].ToLower() == id)
            {
                Console.Write($"Esta seguro de eliminar al estudiante {estNom[i]}? (S/N): ");
                if (Console.ReadLine()!.Trim().ToUpper() == "S")
                {
                    for (int j = i; j < contEst - 1; j++)
                    {
                        estCar[j] = estCar[j + 1];
                        estNom[j] = estNom[j + 1];
                        estEdad[j] = estEdad[j + 1];
                        estGen[j] = estGen[j + 1];
                        estCarr[j] = estCarr[j + 1];
                        estSede[j] = estSede[j + 1];
                        estAsis[j] = estAsis[j + 1];
                        for (int k = 0; k < 4; k++) estNot[j, k] = estNot[j + 1, k];
                    }
                    contEst--;
                    Console.WriteLine("\nEstudiante eliminado exitosamente!");
                }
                else Console.WriteLine("Operacion cancelada.");
                return;
            }
        }

        Console.WriteLine("No se encontro ninguna persona registrada.");
    }

    static void GenerarReporte()
    {
        for (int s = 0; s < sedes.Length; s++)
        {
            Console.WriteLine($"\n>>> {sedes[s].ToUpper()} <<<\nDireccion: {direcciones[s]} | Tel: {telefonos[s]}");
            int total = 0, ap = 0, rep = 0; double suma = 0;
            for (int e = 0; e < contEst; e++)
            {
                if (estSede[e] == s)
                {
                    total++; double p = Prom(e); suma += p;
                    if (p >= 6.0 && estAsis[e] >= 75) ap++; else rep++;
                }
            }
            Console.WriteLine($"Total Alumnos: {total}");
            if (total > 0) Console.WriteLine($"Promedio Sede: {(suma / total):F2}\nAprobados: {ap} | Reprobados: {rep}");
        }
    }

    static void CargarDatosSemilla()
    {
        string[] d = { "012345678", "023456789", "034567890", "045678901", "056789012", "067890123", "078901234", "089012345", "090123456", "091234567", "092345678" };
        string[] n = { "Miguel Angel Moran Aquino", "Moises Antonio Martinez Zaldivar", "Roberto Antonio Lopez Castro", "Ricardo Ernesto Morales Guerrero", "Mauricio Ernesto Velasquez Soriano", "Jaime Osmin Trigueros Chavez", "Walter Alexander Aguilar Moran", "Juan Alfonso Trigueros Chavez", "Ursula Guadalupe Rosales", "Carlos Alberto Rivas", "Jose Mario Ramos" };
        string[] t = { "Autoridad/Decano", "Autoridad/Decano", "Autoridad/Decano", "Autoridad/Decano", "Autoridad/Decano", "Autoridad/Decano", "Autoridad/Decano", "Autoridad/Decano", "Docente", "Servicios Generales", "Servicios Generales" };
        string[] c = { "Rector Magnifico", "Vicerrector General", "Vicerrector Academico", "Decano Ciencias Empresariales", "Decano Ingenieria y Arquitectura", "Decano Ciencias y Humanidades", "Decano Ciencias de la Salud", "Decano Facultad Multidisciplinaria CRI", "Catedratica Tiempo Completo", "Agente de Seguridad Principal", "Auxiliar de Ordenanza y Mantenimiento" };
        int[] s = { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1 };

        for (int i = 0; i < d.Length; i++)
        { 
            empDui[i] = d[i]; 
            empNom[i] = n[i]; 
            empTip[i] = t[i]; 
            empCar[i] = c[i]; 
            empSede[i] = s[i]; 
        }
        contEmp = d.Length;

        estCar[0] = "2026MU502"; estNom[0] = "Madison Martinez"; estEdad[0] = 22; estGen[0] = 'F'; estCarr[0] = 4; estSede[0] = 0; estAsis[0] = 95; estNot[0, 0] = 8.5; estNot[0, 1] = 9.0; estNot[0, 2] = 9.2;
        estCar[1] = "2026GM101"; estNom[1] = "Susana Gonzalez"; estEdad[1] = 23; estGen[1] = 'F'; estCarr[1] = 13; estSede[1] = 1; estAsis[1] = 88; estNot[1, 0] = 7.5; estNot[1, 1] = 8.0; estNot[1, 2] = 8.1;
        contEst = 2;

        Console.WriteLine("CARGA MASIVA COMPLETADA EXITOSAMENTE\n");
        MostrarListados();
    }

    static int SeleccionarSede() {
        for (int i = 0; i < sedes.Length; i++) Console.WriteLine($"{i + 1}. {sedes[i]}");
        return (int)LeerRango("Opcion Sede: ", 1, sedes.Length, true) - 1;
    }

    static string LeerId(string msj, int len, bool soloNum, bool esDui, int posActual) {
        while (true) {
            Console.Write(msj); string txt = Console.ReadLine()!.Trim();
            if (txt.Length != len) { Console.WriteLine($"Debe tener exactamente {len} caracteres."); continue; }
            
            bool numOk = true;
            if (soloNum) foreach (char c in txt) if (!char.IsDigit(c)) numOk = false;
            if (!numOk) { Console.WriteLine("Solo debe contener digitos numericos."); continue; }

            bool existe = false;
            if (esDui) { 
                for (int i = 0; i < contEmp; i++) 
                    if (empDui[i] == txt && i != posActual) existe = true; 
            }
            else { 
                for (int i = 0; i < contEst; i++) 
                    if (estCar[i].Equals(txt, StringComparison.OrdinalIgnoreCase) && i != posActual) existe = true; 
            }

            if (existe) { Console.WriteLine("El identificador ya existe en el sistema."); continue; }
            return txt;
        }
    }

    static string LeerTexto(string msj, bool soloLetras) {
        while (true) {
            Console.Write(msj); string txt = Console.ReadLine()!.Trim();
            if (string.IsNullOrEmpty(txt)) { Console.WriteLine("No puede quedar vacio."); continue; }
            
            bool ok = true;
            if (soloLetras) foreach (char c in txt) if (!char.IsLetter(c) && !char.IsWhiteSpace(c)) ok = false;
            if (!ok) { Console.WriteLine("No se permiten numeros ni simbolos."); continue; }
            return txt;
        }
    }

    static double LeerNota(string msj) {
        while (true) {
            double n = LeerRango(msj, 0, 10, false);
            if (Math.Round(n, 3) == n) return n;
            Console.WriteLine("Maximo 3 decimales.");
        }
    }

    static double Prom(int idx) {
        double sum = 0; int evs = estNot[idx, 3] > 0 ? 4 : 3;
        for (int i = 0; i < evs; i++) sum += estNot[idx, i];
        return sum / evs;
    }

    static double LeerRango(string msj, double min, double max, bool entero) {
        while (true) {
            Console.Write(msj);
            if (double.TryParse(Console.ReadLine(), out double v) && v >= min && v <= max)
                if (!entero || v % 1 == 0) return v;
            Console.WriteLine($"Ingrese un valor {(entero ? "entero" : "valido")} entre {min} y {max}.");
        }
    }
}