#include <windows.h>
#include <QtWidgets/QApplication>
#include <QtWebEngineWidgets/QWebEngineView>

QApplication * g_app;


BOOL WINAPI DllMain(
	_In_ HINSTANCE hinstDLL,
	_In_ DWORD     fdwReason,
	_In_ LPVOID    lpvReserved
) {
	switch (fdwReason)
	{
	case DLL_PROCESS_ATTACH: {
		// g_app = new QApplication(__argc, __argv);
	}break;
	case DLL_PROCESS_DETACH: {}
	default:
		break;
	}
}

