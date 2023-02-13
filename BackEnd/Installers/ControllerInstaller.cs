namespace BackEnd.Installers
{
    public class ControllerInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
             builder.Services.AddControllers();
        }
    }
}
