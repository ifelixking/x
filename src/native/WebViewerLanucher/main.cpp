#include "stdafx.h"

int main(int argc, char ** argv) {

	QApplication a(argc, argv);
	QWebEngineView * pdev = new QWebEngineView();
	pdev->setUrl(QUrl("about:blank"));
	pdev->show();
	

	QWebEngineView * pv = new QWebEngineView();
	pv->setUrl(QUrl("http://www.baidu.com"));
	pv->show();
	pv->page()->setDevToolsPage(pdev->page());
	return a.exec();

}