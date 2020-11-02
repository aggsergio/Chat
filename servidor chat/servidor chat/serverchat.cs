using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace Servidor
{
    class Servidor_Chat
    {
        private TcpListener server;
        private TcpClient client = new TcpClient();
        private IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, 8000);
        private List<Connection> list = new List<Connection>();

        Connection con;

        private struct Connection
        {
            public NetworkStream stream;
            public StreamWriter streamw;
            public StreamReader streamr;
            public string nombre;
        }
        public Servidor_Chat()
        {
            Inicio();
        }

        public void Inicio()
        {
            Console.WriteLine("Conectado");
            server = new TcpListener(ipendpoint);
            server.Start();
            while (true)
            {
                client = server.AcceptTcpClient();

                con = new Connection();
                con.stream = client.GetStream();
                con.streamr = new StreamReader(con.stream);
                con.streamw = new StreamWriter(con.stream);

                con.nombre = con.streamr.ReadLine();

                list.Add(con);
                Console.WriteLine(con.nombre + " ha entrado al chat.  Bienvenido de nuevo.");
                Thread t = new Thread(Escuchar_conexion);
                t.Start();
            }
        }
        void Escuchar_conexion()
        {
            Connection hcon = con;
            do
            {
                try
                {
                    string tmp = hcon.streamr.ReadLine();
                    Console.WriteLine(hcon.nombre + ": " + tmp);
                    foreach (Connection c in list)
                    {
                        try
                        {
                            c.streamw.WriteLine(hcon.nombre + ": " + tmp);
                            c.streamw.Flush();
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                    list.Remove(hcon);
                    Console.WriteLine(con.nombre + " se ha desconectado del chat. Bye, bye!~");
                    break;
                }
            } while (true);
        }

    }
}