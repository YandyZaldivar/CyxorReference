namespace Alimatic.DataDin2.Controllers
{
    using Data;
    using Models;

    using Cyxor.Controllers;

    [Controller(Route = "datadin2 user-role")]
    //class UserRoleController : Controller<UserRole, int, int, UserRoleApiModel, DataDin2DbContext> { }
    class UserRoleController : Controller<UserRole, DataDin2DbContext> { }
}
