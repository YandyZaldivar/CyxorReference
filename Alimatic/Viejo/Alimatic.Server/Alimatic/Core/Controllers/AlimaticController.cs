namespace Alimatic
{
    using Models;

    using Cyxor.Controllers;

    class AlimaticController : Controller
    {
        [Action(typeof(LicenseApiModel))]
        public string GetLicense() => "TODO: [...]";
    }
}
