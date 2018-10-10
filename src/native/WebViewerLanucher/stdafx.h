#pragma once


// #include <windows.h>
#include <QtWidgets/QApplication>
#include <QtWebEngineWidgets/QWebEngineView>

#ifdef _DEBUG
#pragma comment(lib, "Qt5Cored.lib")
#pragma comment(lib, "Qt5Widgetsd.lib")
#pragma comment(lib, "Qt5Networkd.lib")
#pragma comment(lib, "Qt5WebEngineCored.lib")
#pragma comment(lib, "Qt5WebEngineWidgetsd.lib")
#else
#pragma comment(lib, "Qt5Core.lib")
#pragma comment(lib, "Qt5Widgets.lib")
#pragma comment(lib, "Qt5Network.lib")
#pragma comment(lib, "Qt5WebEngineCore.lib")
#pragma comment(lib, "Qt5WebEngineWidgets.lib")
#endif // DEBUG


