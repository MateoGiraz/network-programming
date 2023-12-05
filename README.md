
# Alcance de la Aplicación

La aplicación FreeMarket, compuesta por los módulos FreeMarketServer y FreeMarketClient, ambos comunicados a través del módulo Common, permite a los usuarios interactuar con un mercado virtual a través de una interfaz de consola. Su propósito es proporcionar una plataforma donde los usuarios puedan gestionar y acceder a productos en el sistema. La aplicación permite el registro e inicio de sesión de usuarios, y para estos, la compra, creación, borrado, edición, calificación y consulta de productos. Se permite registrar y consultar imágenes de los diferentes productos. Se permite el acceso de diferentes clientes simultáneos a la aplicación, garantizando el correcto acceso a los recursos compartidos, manteniendo los datos con integridad en todo momento.

# Descripción General de la Arquitectura:

## FreeMarketClient

FreeMarketClient es el proyecto donde se implementa la aplicación de consola donde el usuario puede interactuar con el servidor. Consta de un menú a través del cuál el usuario accede a los servicios ofrecidos. La conección con el servidor se implementa a través de sockets.

Para que las acciones realizadas por el usuario puedan llevarse a cabo, es necesario implementar un mecanismo para que estos pedidos lleguen al servidor. Para lograr esto, creamos un directorio request, en donde implementamos una clase abstracta RequestTemplate y a partir de ella implementamos las peticiones concretas para cada funcionalidad, en pos de declarar un estándar para las peticiones a la vez que intentamos reutilizar código lo máximo posible. Además, en RequestTemplate se hace uso de KOI, el parser que implementamos para esta instancia del obligatorio (cuyo funcionamiento explicaremos más adelante), ya que este será el medio por el cual el cliente enviará y recibirá información por parte del servidor. Por último, también implementamos métodos para manejar las respuestas recibidas por parte del servidor para que la aplicación actúe en base a ella, basándose tanto en el mensaje devuelto como en el código de estado que se recibe dentro de la misma.

De esta forma, RequestTemplate tiene la responsabilidad de enviar el comando, y luego delegar el resto de la petición a sus implementaciones concretas. Asimismo contiene la funcionalidad para recibir e imprimir la respuesta del servidor. 

Sus implementaciones concretas se dividen entre peticiones relativas al usuario y a productos. Las primeras (sign in, log in), son muy similares, difiriendo solamente en el comando enviado al servidor y en que log in configura al usuario logueado en el cliente. Entonces, esta última emplea la lógica de sign in, agregando la funcionalidad extra. En el caso de producto, se desarrollan por un lado las peticiones de GetProduct, y GetProducts (pues no envían un DTO de producto al servidor), y por otro lado el resto (agrupadas como hijas de ProductRequestTemplate, con responsabilidad de crear el DTO de producto con su nombre y usuario asociado, convertirlo a string usando KOI, enviarlo al servidor y esperar la respuesta). Cualquier funcionalidad extra (más información para el producto, envío de imagenes, etc.) es implementada por las implementaciones concretas del template.

![image](https://github.com/MateoGiraz/network-programming/assets/100039777/b0c16b44-9593-49b4-9ae5-d7ea2eb6c225)


## 


## FreeMarketServer

En cuanto a la arquitectura y diseño de este módulo, se utiliza una estructura basada en controladores y repositorios, donde los controladores gestionan la lógica del negocio y los repositorios manejan el acceso y almacenamiento de datos. Es esta última capa la encargada de garantizar la seguridad y confiabilidad del negocio. Se hizo uso de múltiples mecanismos de exclusión mutua (como candados y semáforos) para garantizar la seguridad y coherencia en operaciones concurrentes. \
 \
	Luego de terminar de codificar los controladores y repositorios se hizo necesario crear el mecanismo por el cual el servidor escucha conexiones de los clientes para poder conectarse con ellos. Anteriormente mencionamos que el cliente envía pedidos mediante peticiones concretas según la funcionalidad deseada, pero no mencionamos cómo se encarga el servidor de manejar y llevar a cabo estos pedidos. Para esto decidimos que el servidor maneje las peticiones mediante Handlers específicos para cada pedido concreto. Al igual que con las Request generamos un Template para que cada Handler pueda reutilizar código y mantenga un estándar. Dentro de cada Handler también se utiliza KOI para parsear los DTO que llegan por parte del cliente así como para empaquetar la información a incluir en la respuesta. Por último, en la aplicación de consola de FreeMarketServer se puede observar el registro de pedidos y conexiones, así como la información de la dirección IP del servidor y del puerto de conexión.

El servidor escucha peticiones del cliente, recibe el comando y lo envía a OptionHandler, quien deriva la responsabilidad en función del comando recibido. Las peticiones que envían datos al cliente (GetProduct, GetProducts) tienen su propia lógica. Las que tienen responsabilidad sobre los usuarios, implementan UserHandlerTemplate, que recibe el DTO de usuario e intenta delegar la responsabilidad (de creación o de sesión), a estas implementaciones concretas. En caso de que existiera un error, UserHandlerTemplate configura el DTO de respuesta con un código de error (400) y envía al cliente el mensaje de la excepción. Si no hubiera problema, las implementaciones concretas interactúan con el paquete de lógica del servidor y configuran el DTO de respuesta con un código de éxito (200) y un mensaje que se mostrará en el cliente. El funcionamiento alrededor de la recepción de peticiones de los productos que reciben información del lado del cliente (crear, borrar, editar, comprar, calificar) es análogo.

Este módulo se responsabiliza de manejar los mecanismos de concurrencia, particularmente en la capa de acceso a datos, donde colocamos locks en las operaciones que escriben o leen datos.

![image](https://github.com/MateoGiraz/network-programming/assets/100039777/c0540002-cb95-400e-bf14-da488baa8fe8)

## 


## Common

Finalmente, el proyecto Common es una biblioteca de clases donde se encuentra todo lo necesario para la comunicación entre el servidor y el cliente. Aquí definimos y configuramos los valores estándar del protocolo (esto se encuentra en la clase ProtocolStandard), como el largo de los mensajes. También se implementó los Data Transfer Object (DTO) necesarios para poder transferir todos los diferentes tipos de datos dentro de nuestra aplicación. Se define un método Startup, que muestra un mensaje de bienvenida tanto en el Servidor como en el Cliente y se define el código para manejar las configuraciones de las soluciones. También generamos las clases Helpers, las cuales resultaron vitales tanto para el envío y recepción de mensajes (NetworkHelper) como para la lectura y escritura de los archivos (FileStreamHelper y FileTransferHelper).


### NetworkHelper

NetworkHelper implementa la funcionalidad básica para enviar y recibir mensajes, hace uso de dos métodos principales. 

SendMessage recibe un mensaje en bytes y un socket, Este método permite enviar un mensaje a través de un socket. El método envía el mensaje, y si no pudiera hacerlo completamente, sigue enviando la información faltante en trozos  hasta que se haya enviado todo el mensaje.

RecieveData permite recibir datos a través de un socket. Toma dos argumentos: length, que especifica la longitud de los datos que se esperan recibir, y socket, que es el socket a través del cual se recibirán los datos. El método crea un array de bytes del tamaño especificado y luego recibe los datos. Si no pudiera recibir todo el mensaje, lo hace en partes hasta que se haya recibido la cantidad esperada


### FileStreamHelper

FileStreamHelper proporciona métodos para leer y escribir datos en un archivo. Contiene dos métodos principales:

Read permite leer una parte específica de un archivo. Toma tres argumentos: path, que es la ruta del archivo, offset, que indica la posición desde la cual comenzar a leer, y length, que especifica la longitud de los datos que se deben leer. El método abre el archivo, se posiciona en el offset indicado y luego lee los datos en partes hasta que se haya leído la cantidad esperada.

Write permite escribir datos en un archivo. Toma dos argumentos: filePath, que es la ruta del archivo donde se escribirán los datos, y data, que es un array de bytes que contiene los datos a escribir. El método verifica si el archivo ya existe y decide si debe añadir los datos al final o crear un nuevo archivo. Luego, escribe los datos en el archivo.


### FileTransferHelper

FileTransferHelper proporciona métodos para enviar y recibir archivos a través de sockets de red. Se ocupa de encapsular toda la lógica del manejo de archivos para que luego enviar y recibirlos sea trivial y pueda hacerse en solo un par de líneas de código. Contiene los siguientes métodos principales.

SendFile, que envía un archivo a través de un socket. Primero, envía información sobre el archivo (nombre y tamaño) utilizando el método SendFileInfo. Luego, envía los datos del archivo en partes utilizando el método SendFileData.

SendFileInfo obtiene el nombre del archivo y lo convierte a bytes, luego envía la envía sobre el socket usando NetworkHelper y finalmente envía el tamaño del archivo.

SendFileData envía los datos del archivo en partes utilizando un objeto de tipo FileStreamHelper para leer el archivo y NetworkHelper para enviar los datos.

ReceiveFile recibe un archivo a través de un socket. Utiliza el método ReceiveFileInfo para obtener información sobre el archivo (nombre y tamaño), y luego recibe los datos del archivo en partes utilizando el método ReceiveFileData

ReceiveFileInfo y ReceiveFileData son la contraparte de los métodos SendFileInfo  y SendFileData, y reciben la información enviada por estos.


### Parser

KOI es un parser que desarrollamos inspirándonos en los métodos de JSON Stringify y Parse. El método Stringify pasa un objeto y sus atributos a una representación en forma de string como vimos en clase la cual separa los diferentes atributos y sus valores con una cierta cantidad “#” (SplitToken). Este carácter se considera reservado por lo cual el cliente no lo puede usar en sus mensajes.

Stringify recibe un objeto y recorre sus propiedades. Si la propiedad es primitiva, la agrega al string resultante con cierto formato definido por nosotros. Agrega tanto el nombre de la propiedad como su valor, de forma que luego es recuperable por el método Parse empleando el nombre como clave. Si el valor es una lista, se agrega el nombre de la propiedad y el tipo de la lista, seguido por una concatenación de caracteres que facilitan el Parseo posterior llamada ListSuffix. Se concatenan los ítems de la lista llamando Stringify de forma recursiva sobre cada uno de ellos . Si la propiedad fuera un objeto, se concatena llamando Stringify de forma recursiva sobre este.

Los diferentes objetos resultantes se separan por tres SplitToken, cada una de sus propiedades se separa por dos SplitToken, y dentro de ellas, la clave y valor de cada propiedad se separa por un SplitToken.

El método Parse deshace lo que Stringify hace. Esto lo logra almacenando los distintos atributos y sus valores en un diccionario de tipo &lt;string,object> y tiene métodos públicos auxiliares para recuperar valores de listas o de objetos.

Para cada clave del diccionario, guarda o bien un valor primitivo, o bien un objeto, o bien una lista.

Primero, separa el string en los diferentes objetos (triple SplitToken), para cada uno de ellos evalúa su tipo. Si es una lista, desarrolla una lógica especial para su Parseo, si es un objeto, itera sus atributos (doble SplitToken) y agrega al diccionario su clave y valor (un SplitToken).


# 


### Descripción final de la arquitectura
![image](https://github.com/MateoGiraz/network-programming/assets/100039777/f430e9b8-e010-42b7-b50a-65bd5851d767)

Este diagrama de paquetes muestra una vista general de la arquitectura de nuestro proyecto, que facilita la comprensión y aclara la comunicación entre las distintas capas de las soluciones. El cliente emplea common para comunicarse con el servidor, que también emplea common en su paquete server-connection para recibir peticiones y enviar respuestas. Este paquete dispara métodos de la lógica de la aplicación, que usa los modelos de core-business (el paquete más estable de la solución, que no depende de ningún otro). Almacena estos objetos en un repositorio en memoria implementado en memory-repository y accedido a través de diferentes interfaces estables.


# Protocolo

Nuestro protocolo está diseñado para facilitar la comunicación entre el cliente y el servidor mediante peticiones. Estas peticiones especifican una operación o comando que el cliente desea que el servidor realice. Dado que es el servidor el que responde a las solicitudes y no las genera, decidimos simplificar el protocolo omitiendo cualquier indicador que diferencie entre petición y respuesta en los mensajes. Esta decisión se basa en la premisa de que únicamente los clientes envían peticiones y solo el servidor proporciona respuestas..

Cuando se genera una petición, el primer dato transmitido es el comando específico a ejecutar. Dependiendo de este comando, el servidor lo procesa de manera diferente. A continuación, se especifica la longitud de la información a transmitir, seguido de los datos concretos relacionados con el comando en cuestión.


<table>
  <tr>
   <td>Número de Comando
   </td>
   <td>Acción 
   </td>
  </tr>
  <tr>
   <td>1
   </td>
   <td>Creación del Usuario
   </td>
  </tr>
  <tr>
   <td>2 
   </td>
   <td>Login del Usuario
   </td>
  </tr>
  <tr>
   <td>3
   </td>
   <td>Compra de un producto
   </td>
  </tr>
  <tr>
   <td>4
   </td>
   <td>Creación de un producto
   </td>
  </tr>
  <tr>
   <td>5
   </td>
   <td>Modificar un producto
   </td>
  </tr>
  <tr>
   <td>6
   </td>
   <td>Dar de baja un producto
   </td>
  </tr>
  <tr>
   <td>7
   </td>
   <td>Devolver la lista de productos
   </td>
  </tr>
  <tr>
   <td>8
   </td>
   <td>Consultar un producto en específico
   </td>
  </tr>
  <tr>
   <td>9
   </td>
   <td>Calificar un producto
   </td>
  </tr>
</table>


En nuestra biblioteca Common hemos diseñado un Data Transfer Object (DTO) denominado ResponseDTO. Este DTO se utiliza para transmitir la respuesta del servidor al cliente. Incluye un "StatusCode" para indicar el resultado de la petición, además de contener un mensaje descriptivo del resultado(los Status code pueden ser 200 la petición fue aceptada con éxito, 400 la petición fue mal hecha por el cliente, 500 el servidor tuvo algún problema). En función de esta respuesta, la aplicación del cliente puede tomar las acciones pertinentes.
