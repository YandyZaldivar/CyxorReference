namespace Alimatic.DataDin2.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    [Controller(Route = "datadin2 user-model")]
    //class UserModelController : Controller<UserModel, int, int, UserModelApiModel, DataDin2DbContext> { }
    class UserModelController : Controller<UserModel, DataDin2DbContext> { }
}
