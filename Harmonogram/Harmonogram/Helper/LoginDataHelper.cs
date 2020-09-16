using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Helper
{
    class LoginDataHelper
    {
        public static List<string> users;
        public static List<string> servers;
        public static string cesta = "";
        public static string uzivatele = "";
        public static string servery = "";

        public static string lastloginpath = "";
        public static string lastserverpath = "";
        public static string lastlogin = "";
        public static string lastserver = "";


        public static void addLastLogin(string user)
        {
            File.WriteAllText(lastloginpath, user);
        }

        public static void addLastServer(string server)
        {
            File.WriteAllText(lastserverpath, server);
        }

        public static void addUser(string user)
        {
            if (!users.Contains(user))
            {
                using (StreamWriter sw = new StreamWriter(uzivatele, true))
                {
                    sw.WriteLine(user);

                    sw.Flush();
                }
            }
        }

        public static void addServer(string server)
        {
            if (!servers.Contains(server))
            {
                using (StreamWriter sw = new StreamWriter(servery, true))
                {
                    sw.WriteLine(server);

                    sw.Flush();
                }

            }

        }

        public static void init()
        {
            users = new List<string>();
            servers = new List<string>();

            try
            {
                cesta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
                if (!Directory.Exists(cesta))
                    Directory.CreateDirectory(cesta);
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", cesta);
            }

            lastloginpath = Path.Combine(cesta, "lastlogin.txt");

            if (File.Exists(lastloginpath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(lastloginpath))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            lastlogin = s;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Při načítání nastavení došlo k následující chybě: {0}", e.Message);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(lastloginpath))
                    {
                        sw.WriteLine("op");
                        sw.Flush();
                    }

                    using (StreamReader sr = new StreamReader(lastloginpath))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            lastlogin = s;
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Při vytvoření nastavení došlo k následující chybě: {0}", e.Message);
                }
            }

            lastserverpath = Path.Combine(cesta, "lastserver.txt");

            if (File.Exists(lastserverpath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(lastserverpath))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            lastserver = s;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Při načítání nastavení došlo k následující chybě: {0}", e.Message);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(lastserverpath))
                    {
                        sw.WriteLine("op");
                        sw.Flush();
                    }

                    using (StreamReader sr = new StreamReader(lastserverpath))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            lastserver = s;
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Při vytvoření nastavení došlo k následující chybě: {0}", e.Message);
                }
            }


            servery = Path.Combine(cesta, "servery.txt");

            if (File.Exists(servery))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(servery))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            servers.Add(s);
                        }
                    }
                    servers.Add(lastserver);
                    servers.Reverse();


                }
                catch (Exception e)
                {
                    Console.WriteLine("Při načítání nastavení došlo k následující chybě: {0}", e.Message);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(servery))
                    {
                        sw.WriteLine("server\\msoft");
                        sw.Flush();
                    }

                    using (StreamReader sr = new StreamReader(servery))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            servers.Add(s);
                        }
                    }
                    servers.Reverse();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Při vytvoření nastavení došlo k následující chybě: {0}", e.Message);
                }
            }

            uzivatele = Path.Combine(cesta, "uzivatele.txt");

            if (File.Exists(uzivatele))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(uzivatele))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            users.Add(s);
                        }
                    }
                    users.Add(lastlogin);
                    users.Reverse();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Při načítání nastavení došlo k následující chybě: {0}", e.Message);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(uzivatele))
                    {
                        sw.WriteLine("op");
                        sw.Flush();
                    }

                    using (StreamReader sr = new StreamReader(uzivatele))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            users.Add(s);
                        }
                    }
                    users.Reverse();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Při vytvoření nastavení došlo k následující chybě: {0}", e.Message);
                }
            }








        }
    }
}
