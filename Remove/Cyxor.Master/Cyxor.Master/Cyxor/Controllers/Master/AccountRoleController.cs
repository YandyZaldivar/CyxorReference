namespace Cyxor.Controllers
{
    using Data;
    using Models;

    //class AccountRoleController : Controller<AccountRole, int, int, AccountRoleApiModel, MasterDbContext> { }
    class AccountRoleController : AccountDbContextController<AccountRole> { }
}
