using ServerConnection;
using Common;

Startup.PrintWelcomeMessageServer();

var app = new Server();
app.Listen(3000);
