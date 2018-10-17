#pragma once

#include <QtWebEngineWidgets/QWebEngineView>

namespace WebViewer {

	class WebView_Native : public QObject {
		Q_OBJECT
	public:
		WebView_Native(void * host, QWebEngineView * view);
		~WebView_Native();

	public slots :
		void loadFinished(bool ok);
		void contentsSizeChanged(const QSizeF &size);
		void geometryChangeRequested(const QRect &geom);
		void linkHovered(const QString &url);
		void loadProgress(int progress);
		void loadStarted();
		void scrollPositionChanged(const QPointF &position);
		void selectionChanged();
		void titleChanged(const QString &title);
		void urlChanged(const QUrl &url);
		void windowCloseRequested();

	private:
		void * m_host;
		QWebEngineView * m_view;
		class JsContext * m_jsContext;
		QWebChannel * m_webChannel;
	};

	class JsContext : public QObject
	{
		Q_OBJECT
	public:
		explicit JsContext(QObject *parent = nullptr) :QObject(parent) {}

	signals:
		void onJavaScriptInvoke(const QString & type, const QString & param);
		
	public slots:
		void jsInvoke(const QString & type, const QString & param) { emit onJavaScriptInvoke(type, param); }
	};

}