using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace TuLibreria
{
    public class Sistema
    {
        // Método para leer el número de serie del Disco Duro / CD / DVD
        public static string ObtenerNumeroSerieDisco()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    {
                        return drive.Name; // En este apartado retornamos el nombre que tiene el disco
                    }
                }

                return "No se encontró el disco.";
            }
            catch (Exception ex)
            {
                return "Error al obtener información del disco: " + ex.Message;
            }
        }

        // Método para obtener la cantidad de unidades de disco
        public static int ObtenerCantidadUnidadesDisco()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                return drives.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener la cantidad de unidades de disco: " + ex.Message);
                return -1;
            }
        }

        // Método para obtener el inventario general del sistema
        public static Dictionary<string, string> ObtenerInventarioGeneral()
        {
            Dictionary<string, string> inventario = new Dictionary<string, string>();

            // Agregar información del sistema relevante
            inventario.Add("Sistema Operativo", Environment.OSVersion.VersionString);
            inventario.Add("Versión del .NET Framework", Environment.Version.ToString());
            inventario.Add("Directorio del sistema", Environment.SystemDirectory);
            inventario.Add("Directorio temporal", Path.GetTempPath());

            return inventario;
        }

        // Método para obtener la dirección MAC
        public static string ObtenerMACAddress()
        {
            try
            {
                string macAddress = string.Empty;
                foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                    {
                        macAddress = nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
                return macAddress;
            }
            catch (Exception ex)
            {
                return "Error al obtener la dirección MAC: " + ex.Message;
            }
        }

        // Método para acceder al Registro del Sistema
        public static string LeerClaveRegistro(string clave)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(clave);
                if (key != null)
                {
                    return key.GetValue("").ToString();
                }
                else
                {
                    return "Clave no encontrada.";
                }
            }
            catch (Exception ex)
            {
                return "Error al leer la clave del Registro del Sistema: " + ex.Message;
            }
        }

        // Método para crear una clave en el Registro del Sistema
        public static string CrearClaveRegistro(string clave, string valor)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(clave);
                key.SetValue("", valor);
                key.Close();
                return "Clave creada exitosamente.";
            }
            catch (Exception ex)
            {
                return "Error al crear la clave en el Registro del Sistema: " + ex.Message;
            }
        }

        // Método para borrar una clave del Registro del Sistema
        public static string BorrarClaveRegistro(string clave)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKey(clave);
                return "Clave borrada exitosamente.";
            }
            catch (Exception ex)
            {
                return "Error al borrar la clave del Registro del Sistema: " + ex.Message;
            }
        }

        // Método para modificar una clave del Registro del Sistema
        public static string ModificarClaveRegistro(string clave, string nuevoValor)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(clave, true);
                if (key != null)
                {
                    key.SetValue("", nuevoValor);
                    key.Close();
                    return "Clave modificada exitosamente.";
                }
                else
                {
                    return "Clave no encontrada.";
                }
            }
            catch (Exception ex)
            {
                return "Error al modificar la clave del Registro del Sistema: " + ex.Message;
            }
        }

        // Método para obtener los procesos activos
        public static List<string> ObtenerProcesosActivos()
        {
            List<string> procesos = new List<string>();

            try
            {
                foreach (Process proceso in Process.GetProcesses())
                {
                    procesos.Add(proceso.ProcessName);
                }
            }
            catch (Exception ex)
            {
                procesos.Add("Error al obtener los procesos activos: " + ex.Message);
            }

            return procesos;
        }

        // Método para "matar" un proceso
        public static string MatarProceso(string nombreProceso)
        {
            try
            {
                Process[] procesos = Process.GetProcessesByName(nombreProceso);
                if (procesos.Length > 0)
                {
                    foreach (Process proceso in procesos)
                    {
                        proceso.Kill();
                    }
                    return "Proceso(s) " + nombreProceso + " terminado(s) exitosamente.";
                }
                else
                {
                    return "No se encontró ningún proceso con el nombre " + nombreProceso + ".";
                }
            }
            catch (Exception ex)
            {
                return "Error al intentar terminar el proceso: " + ex.Message;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Número de serie del disco: " + Sistema.ObtenerNumeroSerieDisco());
            Console.WriteLine("Cantidad de unidades de disco: " + Sistema.ObtenerCantidadUnidadesDisco());

            Console.WriteLine("\nInventario general del sistema:");
            var inventario = Sistema.ObtenerInventarioGeneral();
            foreach (var item in inventario)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine("\nDirección MAC: " + Sistema.ObtenerMACAddress());

            // Aquí puedes llamar a otros métodos según sea necesario...

            Console.ReadLine(); // Para que la consola no se cierre inmediatamente
        }
    }
}