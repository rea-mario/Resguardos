Resguardos para Equipos de Computo para una empresa.
1. Ejecución de Script Script_DB.sql
2. Se crean dos usuario Administrador y Operativo.
Administrador:
  Usuario:admin
  Clave:Admin123
Operador:
  Usuario:operador
  Clave:Operador123
3.  Descargar Reporitorio
4.  Configurar la conexion de SQL desde el appsettings.json en ConexionSQL "Server=localhost;Database=ResguardosDb;User Id=TuUsuario;Password=TuPassword;TrustServerCertificate=True"
5.  Iniciar programa.

Para realizar las peticiones, recomiendo Postman
se agrega la coleccion con el nombre: Resguardos.postman_collection.json
ejecutar api/auth/login tiene configurado un Pre-script que ayuda a actualizar el token a todas las peticiones que la ocupan.
ahora ya puedes validar las peticiones.
