using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;

using IniParser;
using IniParser.Model;
using System.IO;
using Oracle.ManagedDataAccess.Types;

namespace solca_webcam
{
    class WebcamApp
    {
        /**
         * Nombre de los parametros de conexion
         */
        static readonly String DB = "DB", USER = "user", PASS = "pass", TNSNAME = "tnsname";

        /**
         * Ultima historia clinica
         */ 
        private Int32 numeroPaciente;

        /**
         * AppData Path
         */
        private string appDataPath;

        /**
         * Archivo de configuracion
         */
        private string configFilePath;

        /**
         * Mensaje de error de la conexion
         */
        private string errorMessage;

        /**
         * Parametros de conexion
         */
        private string user, pass, db;

        /**
         * Is The Connection available
         */
        private bool connectionAvailable;

        public string ErrorMessage { get => errorMessage; }
        public string User { get => user; set => user = value; }
        public string Pass { get => pass; set => pass = value; }
        public string Db { get => db; set => db = value; }
        public bool ConnectionAvailable { get => connectionAvailable; }
        public int NumeroPaciente { get => numeroPaciente; set => numeroPaciente = value; }

        /**
         * Webcam
         */
        public WebcamApp(string appDataPath, string configFileName)
        {
            this.connectionAvailable = false;
            InicilizaOpciones(appDataPath, configFileName);
        }

        /**
         * Consulta el nombre y imagen del paciente
         */
        public (string nombre, byte[] imageData) ConsultarFoto(int hc)
        {
            String nombre = null;
            byte[] imageData = null;

            this.errorMessage = null;

            if (! this.connectionAvailable)
            {
                this.errorMessage = "Conexion a base de datos no disponible";
                return (nombre, imageData);
            }

            // connection available
            using (OracleConnection con = new OracleConnection(this.getConnectionString()))
            {
                try
                {
                    con.Open();

                    // 
                    String sql =
                       @"SELECT X.PRIMER_APELLIDO || ' ' || X.SEGUNDO_APELLIDO || ' ' || INITCAP(X.PRIMER_NOMBRE) || ' ' || INITCAP(X.SEGUNDO_NOMBRE) NOMBRE_PACIENTE, 
                                CASE WHEN IMAGEN IS NULL THEN 0 ELSE LENGTH(IMAGEN) END IMAGEN_LEN, 
                                IMAGEN
                         FROM MGA_PACIENTES_SOLCA X 
                         LEFT JOIN MGA_IMAGENES_X_PACIENTE I 
                                ON(I.NUMERO_PACIENTE = X.NUMERO_PACIENTE AND I.CODIGO_TIPO_PACIENTE = X.CODIGO_TIPO_PACIENTE AND I.CODIGO_TIPO_DOCUMENTO = 3 AND I.NUMERO_SECUENCIA = 1) 
                         WHERE X.NUMERO_PACIENTE = :hc
                           AND X.CODIGO_TIPO_PACIENTE = 1 
                           AND ROWNUM = 1";

                    // Set command to create Anonymous PL/SQL Block
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;

                    // Bind the parameter as OracleDbType.Blob to command for inserting image
                    OracleParameter param = cmd.Parameters.Add(":hc", OracleDbType.Decimal);
                    param.Direction = ParameterDirection.Input;
                    param.Value = hc;

                    OracleDataReader dr = cmd.ExecuteReader();
                    dr.Read();

                    if (!dr.HasRows)
                    {
                        // Result is empty
                        this.errorMessage = "HC " + hc + " no encontrada!";
                        return (nombre, imageData);
                    }

                    // Anotar ultima hc clinica consultada
                    this.numeroPaciente = hc;

                    // Leer nombre y foto del paciente
                    nombre = dr.GetString(0);

                    int imageLen = dr.GetInt32(1);
                    if (imageLen > 0)
                    {
                        OracleBlob b = dr.GetOracleBlob(2);

                        imageData = new byte[b.Length];                        
                        int i = b.Read(imageData, 0, System.Convert.ToInt32(b.Length));

                        //MemoryStream memStream = new MemoryStream(byteArr);

                        //imagePath = Path.Combine(appDataPath, hc + ".jpg");
                        //using (FileStream fs = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
                        //{
                        //    fs.Write(byteArr, 0, (int)byteArr.Length);
                        //    fs.Close();
                        //}
                            
                    }

                    con.Dispose(); // Free resource                    
                }
                catch (Exception ex)
                {
                    this.errorMessage = ex.Message;
                    Console.WriteLine(ex.ToString());
                    
                }
            }
            return (nombre, imageData);
        }

        /**
         * Sube la foto del paciente
         */
        public bool SubirImageBaseDatos(String imagePath)
        {
            Int32 hc = this.NumeroPaciente;
            if (hc == 0)
            {
                this.errorMessage = "Se requiere que ingrese la HC y consulte nombre del paciente.";
                return false;
            }

            using (OracleConnection con = new OracleConnection(this.getConnectionString()))
            {
                try
                {
                    con.Open();

                    //Read file into byte[]
                    FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                    byte[] ImageData = new byte[fs.Length];
                    fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();//Close the File Stream

                    // Create Anonymous PL/SQL block string
                    String sql = "DECLARE " +
                                 "\n LN_EXISTE NUMBER;" +
                                 "\n BEGIN " +
                                 "\n   SELECT COUNT(1) INTO LN_EXISTE FROM MGA_IMAGENES_X_PACIENTE WHERE NUMERO_PACIENTE=" + hc + " AND CODIGO_TIPO_DOCUMENTO=3 AND NUMERO_SECUENCIA=1;" +
                                 "\n   IF LN_EXISTE = 0 THEN " +
                                 "\n     INSERT INTO MGA_IMAGENES_X_PACIENTE ( " +
                                 "\n     NUMERO_PACIENTE, CODIGO_SERVICIO, NUMERO_SECUENCIA, USUARIO_INGRESO, FORMATO_IMAGEN,IMAGEN,ORDEN,FECHA_INGRESO,CODIGO_TIPO_DOCUMENTO, CODIGO_EMPRESA,CODIGO_TIPO_PACIENTE) VALUES( " +
                                 "\n     " + hc + ", 50, 1, USER, 'JPG', :1, 1, SYSDATE, 3, 1, 1); " +
                                 "\n   ELSE " +
                                 "\n     UPDATE MGA_IMAGENES_X_PACIENTE SET IMAGEN = :1, FECHA_MODIFICACION=SYSDATE, USUARIO_MODIFICACION=USER " +
                                 "\n     WHERE NUMERO_PACIENTE=" + hc + " AND CODIGO_TIPO_DOCUMENTO=3 AND NUMERO_SECUENCIA=1;" +
                                 "\n   END IF; " +
                                 "\n END; ";

                    // Set command to create Anonymous PL/SQL Block
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;

                    // Bind the parameter as OracleDbType.Blob to command for inserting image
                    OracleParameter param = cmd.Parameters.Add("blobtodb", OracleDbType.Blob);
                    param.Direction = ParameterDirection.Input;
                    param.Value = ImageData;

                    // Bind the parameter as OracleDbType.Blob to command for retrieving the image
                    // OracleParameter param2 = cmd.Parameters.Add("blobfromdb", OracleDbType.Blob);
                    // param2.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery(); // Execute the Anonymous PL/SQL Block

                    con.Dispose();
                    return true; 
                    
                } catch (Exception ex1)
                {
                    this.errorMessage = ex1.Message;
                    return false;
                }

                
            }



            /*
            byte[] byteData = new byte[0]; // fetch the value of Oracle parameter
            byteData = (byte[])((OracleBlob)(cmd.Parameters[1].Value)).Value;            
            int ArraySize = new int();  // get the length of the byte array
            ArraySize = byteData.GetUpperBound(0);            
            FileStream fs1 = new FileStream(@DestLoc, FileMode.OpenOrCreate, FileAccess.Write);
            fs1.Write(byteData, 0, ArraySize); // Write the Blob data fetched from database to the filesystem
            fs1.Close();
            */
        }

        /**
         * Devuelve la cadena de conexion
         * 
         * @return null if not set
         */
        public string getConnectionString()
        {
            if (String.IsNullOrEmpty(this.db))
            {
                return null; // there is not conexion string
            }
            return buildConnectionString(this.user, this.pass, this.db);
        }

        /**
         * Arma la cadena de conexion
         */
        private string buildConnectionString(String user, String pass, String db)
        {
            return "Data Source=" + db + ";User Id=" + user + ";Password=" + pass + ";";
        }


        /**
         * Valida la conexion
         */
        public bool ValidarConexion()
        {
            return ValidarConexion(this.user, this.pass, this.db);
        }

        /**
         * Valida la conexion
         */
        public bool ValidarConexion(String user, String pass, String db)
        {
            this.errorMessage = "";
            this.connectionAvailable = false;

            string connectionString = buildConnectionString(user, pass, db);

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    try
                    {
                        conn.Open(); //
                    }
                    catch (Exception ex1)
                    {
                        this.errorMessage = ex1.Message;
                        return false;
                    }
                    
                    // Test a simple query
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT dummy FROM DUAL";
                        cmd.CommandType = CommandType.Text;

                        OracleDataReader dr = cmd.ExecuteReader();
                        dr.Read();

                        this.connectionAvailable = true;
                        return true; // conexion exitosa
                    }
                    catch (Exception ex2)
                    {
                        this.errorMessage = ex2.Message;
                        return false;
                    }
                    finally
                    {
                        conn.Dispose();
                    }
                }
            }
            catch (ArgumentException ex0)
            {
                this.errorMessage = ex0.Message;
                return false;
            }
        }
        /**
         * Guardar ini
         */
        public void GuardarOpciones(String user, String pass, String db)
        {
            this.user = user;
            this.pass = pass;
            this.db = db;

            try
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(this.configFilePath, Encoding.UTF8);

                data[DB][USER] = this.user;
                data[DB][PASS] = this.pass;
                data[DB][TNSNAME] = this.db;

                parser.WriteFile(this.configFilePath, data, Encoding.UTF8);

            } catch(Exception e1)
            {
                this.errorMessage = e1.Message;
            }
        }

        /**
         * Inicializa opciones
         */
        public (string user, string pass, string db) InicilizaOpciones()
        {
            if (!File.Exists(this.configFilePath))
            {
                // Touch it
                using (StreamWriter sw = File.CreateText(configFilePath))
                {
                    sw.Close();
                }

            }
            try
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(this.configFilePath, Encoding.UTF8);

                this.user = data[DB][USER];
                this.pass = data[DB][PASS];
                this.db = data[DB][TNSNAME];

            }
            catch (Exception ex1)
            {
                this.user = null;
                this.pass = null;
                this.db = null;

                this.errorMessage = ex1.Message;
            }
            
            return (user: this.user, pass: this.pass, db: this.db);
        }

        /**
         * Lee archivo ini 
         */
        public (string user, string pass, string db) InicilizaOpciones(string appDataPath, string configFileName)
        {
            this.appDataPath = appDataPath;
            this.configFilePath = Path.Combine(appDataPath, configFileName);

            return this.InicilizaOpciones();
        }

    }
}
