namespace Alimatic.Coralsa.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    [Controller(Route = "coralsa user-model")]
    //class UserModelController : Controller<UserModel, int, int, UserModelApiModel, CoralsaDbContext> { }
    class UserModelController : Controller<UserModel, CoralsaDbContext> { }
}
