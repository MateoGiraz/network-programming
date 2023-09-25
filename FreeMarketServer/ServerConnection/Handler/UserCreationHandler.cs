using System.Net.Sockets;
using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using CoreBusiness;

namespace ServerConnection.Handler;

public class UserCreationHandler
{
    internal static void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var userString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        var userMap = KOI.Parse(userString);

        var userDTO = new UserDTO()
        {
            UserName = userMap["UserName"].ToString(),
            Password = userMap["Password"].ToString()
        };
        Thread.Sleep(1000);
        OwnerController oc = new OwnerController();
        String response=oc.LogIn(userDTO.UserName, userDTO.Password);
        
        /*String response="";
        try
        {
            oc.LogIn(userDTO.UserName, userDTO.Password);
            
        }
        catch (BusinessLogicException ex)
        {
            Console.WriteLine("Error :" + ex.Message);
            response = ex.Message;
        }*/


        List<Owner> owners = oc.GetOwners();
        foreach (var owner in owners)
        {
            Console.WriteLine("Usuario:" + owner.UserName);
        }


        Console.WriteLine("Received Username: " + userDTO.UserName);
        Console.WriteLine("Received Password: " + userDTO.Password);
        
        //Acá le responde el server al cliente.
        //De momento estoy usando el UserDTO porque no le se a enviar Strings asi nomás
        //Capaz habría que hacer un ResponseDTO para las respuestas
        var responseDTO = new UserDTO()
        {
            UserName = userDTO.UserName,
            Password = response
        };

        var responseData = KOI.Stringify(responseDTO);
        var responseMessageLength = ByteHelper.ConvertStringToBytes(responseData).Length;
        
        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(responseMessageLength), socket);
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(responseData), socket);
        //SendLength(socket, responseMessageLength);
        //SendData(socket, userData);
    }



}