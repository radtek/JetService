using JetService.IntegrationTests.Executable;
using NUnit.Framework;

namespace JetService.IntegrationTests.Tests
{
  [TestFixture, Ignore("Service installer should check/add Log on as service permission")]
  public class RunUnderUserTest : RunUnderServiceTestBase
  {
    protected override void ExecuteTestImpl(TestAction testAction, string[] argz, OnServiceInstalled onInstalled)
    {
      UserManagement.WithNewUser(UserGroup.Admin,
                                 u => InstallRemoveService(
                                   A("/user=" + u.UserName, "/password=" + u.Password),
                                   testAction,
                                   argz,
                                   onInstalled));
    }
  }
}