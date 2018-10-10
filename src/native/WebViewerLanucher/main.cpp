#include "stdafx.h"

int main(int argc, char ** argv) {

	QCoreApplication::setAttribute(Qt::AA_EnableHighDpiScaling);
	QApplication app(argc, argv);

	QWebEngineView view;

	view.setUrl(QUrl(QStringLiteral("http://www.sina.com.cn")));
	view.resize(1024, 768);
	view.show();

	return app.exec();

}