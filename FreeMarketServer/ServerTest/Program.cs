using ServerConnection;
using Common;
using BusinessLogic;
using CoreBusiness;
using MemoryRepository;

Startup.PrintWelcomeMessageServer();
var app = new Server();
app.Listen(3000);


