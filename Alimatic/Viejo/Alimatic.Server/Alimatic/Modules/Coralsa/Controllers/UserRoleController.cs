namespace Alimatic.Coralsa.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    [Controller(Route = "coralsa user-role")]
    //class UserRoleController : Controller<UserRole, int, int, UserRoleApiModel, CoralsaDbContext> { }
    class UserRoleController : Controller<UserRole, CoralsaDbContext> { }
}
