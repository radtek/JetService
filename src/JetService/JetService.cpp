// JetService.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "resource.h"
#include "Command.h"
#include "ConsoleAction.h"
#include "SimpleConsoleWriter.h"
#include "DeleteServiceAction.h"
#include "DeleteByNameAction.h"
#include "CreateServiceAction.h"
#include "ProcessAction.h"
#include "ServiceAction.h"
#include "ConsoleCommandsRunner.h"
#include "ValidateCreateServiceAction.h"
#include "ValidateServiceTaskAction.h"
#include "ValidateLogonSIDAction.h"
#include "GrantLogonAsServiceAction.h"
#include "CheckUserAction.h"
#include "GrantServiceDACLAction.h"

#include "Logger.h"
const Logger LOG(L"main");

int _tmain(int argc, _TCHAR* argv[])
{
  SimpleConsoleWriter sConsole;
  ConsoleWriter* console = &sConsole;
  CString version;
  version.Append(L"JetBrains JetService v");
  version.AppendFormat(IDS_JET_SERVICE_VERSION);
  console->Write(version);

  Argz az(argc, argv);

  DeleteServiceAction deleteAction;
  DeleteByNameAction  deleteByNameAction;
  CreateServiceAction createAction;
  ServiceAction       serviceAction;
  ProcessAction       processAction;
  CheckUserAction     checkUserAction;
  GrantLogonAsServiceAction grantUserAction;
  GrantServiceDACLAction grantServiceAction;
  ValidateCreateServiceAction validateAction;
  ValidateServiceTaskAction validateTaskAction;
  ValidateLogonSIDAction validateSIDAction;
  

  ConsoleAction* actions[] = {
    &serviceAction,
    &createAction, 
    &deleteAction,
    &deleteByNameAction,
    &processAction,
    &checkUserAction,
    &grantUserAction,
    &grantServiceAction,
    &validateAction,
    &validateTaskAction,
    &validateSIDAction,
    NULL
  };

  return (ConsoleCommandsRunner(console, &az, actions)).executeCommand();
}

